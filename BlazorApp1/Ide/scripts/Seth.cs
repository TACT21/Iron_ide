using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;
using Syncfusion.Blazor.RichTextEditor;
using IronPython.Hosting;
using System.Text;
using BlazorDownloadFile;
using Syncfusion.Blazor.Inputs;
using System.Net;

namespace Ferrum
{
    public partial class Coder: ComponentBase
    {
        [Inject] public IBlazorDownloadFileService BlazorDownloadFileService { get; set; }
        public string molding =string.Empty;
        public string console = string.Empty;
        public string input = string.Empty;
        public string q = string.Empty;
        public bool debug = false;
        //public SfRichTextEditor rteObj;
        public string inline = string.Empty;
        public SfMaskedTextBox maskedTextBox;
        public List<ToolbarItemModel> ToolsForInline = new List<ToolbarItemModel>()
        {
            new ToolbarItemModel() { Command = ToolbarCommand.Undo },
            new ToolbarItemModel() { Command = ToolbarCommand.Redo}
        };
        /*public void Start()
        {
            rteObj.EnableTabKey = true;
            rteObj.AutoSaveOnIdle = true;
            rteObj.EnablePersistence = true;
            rteObj.Height = "50vh";
        }*/

        public void Input()
        {
            input = maskedTextBox.Value;
            Console.WriteLine(maskedTextBox.Value);
        }

        public string Converter(string raw)
        {
            Console.WriteLine(raw);
            string s = raw.Replace("&nbsp;&nbsp;&nbsp;&nbsp;", "\t");
            s = WebUtility.HtmlDecode(s);
            s = s.Replace("</p>", "\r\n");
            s = Regex.Replace(s, "<[^>]*?>", "");
            s = s.Replace("input", "Utility.Input");
            s = s.Replace("print", "Utility.Print");
            var a = new byte[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                a[i] = Convert.ToByte(s[i]);
            }
            Console.WriteLine(Encoding.UTF8.GetString(a));
            return Encoding.UTF8.GetString(a);
        }

        protected override async void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                //Start();
            }
        }
        public void Mold()
        {
            //molding = Converter(rteObj.Value);
            Console.WriteLine(molding);
            try
            {
                console += "<div class='bar'><p>"+DateTime.Now+"</p></div>" ;
                var u = new Utility();
                var runtime = Python.CreateRuntime();
                runtime.IO.SetInput(new MemoryStream(), Encoding.Default);
                var eng = Python.GetEngine(runtime);
                var scope = eng.CreateScope();
                scope.SetVariable("Utility", u);
                var source = eng.CreateScriptSourceFromString(molding);
                eng.Execute(molding,scope);
            }
            catch (Exception ex)
            {
                if (debug)
                {
                    console += "<div style='color: red'>" + ex.ToString() + "</div>";
                }
                else
                {
                    console += "<div style='color: red'>" + ex.Message + "</div>";
                }
            }
        }
        /*public void New()
        {
            if (rteObj.Value == string.Empty)
            {

            }
        }*/


        public async Task<string> Readline_core(string m)
        {
            q = m;
            while (true)
            {
                if (input != string.Empty)
                {
                    Console.WriteLine(input);
                    break;
                }
                Task.Delay(1000);
            }
            var result = input;
            input = string.Empty;
            return result;
        }
        public async void Save()
        {
            //await BlazorDownloadFileService.DownloadFileFromText("*.py", "# coding: utf-8\r\n\r\n" + rteObj.Value, System.Text.Encoding.UTF8, 3000, "application/octet-stream") ;
        }

        public void Print_alt(string m)
        {
            Console.WriteLine(m);
            console = console+ "\r\n"+m;
        }
    }

    public class Utility
    {
        public static string? Input(string mess)
        {
            Console.WriteLine (mess);
            var a = new Coder();
            return a.Readline_core(mess).Result;
        }
        public static void Print(string mess)
        {
            var a = new Coder();
            a.Print_alt(mess);
        }
    }
}
