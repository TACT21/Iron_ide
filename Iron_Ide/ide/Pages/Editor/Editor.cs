using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ide.Components.Engine;

namespace ide.Pages.Editor
{
    public class Editor:ComponentBase
    {
        [Inject]
        protected IJSInProcessRuntime JSRuntime { set; get; }
        /// <summary>
        /// The list of (string,string,object) type argument.
        /// each argument elements mean those
        /// Item #1: type is string & mean the word to identify each functions.
        /// Item #2: type is string & mean the word to replace each functions from the script.
        /// Item #3; type is dynamic (accept Func(object,object[]) type and Action(object[]) type)& mean the target function.
        /// </summary>
        public LinkedList<(string, string, dynamic)> funcs {private set; get; } = new ();

        public void Running()
        {
            var engine = new Initializer();
            var write = WriteConsole;
            funcs.AddLast(("WriteLine", "print", write));
        }
        public void WriteConsole(string[] args)
        {
            var mess = String.Join((char)0, args);
            Console.WriteLine(mess);
        }
    }
}
