using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;
using Syncfusion.Blazor.RichTextEditor;
using IronPython.Hosting;

namespace Ferrum
{
    public partial class Coder: ComponentBase
    {
        public string inline = string.Empty;
        public string molding =string.Empty;
        public string console = string.Empty;
        public string input = string.Empty;
        public string q = string.Empty;
        public void Mold()
        {
            molding = Regex.Replace(inline, "<[^>]*?>", "");
            molding = molding.Replace("print", "Utility.print");
            var u = new Utility();
            var eng = Python.CreateEngine();
            var scope = eng.CreateScope();
            scope.SetVariable("Utility", u);
            eng.Execute(molding, scope);
            dynamic greetings = scope.GetVariable("greetings");
            var result = greetings("world");
            Console.WriteLine(result);
        }
        public void New()
        {
            if (inline == string.Empty)
            {

            }
        }
        public async Task<string> Readline_core(string m)
        {
            q = m;
            while (true)
            {
                if (input != string.Empty)
                {
                    break;
                }
                Task.Delay(1000);
            }
            var result = input;
            input = string.Empty;
            return result;
        }
    }

    public class Utility
    {
        public static string? Readline_alt(string mess)
        {
            var a = new Coder();
            return a.Readline_core(mess).Result;
        }
    }
}
