using System;
using System.Collections.Generic;
using System.Linq;
using IronPython.Hosting;
using System.Runtime.InteropServices;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string sauce = @"
import clr
clr.AddReference('MyApp')
def greetings(name):
    print('test')
    a = scope.Run_func('test')
    print(a)
    return 'Hello ' + name.title() + '!'
";
            var u = new Utility();
            var eng = Python.CreateEngine();
            var scope = eng.CreateScope();
            scope.SetVariable("scope", u);
            eng.Execute(sauce, scope);
            dynamic greetings = scope.GetVariable("greetings");
            var result = greetings("world");
            Console.WriteLine("end");
            Console.WriteLine(result);
        }

    }

    public class Utility
    {
        public static string? Readline_alt(string mess)
        {
            Console.Write(mess + " >>>");
            return Console.ReadLine();
        }
    }

}