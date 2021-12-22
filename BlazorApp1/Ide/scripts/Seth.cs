using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;
using Syncfusion.Blazor.RichTextEditor;
using IronPython.Hosting;
using System.Text;
using BlazorDownloadFile;
using Syncfusion.Blazor.Inputs;
using System.Net;
using System.Collections;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;


namespace Ferrum
{
    public partial class Coder: ComponentBase
    {
        [Inject]
        private IJSRuntime jsRuntime { get; set; }
        [Inject] public IBlazorDownloadFileService BlazorDownloadFileService { get; set; }
        public string molding =string.Empty;
        public string console = string.Empty;
        public string input = string.Empty;
        public string q = string.Empty;
        public bool debug = false;
        public SfRichTextEditor rteObj;
        public string inline = string.Empty;
        public SfMaskedTextBox maskedTextBox;
        public string path = string.Empty;
        public bool isSave = false;
        public bool isRun = false;
        public string output = string.Empty;
        public List<ToolbarItemModel> ToolsForInline = new List<ToolbarItemModel>()
        {
            new ToolbarItemModel() { Command = ToolbarCommand.Undo },
            new ToolbarItemModel() { Command = ToolbarCommand.Redo}
        };
        public void Start()
        {
            rteObj.EnableTabKey = true;
            rteObj.AutoSaveOnIdle = true;
            rteObj.EnablePersistence = true;
            rteObj.Height = "40vh";
        }

        public string Converter(string raw)
        {
            Console.WriteLine(raw+"@converter");
            string s = "Utility_port\r\nfrom time import sleep\r\n" + raw.Replace("&nbsp;&nbsp;&nbsp;&nbsp;", "\t");
            s = WebUtility.HtmlDecode(s);
            s = s.Replace("</p>", "\r\n");
            s = Regex.Replace(s, "<[^>]*?>", "");
            output = s;
            //TODO HOW TO Input "yield"
            MatchCollection matches = Regex.Matches(s, ".*?input\\u0028.*?\\u0029.*?");//Input functionの抽出
            foreach (Match m in matches)
            {
                Match matche = Regex.Match(m.Value, "\\u0028.*?\\u0029");//input(...)を抜き出す。
                Console.WriteLine("SOA");
                Console.WriteLine(matche);
                Console.WriteLine(m.Value);
                Console.WriteLine(m.Value.Split(matche.Value));
                Console.WriteLine("EOA");
                var arry = m.Value.Split(matche.Value);
                if(arry.Length > 0)
                {
                    s = s.Replace(m.Value, (char)95 + "input= Utility.Input" + matche.Value + "\n\rsleep(6)\r\n" + arry[0].Replace("input", "") + "Utility.return_input\n\rsleep(6)" + arry[1]);//input(...)らへんをᐁ変数で置き換えて…
                }
                else
                {
                    s = s.Replace(m.Value, (char)95 + "input=Utility.Input" + matche.Value + "\n\rsleep(6)\r\n" + arry[0].Replace("input", "") + "Utility.return_input\n\rsleep(6)");//input(...)らへんをᐁ変数で置き換えて…
                }
                
            }
            s = s.Replace("print", "Utility.Print");
            var a = new byte[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                a[i] = Convert.ToByte(s[i]);
            }
            Console.WriteLine(Encoding.UTF8.GetString(a));
            return Encoding.UTF8.GetString(a);
        }


        public void Mold()
        {
            molding = Converter(rteObj.Value);
            
            Console.WriteLine(molding);
            try
            {
                console += "<div class='bar'><p>" + DateTime.Now + "</p></div>";
                var u = new Utility();
                var runtime = Python.CreateRuntime();
                runtime.IO.SetInput(new MemoryStream(), Encoding.Default);
                var eng = Python.GetEngine(runtime);
                var scope = eng.CreateScope();
                scope.SetVariable("Utility", u);
                var source = eng.CreateScriptSourceFromString(molding);
                eng.Execute(molding, scope);
            }
            catch (Exception ex)
            {
                if (debug)
                {
                    console += "<pre style='color: red'>" + ex.ToString() + "</pre>";
                }
                else
                {
                    console += "<pre style='color: red'>" + ex.Message + "</pre>";
                }
            }
        }
        public void New()
        {
            if (rteObj.Value == string.Empty)
            {
                rteObj.Value = "<p>a = input\u0028\"TEST>>\"\u0029</p><p>print(a)</p>";
            }
        }
        /// <summary>
        /// Saving Context which contein rteObj.
        /// </summary>
        public async void Save()
        {
            await BlazorDownloadFileService.DownloadFileFromText("*.py", "# coding: utf-8\r\n\r\n" + rteObj.Value, System.Text.Encoding.UTF8, 3000, "application/octet-stream") ;
        }
        /// <summary>
        /// Set qyestion field
        /// </summary>
        /// <param name="m">question such as "input>>"</param>
        public void Question(string m)
        {
            Console.WriteLine(m);
            q = m;
        }
        /// <summary>
        /// Get input
        /// </summary>
        public void Input()
        {
            Utility_port.import_temp = maskedTextBox.Value;
        }
        /// <summary>
        /// Output console
        /// </summary>
        /// <param name="m">output text</param>
        public void Output(string m)
        {
            Console.WriteLine("callmain");
            console = console+ "\r\n" + m;
        }
    }
    /// <summary>
    ///  
    ///  
    /// </summary>
    ///<param name="oncheng_console">Console_messege</param>
    ///<param name="oncheng_input">Console_messege</param>
    ///<param name="console_temp">Console_messege</param>
    ///<param name="import_temp">Console_messege</param>
    public static class Utility_port
    {
        public static Action<string> oncheng_console;
        public static Action<string> oncheng_input;
        public static Func<Task> Wait;
        public static string import_temp = string.Empty;
        /// <summary>
        /// Input function
        /// </summary>
        /// <param name="m">Console_messege</param>
        /// <returns></returns>
        public static async Task<string> Readline_core(object m = null)
        {
            var forc = new Coder();
            oncheng_input(m.ToString());
            for (int i = 0; i < 5; i++)
            {
                if (import_temp != string.Empty)
                {
                    Console.WriteLine(import_temp);
                    break;
                }
                Console.WriteLine("Wait");
                await Task.Run(() => Task.Delay(1000));
            }
            var result = import_temp;
            import_temp = string.Empty;
            return result;
        }
        private static async Task<object> Readline_core_null() 
        { 
            oncheng_input("何かを入力してください");
            for (int i = 0; i < 10; i++)
            {
                if (import_temp != string.Empty)
                {
                    Console.WriteLine(import_temp);
                    break;
                }
            }
            var result = import_temp;
            import_temp = string.Empty;
            return result;
        }
        public static void Print_alt(object m = null)
        {
            Console.WriteLine("Write!");
            Console.WriteLine(m);
            if(m != null || m != string.Empty)
            {
                oncheng_console(m.ToString());
            }
            else
            {
                oncheng_console("Designated value is null");
            }
        }
    }



    public class Utility
    {
        public static string return_input = "NULLs";
        public async Task Input(string mess = null)
        {
            Console.WriteLine (mess);
            return_input = await Task.Run(() => Utility_port.Readline_core(mess));
        }

        public static string Get_input(string mess)
        {
            return Utility_port.import_temp;
        }

        public static void Set_q(string mess)
        {
            Utility_port.oncheng_input(mess);
        }

        public static void Print(object mess = null)
        {
            Console.WriteLine(mess);
            Utility_port.Print_alt(mess);
        }
    }
}
