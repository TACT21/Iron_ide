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
    /// IronPython �� web assembly ��œ��������߂̃Z�b�g�B
    /// </summary>
    [JSExport]
    internal static async Task Ignition()
    {
        Dictionary<string, Func< dynamic[],Task <dynamic?>>> funcs = new()
        {
            { "input", InputAgent},
            { "print",PrintAgent }
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

    public static async Task<dynamic?> InputAgent(dynamic[] args)
    {
        return await ReadInput(args);
    }
    public static async Task<string> ReadInput(dynamic[] args,int waitSec = 0)
    {
        Console.WriteLine("Reading...");
        if(args.Length == 0)
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
        while(i <= waitSec)
        {
            i++;
            await Task.Delay(1000);
            result = GetInput();
            if(result != "")
            {
                break;
            }
            if(waitSec == 0)
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
}
  