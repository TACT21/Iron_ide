using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using IronIde.Components;

Console.WriteLine("Hello, Browser!");

public partial class MyClass
{
    [JSExport]
    internal static string Greeting()
    {
        var text = $"Hello, World! from {GetHRef()}";
        Console.WriteLine(text);
        return text;
    }

    [JSImport("window.location.href", "main.js")]
    internal static partial string GetHRef();

    /// <summary>
    /// 実行環境初期化エージェント
    /// Main.jsから呼ばれる
    /// </summary>
    [JSExport]
    internal static async Task Ignition()
    {
        Dictionary<string, Func< dynamic[],Task <dynamic?>>> funcs = new()
        {
            { "input", IngnitionFuncHelper.InputAgent},
            { "error", IngnitionFuncHelper.ErrorPrintAgent},
            { "print", IngnitionFuncHelper.PrintAgent }
        };
        EngineSettings settings = new EngineSettings();
        foreach (var item in funcs.Keys)
        {
            settings.EventName.AddLast(item);
        }
        var engine = new Engine(settings);
        var script = GetScript();
        Console.WriteLine($"Call engine making process @ thread #{Thread.CurrentThread.ManagedThreadId}");
        var newThread = new Thread(engine.Ignition);
        newThread.Start(script);
        Console.WriteLine($"Ignition request has been ordered @ thread #{Thread.CurrentThread.ManagedThreadId}");
        while (true)
        {
            await Task.Delay(500);
            string json = "";
            if(EngineBridge.from == EngineBridge.From.Engine)
            {
                json = EngineBridge.ReadBridge();
                dynamic? result = null;
                if (json != string.Empty)
                {
                    try
                    {
                        var action = JsonSerializer.Deserialize<FuncCapsule>(json);
                        if (action != null)
                        {
                            result = await funcs[action.name](action.args);
                        }
                    }
                    catch (Exception ex) { 
                        Console.Error.WriteLine(ex);
                    }
                }
                Console.WriteLine($"Task calling request has been received @ thread #{Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine($"Type: {(result != null ? result.GetType().Name:"NULL")}");
                ResultCapule capule = new ResultCapule();
                capule.SetValue(result);
                Console.WriteLine(JsonSerializer.Serialize<ResultCapule>(capule));
                EngineBridge.WriteBridge(JsonSerializer.Serialize<ResultCapule>(capule));
                EngineBridge.from = EngineBridge.From.Owner;
                Console.WriteLine($"Mission completed @ thread #{Thread.CurrentThread.ManagedThreadId}");
            }
        }
    }

    internal static class IngnitionFuncHelper
    {
        public static async Task<dynamic?> InputAgent(dynamic[] args)
        {
            return await ReadInput(args);
        }
        public static async Task<string> ReadInput(dynamic[] args, int waitSec = 0)
        {
            Console.WriteLine("Reading...");
            if (args.Length == 0)
            {
                InputInvoke("");
            }
            else
            {
                var mess = "";
                foreach (var item in args)
                {
                    mess += item.ToString();
                }
                Console.WriteLine(mess);
                InputInvoke(mess);
            }
            int i = 0;
            string result = "";
            while (i <= waitSec)
            {
                i++;
                await Task.Delay(1000);
                result = GetInput();
                if (result != "")
                {
                    break;
                }
                if (waitSec == 0)
                {
                    i = 0;
                }
            }
            return result;
        }
        public static async Task<dynamic?> PrintAgent(dynamic[] args)
        {
            var mess = "";
            foreach (var item in args)
            {
                mess += item.ToString();
            }
            AddConsole(mess);
            return null;
        }
        public static async Task<dynamic?> ErrorPrintAgent(dynamic[] args)
        {
            var mess = "";
            foreach (var item in args)
            {
                mess += item.ToString();
            }
            AddConsole($"<p style = \"color:#ef857d\">{mess}</p>");
            return null;
        }
    }

    [JSExport]
    internal static async Task Compere()
    {
        var funcHelper = new CompereFuncHelper();
        Dictionary<string, Func<dynamic[], Task<dynamic?>>> funcs = new()
        {
            { "input", funcHelper.ReadInput},
            { "error", funcHelper.ErrorPrintAgent},
            { "print", funcHelper.PrintAgent }
        };
        EngineSettings settings = new EngineSettings();
        foreach (var item in funcs.Keys)
        {
            settings.EventName.AddLast(item);
        }
        var engine = new Engine(settings);
        var script = GetScript();
        funcHelper.action = () =>
        {
            funcHelper.isCandidate = true;
            var newThread = new Thread(engine.Ignition);
            newThread.Start(script);
        };
        Console.WriteLine($"Call engine making process @ thread #{Thread.CurrentThread.ManagedThreadId}");
        var newThread = new Thread(engine.Ignition);
        newThread.Start(script);
        Console.WriteLine($"Ignition request has been ordered @ thread #{Thread.CurrentThread.ManagedThreadId}");
        while (true)
        {
            await Task.Delay(500);
            string json = "";
            if (EngineBridge.from == EngineBridge.From.Engine)
            {
                json = EngineBridge.ReadBridge();
                dynamic? result = null;
                if (json != string.Empty)
                {
                    try
                    {
                        var action = JsonSerializer.Deserialize<FuncCapsule>(json);
                        if (action != null)
                        {
                            result = await funcs[action.name](action.args);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex);
                    }
                }
                Console.WriteLine($"Task calling request has been received @ thread #{Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine($"Type: {(result != null ? result.GetType().Name : "NULL")}");
                ResultCapule capule = new ResultCapule();
                capule.SetValue(result);
                Console.WriteLine(JsonSerializer.Serialize<ResultCapule>(capule));
                EngineBridge.WriteBridge(JsonSerializer.Serialize<ResultCapule>(capule));
                EngineBridge.from = EngineBridge.From.Owner;
                Console.WriteLine($"Mission completed @ thread #{Thread.CurrentThread.ManagedThreadId}");
            }
        }
    }

    internal class CompereFuncHelper
    {
        /// <summary>
        /// 被験者のコンソール
        /// </summary>
        internal LinkedList<string> outputs;

        /// <summary>
        /// 試験車の模範解答
        /// </summary>
        internal string answer;

        /// <summary>
        /// 被験者の回答
        /// </summary>
        internal string result;

        internal string[] inputs;
        private int count = 0;
        internal bool isCandidate;
        internal Action action { get; set; }
        private void AddOutPut(string mess)
        {
            if (isCandidate)
            {
                outputs.AddLast(mess);
            }
        }

        internal async Task<dynamic?> ReadInput(dynamic[] args)
        {
            string? result = null;
            if(count > inputs.Length)
            {
                AddOutPut($"Input:null");
                AddOutPut($"Warning:Unexpected reading stdin, so console helper provide null with program");
            }
            else
            {
                result = inputs[count];
            }
            count ++;
            AddOutPut($"Input:{result}");
            return result;
        }
        internal async Task<dynamic?> PrintAgent(dynamic[] args)
        {
            var mess = "";
            foreach (var item in args)
            {
                mess += item.ToString();
            }
            AddOutPut($"Output:{mess}");
            AddConsole(mess);
            if(args == new object[] { "<p style = \"color:#7df0a3\">", "All Tasks is completed.", "</p>" })
            {
                action();
            }
            return null;
        }
        internal async Task<dynamic?> ErrorPrintAgent(dynamic[] args)
        {
            var mess = "";
            foreach (var item in args)
            {
                mess += item.ToString();
            }
            AddOutPut($"Error:{mess}");
            AddConsole($"<p style = \"color:#ef857d\">{mess}</p>");
            return null;
        }
    }

    [JSImport("ironPython.getInput", "main.js")]
    internal static partial string GetInput();

    [JSImport("ironPython.askQuestion", "main.js")]
    internal static partial void InputInvoke(string mess);

    [JSImport("ironPython.addConsole", "main.js")]
    internal static partial void AddConsole(string mess);

    [JSImport("ironPython.clearQuestion", "main.js")]
    internal static partial void ClearQuestion(string mess);

    [JSImport("ironPython.getScript", "main.js")]
    internal static partial string GetScript();

}
  