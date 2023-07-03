using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
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
    /// IronPython を web assembly 上で動かすためのセット。
    /// </summary>
    [JSExport]
    internal static void Ignition()
    {
        //置換対象文字列 ,関数　のセット
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
        new Thread(engine.Ignition(script)).Start();
        Console.WriteLine($"Ignition request has been ordered by thread #{Thread.CurrentThread.ManagedThreadId}");
    }

    [JSImport("ironPython.getInput", "main.js")]
    internal static partial string GetInput();

    [JSImport("ironPython.askQuestion", "main.js")]
    internal static partial void InputInvoke(string a);

    [JSImport("ironPython.addConsole", "main.js")]
    internal static partial void AddConsole(string a);

    [JSImport("ironPython.clearQuestion", "main.js")]
    internal static partial void ClearQuestion(string a);

    [JSImport("ironPython.getScript", "main.js")]
    internal static partial string GetScript();

    public static async Task<dynamic> InputAgent(dynamic[] args)
    {
        return ReadInput(args);
    }
    public static async Task<string> ReadInput(dynamic[] args,int waitSec = 0)
    {
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

    }
}
