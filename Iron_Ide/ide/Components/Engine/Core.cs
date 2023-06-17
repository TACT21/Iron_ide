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
using IronPython.Hosting;
using System.Text;
using System.Runtime.Serialization;
using BlazorWorker.WorkerBackgroundService;

namespace ide.Components.Engine
{
    public class Core
    {
        /// <summary>
        /// When you use interactive functions, you must set this value before ignition
        /// </summary>
        public IJSInProcessRuntime? JSRuntime {
            set {
                if (IsRunning) { 
                    throw new IOException(); 
                }
                this.JSRuntime = value;
            }
            private get {
                return this.JSRuntime;
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
                if (IsRunning)
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
        public List<Assembly>? Assemblies
        {
            set
            {
                if (IsRunning)
                {
                    throw new IOException();
                }
                this.Assemblies = value;
            }
            private get
            {
                return this.Assemblies;
            }
        }
        public bool IsRunning { private set; get; } = false;
        /// <summary>
        /// function call agent. args is there:(id,position,args)
        /// </summary>
        public Action<string,string,object[]>? funcArgent { get; set; } = null;
        public async Task<Dictionary<string,object>> Ignition(string script,bool IsDebugging)
        {
            Microsoft.Scripting.Hosting.ScriptEngine scriptEngine;
            Microsoft.Scripting.Hosting.ScriptScope scriptScope;
            Microsoft.Scripting.Hosting.ScriptSource scriptSource;
            if (JSRuntime == null && Funcs != null && Funcs.Count > 0) { throw new NullReferenceException(); }
            if (IsRunning) { throw new IOException(); }
            var task = Transformer(script); 
/*            SortedDictionary<string, object> FuncsNames = new();
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
            }*/
            var runtime = Python.CreateRuntime();
            runtime.IO.SetInput(new MemoryStream(), Encoding.Default);
            scriptEngine = Python.GetEngine(runtime);
            foreach (var assembly in Assemblies)
            {
                scriptEngine.Runtime.LoadAssembly(assembly);
            }
            scriptScope = scriptEngine.CreateScope();
            scriptSource = scriptEngine.CreateScriptSourceFromString(await task);
            var utility = new IronUtility();
            scriptScope.SetVariable("IronPythonUtility", utility);
            scriptSource.Execute(scriptScope);
            var vars = scriptScope.GetVariableNames();
            Dictionary<string, object> result = new();
            if (IsDebugging)
            {
                foreach (var item in vars)
                {
                    object value;
                    if (scriptScope.TryGetVariable(item,out value))
                    {
                        result.Add(item, value);
                    }                
                }
            }
            return result;
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
        /// <param name="Funcs">JSRuntime</param>
        /// <param name="JSRuntime">Funcs (Allow default)</param>
        /// <param name="assemblies">assemblies (Allow default)</param>
        public void Initializer(LinkedList<(string, string, object)> Funcs, IJSInProcessRuntime JSRuntime,List<Assembly> assemblies)
        {
            if (IsRunning)
            {
                throw new IOException();
            }
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
                this.JSRuntime = JSRuntime;
            }
            catch (Exception e)
            {
                ex = e;
            }
            if(ex != null)
            {
                throw ex;
            }

            try
            {
                this.Assemblies = assemblies;
            }
            catch (Exception e)
            {
                ex = e;
            }
            if (ex != null)
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
        public IJSInProcessObjectReference? JSRuntime { set; get; }= null;
        public object? DoTask(string name, object[]args)
        {
            string id = Guid.NewGuid().ToString();
            string message = "DoAction" + id + "," + name + ",\"" + String.Join("\"\a\"",args)+"\"";
            if (this.Funcs[name] == null)
            {
                throw new NullReferenceException();
            }else if (this.Funcs[name].GetType().Name.IndexOf("Action") != -1)
            {
                JSRuntime.InvokeVoid("SessionStorageWrite", new string[] { Settings.FunctionBridgeName, message });
                return null;
            }
            else if (this.Funcs[name].GetType().Name.IndexOf("Func") != -1)
            {
                JSRuntime.InvokeVoid("SessionStorageWrite", new string[] { Settings.FunctionBridgeName, message });
                string result = string.Empty;
                for (int i = 0; i <= waitingSec;)
                {
                    result = JSRuntime.Invoke<string>("SessionStorageRead", new string[] { id });
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
                        this.JSRuntime.InvokeVoid("SessionStorageRemove", new string[] { id});
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
}
