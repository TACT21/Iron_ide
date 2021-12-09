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
def greetings(name):
    print('test')
    a = scope.Readline_alt('test')
    print(a)
    return 'Hello ' + name.title() + '!'
";
            var eng = Python.CreateEngine();
            var scope = eng.CreateScope();
            var u = new Utility();
            Func<string, string> func = Readline_alt;
            scope.SetVariable("scope", u);
            eng.Execute(sauce, scope);//run
            dynamic greetings = scope.GetVariable("greetings");
            var result = greetings("world");
            Console.WriteLine("end");
            Console.WriteLine(result);
        }

        public static string Readline_alt(string mess)
        {
            Console.Write(mess + " >>>");
            return Console.ReadLine();
        }
    }

    public class Utility
    {
        public static string Readline_alt(string mess)
        {
            Console.Write(mess + " >>>");
            return Console.ReadLine();
        }
    }

}