using System;
using System.Runtime.InteropServices.JavaScript;
using System.Threading;
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
        new Thread(SecondThread).Start();
        Console.WriteLine($"Hello, Browser from the main thread {Thread.CurrentThread.ManagedThreadId}");

        static void SecondThread()
        {
            Console.WriteLine($"Hello from Thread {Thread.CurrentThread.ManagedThreadId}");
            for (int i = 0; i < 5; ++i)
            {
                Console.WriteLine($"Ping {i}");
                Thread.Sleep(1000);
            }
        }
    }
}
