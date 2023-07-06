using IronPython.Compiler;
using IronPython.Hosting;
using IronPython.Runtime.Operations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;

namespace IronIde.Components
{
    public class Engine
    {
        EngineSettings settings = null!;
        LinkedList<(string,object)> Variables = new LinkedList<(string,object)> ();
        public Engine(EngineSettings engineSettings) {
            this.settings = engineSettings;
        }
        public Engine()
        {
            this.settings = new EngineSettings();
        }

        public void Ignition(object? script)
        {
            if(script != null) {
                Ignition(((string)script));
            }
            else
            {
                throw new ArgumentNullException(nameof(script));
            }
        }

        public void Ignition(string rawscript,bool Recycle = false)
        {
            string script = "IronPythonUtility.Test(str(hasattr(IronPythonUtility, 'DoTask')))\n";
            Console.WriteLine($"Create script @ thread #{Thread.CurrentThread.ManagedThreadId}");
            //スクリプト成形
            foreach (var item in settings.EventName)
            {
                
                Regex rx = new Regex(item + @"\s*\x28.*\x29",
                  RegexOptions.Compiled | RegexOptions.IgnoreCase);
                var strings = rx.Matches(script);
                foreach (Match aim in strings)
                {
                    rawscript = rawscript.Replace(aim.Value, "IronPythonUtility.DoTask(" + item + "," + aim.Value.replace(item, "").replace("(", "[").replace(")", "]") + ")");
                }
            }
            script += rawscript;
            Console.WriteLine($"===script===");
            Console.WriteLine(script);
            Console.WriteLine($"===script===");
            Console.WriteLine($"Create runtime @ thread #{Thread.CurrentThread.ManagedThreadId}");
            //エンジン 作成
            Microsoft.Scripting.Hosting.ScriptEngine scriptEngine;
            Microsoft.Scripting.Hosting.ScriptScope scriptScope;
            Microsoft.Scripting.Hosting.ScriptSource scriptSource;
            var runtime = Python.CreateRuntime();
            Console.WriteLine($"Create memory space @ thread #{Thread.CurrentThread.ManagedThreadId}");
            runtime.IO.SetInput(new MemoryStream(), Encoding.Default);
            Console.WriteLine($"Create engine @ thread #{Thread.CurrentThread.ManagedThreadId}");
            scriptEngine = Python.GetEngine(runtime);
            Console.WriteLine($"Load assemblies @ thread #{Thread.CurrentThread.ManagedThreadId}");
            foreach (var assembly in settings.Assemblies)
            {
                scriptEngine.Runtime.LoadAssembly(assembly);
            }
            Console.WriteLine($"Create scope @ thread #{Thread.CurrentThread.ManagedThreadId}");
            scriptScope = scriptEngine.CreateScope();
            Console.WriteLine($"Create source @ thread #{Thread.CurrentThread.ManagedThreadId}");
            scriptSource = scriptEngine.CreateScriptSourceFromString(script);
            Console.WriteLine($"Check the rely system @ thread #{Thread.CurrentThread.ManagedThreadId}");
            var utility = new IronUtility();
            utility.DoTask("print", new object[] { "the rely system seemed to be fine." });
            scriptScope.SetVariable("IronPythonUtility", utility);
            Console.WriteLine($"Ignition @ thread #{Thread.CurrentThread.ManagedThreadId}");
            try
            {
                scriptSource.Execute(scriptScope);
            }catch (Exception ex) {
                utility.DoTask("print", new object[] {ex.Message,"\n@",ex.Source, "\n===StackTrace===\n", ex.StackTrace});
            }
            //後始末
            if (Recycle)
            {
                var variableNames = scriptScope.GetVariableNames();
                foreach (var variableName in variableNames)
                {
                    Variables.AddLast((variableName,scriptScope.GetVariable(variableName)));
                }
            }
        }
    }

    public class EngineSettings
    {
        public List<Assembly> Assemblies { set; get; } = new();
        /// <summary>
        /// 代替対象関数名セット
        /// </summary>
        public LinkedList<string> EventName = new();
    }

    public static class EngineBridge
    {
        public static MemoryStream BridgeStream { set; get; } = new MemoryStream();
        public static string BridgeString { set; get; } = string.Empty;
        public static Encoding StandardEncoding { set; get; } = Encoding.UTF8;
        public static void WriteBridge(string mess)
        {
            /*var array = StandardEncoding.GetBytes(mess);
            Bridge.Write(array, 0, array.Length);*/
            BridgeString = mess;
        }
        public static string ReadBridge()
        {
            /*var array = new byte[Bridge.Length];
            Bridge.Read(array, 0, array.Length);
            return StandardEncoding.GetString(array);*/
            return BridgeString;
        }
        public static From from = From.Neither;
        public enum From
        {
            Engine,
            Owner,
            Neither
        }
    }

    public class FuncCapsule{
        public string name { set; get; } = string.Empty;
        public object[] args { set; get; } = null;
    }

    public class ResultCapule
    {
        private string type= string.Empty;
        private string resultJson = string.Empty;
        public void SetValue(dynamic? value)
        {
            if (value == null)
            {
                type = "System.Object";
            }
            else
            {
                resultJson = JsonSerializer.Serialize(value, value.GetType());
                type = value.GetType().FullName;
            }
        }
        public dynamic? GetValue()
        {
            return JsonSerializer.Deserialize(resultJson, Type.GetType(type));
        }
    }

    class IronUtility
    {
        public uint waitingSec = 0;
        public dynamic? DoTask(string name, object[] args)
        {
            FuncCapsule func = new FuncCapsule() { args = args, name = name };
            var order = JsonSerializer.Serialize<FuncCapsule>(func);
            EngineBridge.WriteBridge(order);
            Console.WriteLine($"order is claimed @ thread #{Thread.CurrentThread.ManagedThreadId}\n"+order);
            EngineBridge.from = EngineBridge.From.Engine;
            string json;
            while(true)
            {
                Thread.Sleep(1000);
                if (EngineBridge.from == EngineBridge.From.Owner)
                {
                    Console.WriteLine($"Result is received @ thread #{Thread.CurrentThread.ManagedThreadId}");
                    json = EngineBridge.ReadBridge();
                    break;
                }
            }
            dynamic? result;
            try
            {
                var capsule = JsonSerializer.Deserialize<ResultCapule>(json); 
                result = capsule.GetValue();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message + "\n we provide null to program. Stack trace is below.\n" + ex.StackTrace);
                return null;
            }
            return result;
        }
        public void Test(string mess)
        {
            Console.WriteLine(mess);
        }
    }
}
