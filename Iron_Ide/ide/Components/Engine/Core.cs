using Microsoft.JSInterop;
using BlazorWorker.BackgroundServiceFactory;
using BlazorWorker.Core;
using System.Text.RegularExpressions;
using IronPython.Compiler;
using IronPython.Runtime.Operations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Reflection;

namespace ide.Components.Engine
{
    public class Core
    {
        /// <summary>
        /// When you use interactive functions, you must set this value before ignition
        /// </summary>
        public IJSInProcessRuntime? jSRuntime {
            set {
                if (running) { 
                    throw new IOException(); 
                }
                this.jSRuntime = value;
            }
            private get {
                return this.jSRuntime;
            } 
        }
        /// <summary>
        /// The list of (string,string,object) type argument.
        /// each argument elements mean those
        /// Item #1: type is string & mean the word to identify each functions.
        /// Item #2: type is string & mean the word to replace each functions from the script.
        /// Item #3; type is dynamic (accept Func(object,object[]) type and Action(object[]) type)& mean the target function.
        /// </summary>
        public LinkedList<(string,string,dynamic)>? Funcs
        {
            set
            {
                if (running)
                {
                    throw new IOException();
                }
                this.Funcs = value;
            }
            private get
            {
                return this.Funcs;
            }
        }
        public bool running { private set; get; } = false;
        /// <summary>
        /// function call agent. args is there:(id,position,args)
        /// </summary>
        public Action<string,string,object[]>? funcArgent { get; set; } = null;
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="IOException"></exception>
        public async Task Ignition(string script)
        {
            if(jSRuntime == null && Funcs != null && Funcs.Count > 0) { throw new NullReferenceException(); }
            if (running) { throw new IOException(); }
            var task = Transformer(script); 
            SortedDictionary<string, object> FuncsNames = new();
            if (Funcs == null)
            {
                Funcs = new LinkedList<(string, string, object)>();
            }
            else
            {
                foreach (var item in Funcs)
                {
                    FuncsNames.Add(item.Item2, item.Item3);
                }
            }
            script = await task;
        }
        private async Task<string> Transformer (string script)
        {
            foreach (var item in Funcs)
            {
                var regex = "";
                foreach (var s in item.Item2)
                {
                    regex += "\\";
                    regex += ((int)s).ToString();
                }
                Regex rx = new Regex(regex + @"\s*\x28[^\x29]*\x28",
                  RegexOptions.Compiled | RegexOptions.IgnoreCase);
                var strings = rx.Matches(script);
                foreach (Match aim in strings)
                {
                    script = script.Replace(aim.Value, "IronPythonUtility.DoTask(" + item.Item1 + "," + aim.Value.replace("item.Item2", "").replace("(", "[").replace(")", "]") + ")");
                }
            }
            return script;
        }
        /// <summary>
        /// Initialize a instance after create this from this class.
        /// </summary>
        /// <param name="Funcs">jSRuntime</param>
        /// <param name="jSRuntime">Funcs</param>
        public void Initializer(LinkedList<(string, string, object)> Funcs, IJSInProcessRuntime jSRuntime)
        {
            Exception? ex = null;
            if(this.Funcs == null)
            {
                this.Funcs = new LinkedList<(string, string, object)>();
            }
            try
            {
                foreach (var item in Funcs)
                {
                    this.Funcs.AddLast(item);
                }
            }catch(Exception e)
            {
                ex = e;
            }
            try
            {
                this.jSRuntime = jSRuntime;
            }
            catch (Exception e)
            {
                ex = e;
            }
            if(ex != null)
            {
                throw ex;
            }
        }
    }
    class IronUtility
    {
        public uint waitingSec = 0;
        /// <summary>
        /// the sets of (function name, type)
        /// </summary>
        public  SortedList<string, dynamic?>? Funcs { get; private set; }
        public IJSInProcessObjectReference? jSRuntime { set; get; }= null;
        public object? DoTask(string name, object[]args)
        {
            string id = Guid.NewGuid().ToString();
            string sddressName = "ActionBridge";
            string message = "DoAction" + id + "," + name;
            if (this.Funcs[name] == null)
            {
                throw new NullReferenceException();
            }else if (this.Funcs[name].GetType().Name.IndexOf("Action") != -1)
            {
                jSRuntime.InvokeVoid("SessionStorageWrite", new string[] { sddressName, message });
                return null;
            }
            else if (this.Funcs[name].GetType().Name.IndexOf("Func") != -1)
            {
                jSRuntime.InvokeVoid("SessionStorageWrite", new string[] { sddressName, message });
                string result = string.Empty;
                for (int i = 0; i <= waitingSec;)
                {
                    result = jSRuntime.Invoke<string>("SessionStorageRead", new string[] { id });
                    if (result != null)
                    {
                        object? treasure = null;
                        //Write string into memory stream with specific encording
                        using (MemoryStream ms = new MemoryStream(Settings.defaultEncoding.GetBytes(result)))
                        {
                            if (Settings.takingRisk)
                            {
                                BinaryFormatter formatter = new BinaryFormatter();
                                // Deserialize the hashtable from the file and
                                // assign the reference to the local variable.
                                treasure = formatter.Deserialize(ms);
                            }
                            else
                            {
                                //Deserialize with XmlSerializer.
                                XmlSerializer serializer = new XmlSerializer(this.Funcs[name].GetMethodInfo().ReturnType);
                                treasure = serializer.Deserialize(ms);
                            }
                        }
                        return treasure;
                    } 
                    Thread.Sleep(1000);
                    if(waitingSec != 0)
                    {
                        i++;
                    }
                }
                return null;
            }
            return this.Funcs[name];
        }
    }

    public class Initializer { 
        Dictionary<string, dynamic> _Funcs;
        IJSRuntime jSRuntime;
        public async Task Initialize(IWorkerFactory workerFactory, LinkedList<(string, string, dynamic)> funcs, IJSRuntime jSRuntime, string script)
        {
            foreach (var item in funcs)
            {
                _Funcs.Add(item.Item1, item.Item3);  
            }
            this.jSRuntime = jSRuntime;
            // Create worker.
            var worker = await workerFactory.CreateAsync();
            // Create service reference. For most scenarios, it's safe (and best) to keep this 
            // reference around somewhere to avoid the startup cost.
            var service = await worker.CreateBackgroundServiceAsync<Core>();
            await service.RunAsync(s => s.Initializer(funcs, (IJSInProcessRuntime)jSRuntime));
            var result = await service.RunAsync(s => s.Ignition(script));
        }

        public async Task DoTask(string id,string name, object[]args)
        {
            string fomattee = String.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                if (_Funcs[name].GetMethodInfo().ReturnType.FullName.IndexOf("Task") != -1)
                {
                    var result = await _Funcs[name]();
                    if (Settings.takingRisk)
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(ms, result);
                    }
                    else
                    {
                        XmlSerializer serializer = new XmlSerializer(Type.GetType(result));
                        serializer.Serialize(ms, result);
                    }
                }
                else
                {
                    var result = await Task.Run(() => _Funcs[name]());
                    if (Settings.takingRisk)
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(ms, result);
                    }
                    else
                    {
                        XmlSerializer serializer = new XmlSerializer(Type.GetType(result));
                        serializer.Serialize(ms, result);
                    }
                }
                byte[] bytes = new byte[(int)ms.Length];
                await ms.ReadAsync(bytes, 0, (int)ms.Length);
                fomattee = Settings.defaultEncoding.GetString(bytes);
                await this.jSRuntime.InvokeVoidAsync("SessionStorageWrite", new string[] { id, fomattee }); 
            }
        }

    }

}
