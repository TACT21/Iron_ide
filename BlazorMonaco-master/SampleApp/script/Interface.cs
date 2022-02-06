using System;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
namespace SampleApp.script
{
    public static class Interface
    {
        public static IJSRuntime jSRuntime { set; get; }
        public static string cmd { set; get; }
        public static Action clear;
        public static Action<string> set;

        [JSInvokable]
        public static string Getinput()
        {
            return cmd;
        }

        [JSInvokable]
        public static void Clearinput()
        {
            Console.WriteLine("called");
            //clear();
        }

        [JSInvokable]
        public static void Setinput(string mess)
        {
            set(mess);
        }
    }        
}
