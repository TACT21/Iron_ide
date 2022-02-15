using System.Text;
using System;
using System.Net;
using Microsoft.JSInterop;

namespace Ide.script
{
    public class Engine
    {
        public string Test(IJSRuntime jSRuntime, string order = "Waiting input something >>>")
        {
            Console.WriteLine("Test");
            var a = ((IJSInProcessRuntime)jSRuntime).Invoke<string>("Input");
            Console.WriteLine("Test");
            return a;
        }
    }
}
