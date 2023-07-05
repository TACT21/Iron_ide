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
        new MemoryStream();
        //�u���Ώە����� ,�֐��@�̃Z�b�g
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
        var newThread = new Thread(engine.Ignition);
        newThread.Start(script);
        AddConsole($"Ignition request has been ordered by thread #{Thread.CurrentThread.ManagedThreadId}");
        while (true)
        {
            await Task.Delay(500);
            string json = "";
            if(EngineBridge.from == EngineBridge.From.Engine)
            {
                var bytes = new byte[EngineBridge.Bridge.Length];
                EngineBridge.Bridge.Read(bytes, 0, bytes.Length);
                json = EngineBridge.StandardEncoding.GetString(bytes);
                var action = JsonSerializer.Deserialize<FuncCapsule>(json);
                dynamic? result = null;
                if (action != null)
                {
                    result = await funcs[action.name](action.args);
                }
                ResultCapule capule = new ResultCapule();
                capule.SetValue(result);
                EngineBridge.Bridge.Write(
                    EngineBridge.StandardEncoding.GetBytes(
                        JsonSerializer.Serialize(capule)
                    )
                );
                EngineBridge.from = EngineBridge.From.Owner;
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
        var mess = "";
        foreach (var item in args)
        {
            mess += item.ToString();
        }
        AddConsole(mess);
        return null;
    }
}
