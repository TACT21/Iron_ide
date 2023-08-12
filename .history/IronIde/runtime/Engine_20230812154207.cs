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

        public void Ignition(object? script,string assembliesOrder = String.Empty)
        {
            if(script != null) {
                Ignition(((string)script),assembliesOrder);
            }
            else
            {
                throw new ArgumentNullException(nameof(script));
            }
        }

        public void Ignition(string script, string assembliesOrder = String.Empty, bool Recycle = false)
        {
            //通例名,クラス名のセット
            Dictionary<string,string> importAim = new();  
            Console.WriteLine($"Create script @ thread #{Thread.CurrentThread.ManagedThreadId}");
            //import文取得
            Regex rx = new Regex(@"import\s*.*\s*as.*",
                  RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var strings = rx.Matches(script);
            foreach (Match aim in strings)
            {
                var value = aim.value.Replace("import","").spirit("as");
                importAim.Add(value[1].Trim(),value[0].Trim());
                script = script.Replace(aim.value,"");
            }
            rx = new Regex(@"import\s*.*",
                  RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var strings = rx.Matches(script);
            foreach (Match aim in strings)
            {
                var value = aim.value.Replace("import","");
                importAim.Add(value[1].Trim(),value[0].Trim());
                script = script.Replace(aim.value,"");
            }
            //スクリプト成形
            foreach (var item in settings.EventName)
            {
                
            }
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
            utility.DoTask("print", new object[] { "IronPythonIDE with Dynamic Language Runtime" });
            scriptScope.SetVariable("IronPythonUtility", utility);
            Console.WriteLine($"Ignition @ thread #{Thread.CurrentThread.ManagedThreadId}");
            try
            {
                scriptSource.Execute(scriptScope);
            }catch (Exception ex) {
                Console.Error.WriteLine(ex.Message, "\n@", ex.Source, "\n===StackTrace===\n", ex.StackTrace);
                utility.DoTask("print", new object[] { "<p style = \"color:#ef857d\">", ex.Message,"</p>"});
            }
            utility.DoTask("print", new object[] { "<p style = \"color:#7df0a3\">", "All Tasks is Compreate.", "</p>" });
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
        private void Replacer(string script, string aim){
            Regex rx = new Regex(item + @"\s*\x28.*\x29",
                  RegexOptions.Compiled | RegexOptions.IgnoreCase);
                var strings = rx.Matches(script);
            foreach (Match aim in strings)
            {
                script = 
                    script.Replace(aim.Value, "IronPythonUtility.DoTask(\"" + item + "\"," + aim.Value.Replace(item, "").Replace("(", "[").Replace(")", "]") + ")")//配列化
                    .Replace(",[]","");//空配列回避
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
    
    /// <summary>
    /// 実行オーダー格納用クラス
    /// </summary>
    public class FuncCapsule{
        /// <summary>
        /// UIプロセス側に登録された関数識別子
        /// </summary>
        public string name { set; get; } = string.Empty;
        /// <summary>
        /// 引数群
        /// </summary>
        public object[] args { set; get; } = null;
    }

    /// <summary>
    /// 結果格納用クラス
    /// </summary>
    public class ResultCapule
    {
        /// <summary>
        /// 返り値の Type の完全名
        /// シリアル化用にPublicだが、参照しないでください。
        /// </summary>
        public string type { set; get; } = string.Empty;
        /// <summary>
        /// 返り値のJsonシリアル
        /// シリアル化用にPublicだが、参照しないでください。
        /// </summary>
        public string resultJson { set; get; } = string.Empty;
        /// <summary>
        /// 返り値のJsonシリアル格納
        /// </summary>
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
            Console.WriteLine($"Write to capsule {resultJson}");
        }
        /// <summary>
        /// 返り値の取得
        /// </summary>
        public dynamic? GetValue()
        {
            Console.WriteLine($"Revert from capsule {type}");
            return JsonSerializer.Deserialize(resultJson, Type.GetType(type));
        }
    }

    public class IronUtility
    {
        public uint waitingSec = 0;

        /// <summary>
        /// 引数付外部変数実行プロセスのエントリポイント
        /// </summary>
        /// <param name="name">UIプロセス側に登録された関数識別子</param>
        /// <param name="args">引数</param>
        /// <returns>実行結果(型:実行時解決)</returns>
        public dynamic? DoTask(string name, IronPython.Runtime.PythonList args)
        {
            var property = new object[args.Count];
            for (int i = 0; i < args.Count; i++)
            {
                if (args[i] != null)
                {
                    property[i] = args[i];
                }
            }
            return DoTask(name, property);
        }

        /// <summary>
        /// 引数無し外部変数実行プロセスのエントリポイント
        /// </summary>
        /// <param name="name">UIプロセス側に登録された関数識別子</param>
        /// <returns>実行結果(型:実行時解決)</returns>
        public dynamic? DoTask(string name)
        {
            var property = new object[0];
            return DoTask(name, property);
        }

        /// <summary>
        /// 外部変数実行プロセス実行関数
        /// </summary>
        /// <param name="name">UIプロセス側に登録された関数識別子</param>
        /// <param name="args">引数</param>
        /// <returns>実行結果(型:実行時解決)</returns>
        public dynamic? DoTask(string name, object[] args)
        {
            FuncCapsule func = new FuncCapsule() { args = args, name = name };
            var order = JsonSerializer.Serialize<FuncCapsule>(func);
            EngineBridge.WriteBridge(order);
            Console.WriteLine($"order is claimed @ thread #{Thread.CurrentThread.ManagedThreadId}");
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
    }
}
