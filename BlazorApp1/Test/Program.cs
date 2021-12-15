using System;
using System.Collections.Generic;
using System.Linq;
using IronPython.Hosting;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string sauce = @"
import clr
clr.AddReferenceByPartialName('System.Threading.Tasks')
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
        public async  static Task<string> Readline_alt(string mess)
        {
            await Task.Delay(1000);
            Console.Write(mess + " >>>");
            return Console.ReadLine();
        }
    }

}