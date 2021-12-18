using System;
using System.Collections.Generic;
using System.Linq;
using IronPython.Hosting;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Match matche = Regex.Match("a = input('a')", "[\(].+?[\)]");
            Console.WriteLine(matche);
        }

        void python()
        {
            string sauce = @"
print('test')
a = scope.Readline_alt('test')
print(a)
";
            var u = new Utility();
            var eng = Python.CreateEngine();
            var scope = eng.CreateScope();
            scope.SetVariable("scope", u);
            eng.Execute(sauce, scope);
            Console.WriteLine("end");
        }
    }

    public class Utility
    {
        public static string Readline_alt(string mess)
        {
            Console.Write(mess + " >>>");
            return Temp.Read().Result;
        }
    }
    public static class Temp
    {
        public static async Task<string> Read()
        {
            Console.WriteLine("console");
            return await Task.Run(() => {
                return Console.ReadLine();
            });
        }
    }

}