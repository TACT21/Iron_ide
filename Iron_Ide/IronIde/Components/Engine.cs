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

        public void Ignition(string script,bool Recycle = false)
        {
            Console.WriteLine($"Ignition at thread #{Thread.CurrentThread.ManagedThreadId}");
            //スクリプト成形
            foreach (var item in settings.EventName)
            {
                var regex = "";
                foreach (var s in item)
                {
                    regex += "\\";
                    regex += ((int)s).ToString();
                }
                Regex rx = new Regex(regex + @"\s*\x28[^\x29]*\x28",
                  RegexOptions.Compiled | RegexOptions.IgnoreCase);
                var strings = rx.Matches(script);
                foreach (Match aim in strings)
                {
                    script = script.Replace(aim.Value, "IronPythonUtility.DoTask(" + item + "," + aim.Value.replace(item, "").replace("(", "[").replace(")", "]") + ")");
                }
            }
            //エンジン 作成
            Microsoft.Scripting.Hosting.ScriptEngine scriptEngine;
            Microsoft.Scripting.Hosting.ScriptScope scriptScope;
            Microsoft.Scripting.Hosting.ScriptSource scriptSource;
            var runtime = Python.CreateRuntime();
            runtime.IO.SetInput(new MemoryStream(), Encoding.Default);
            scriptEngine = Python.GetEngine(runtime);
            foreach (var assembly in settings.Assemblies)
            {
                scriptEngine.Runtime.LoadAssembly(assembly);
            }
            scriptScope = scriptEngine.CreateScope();
            scriptSource = scriptEngine.CreateScriptSourceFromString(script);
            var utility = new IronUtility();
            scriptScope.SetVariable("IronPythonUtility", utility);
            scriptSource.Execute(scriptScope);
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
        public static object BridgeLocker { set; get; } = new object();
        public static MemoryStream Bridge { set; get; }
        public static Encoding StandardEncoding { set; get; } = Encoding.UTF8;
        public static From? from = null;
        public enum From
        {
            Engine,
            Owner
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
        public void SetValue(dynamic value)
        {
            resultJson = JsonSerializer.Serialize(value, value.GetType());
            type = value.GetType().FullName;
        }
        public dynamic GetValue()
        {
            return JsonSerializer.Deserialize(resultJson, Type.GetType(type));
        }
    }

    class IronUtility
    {
        public uint waitingSec = 0;
        public dynamic? DoTask(string name, object[] args)
        {
            FuncCapsule capsule = new FuncCapsule() { args = args, name = name };
            EngineBridge.Bridge.Write(
                EngineBridge.StandardEncoding.GetBytes(
                    JsonSerializer.Serialize<FuncCapsule>(capsule)
                )
            );
            EngineBridge.from = EngineBridge.From.Engine;
            Thread.Sleep(1000);
            string json = "";
            lock (EngineBridge.BridgeLocker)
            {
                var bytes = new byte[EngineBridge.Bridge.Length];
                EngineBridge.Bridge.Read( bytes, 0, bytes.Length );
                json = EngineBridge.StandardEncoding.GetString(bytes);
            }
            return JsonSerializer.Deserialize<ResultCapule>(json).GetValue();
        }
    }
}
