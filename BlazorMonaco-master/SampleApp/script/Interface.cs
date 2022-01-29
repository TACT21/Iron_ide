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
        public static NavigationManager navigationManager { get; set; }
        public static IJSRuntime jSRuntime { get; set; }
        //初期化済みか
        private static bool initialized = false;
        public static bool getinitialized { get; }
        public static Func<string> getinput { set; get; }

        static void Initialize()
        {
           var a = navigationManager != null ? navigationManager.BaseUri : null;
            //SetBaseUrl関数を行う
            jSRuntime.InvokeVoidAsync("SetBaseUrl", a);
            initialized = true;
        }

        static string Getinput()
        {
            return getinput();
        }

        public static string Callinput()
        {
            Console.WriteLine("Test");
            var a = ((IJSInProcessRuntime)jSRuntime).Invoke<string>("GetInput", Assembly.GetEntryAssembly().GetCustomAttribute<GuidAttribute>().Value);
            Console.WriteLine("Test");
            return a;
        }
    }
}
