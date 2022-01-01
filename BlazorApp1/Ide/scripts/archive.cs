using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;
using System.Text;
using System.Net;

namespace Ferrum.archive
{
    public class Old: ComponentBase
    {
        public string Converter(string raw)
        {
            Console.WriteLine(raw + "@converter");
            string s = "Utility_port\r\n" + raw.Replace("&nbsp;&nbsp;&nbsp;&nbsp;", "\t");
            s = WebUtility.HtmlDecode(s);
            s = s.Replace("</p>", "\r\n");
            s = Regex.Replace(s, "<[^>]*?>", "");
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
                if (arry.Length > 0)
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
    }
}
