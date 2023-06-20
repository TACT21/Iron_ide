using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ide.Components.Engine;
using BlazorWorker.Core;

namespace ide.Pages.Editor.VisualConsole.Front
{
    public class ConsoleManager : ComponentBase
    {
        [Parameter]
        public string Script { get; set; } = string.Empty;
        [Inject]
        protected IJSInProcessRuntime JSRuntime { set; get; }
        [Inject]
        protected IWorkerFactory WorkerFactory { set; get; }
        /// <summary>
        /// The list of (string,string,object) type argument.
        /// each argument elements mean those
        /// Item #1: type is string & mean the word to identify each functions.
        /// Item #2: type is string & mean the word to replace each functions from the script.
        /// Item #3; type is dynamic (accept Func(object,object[]) type and Action(object[]) type)& mean the target function.
        /// </summary>
        public LinkedList<(string, string, dynamic)> funcs { private set; get; } = new();
        public string ConsoleInput { get; set; } = string.Empty;
        public string ConsoleOutput { get; set; } = string.Empty;
        public string ConsoleAsk { get; set; } = string.Empty;

        private string WaitingReading = string.Empty;


        public async void Running()
        {
            var engine = new Initializer();
            var write = WriteConsole;
            var read = ReadConsole;
            funcs.AddLast(("WriteLine", "print", write));
            funcs.AddLast(("ReadLine", "input", read));
            await engine.Initialize(WorkerFactory, funcs, JSRuntime, Script);
        }
        public void WriteConsole(string[] args)
        {
            var mess = String.Join((char)0, args);
            System.Console.WriteLine(mess);
            ConsoleOutput += "<p>" + mess + "</p>";
        }
        public void EnterConsole()
        {
            WaitingReading = ConsoleInput;
        }

        public async Task<string> ReadConsole(string[] args)
        {
            if (args.Length != 0)
            {
                ConsoleAsk = String.Join((char)0, args);
            }
            string answer = string.Empty;
            while (true)
            {
                if (WaitingReading != string.Empty)
                {
                    answer = WaitingReading;
                    WaitingReading = string.Empty;
                    break;
                }
            }
            return answer;
        }
    }
}
