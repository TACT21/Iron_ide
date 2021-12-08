﻿using System;
using System.Collections.Generic;
using System.Linq;
using IronPython.Hosting;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        public static string Readline_alt(string mess)
        {
            Console.Write(mess + " >>>");
            return Console.ReadLine();
        }
        public static Func<string, string> func = Readline_alt;
        dynamic dynamic_func = func;
        public static void Main(string[] args)
        {
            string sauce = @"
import clr
clr.AddReference('MyApp')
def greetings(name):
    print('test')
    a = Utility.Readline_alt('test>>')
    print(a)
    return 'Hello ' + name.title() + '!'
";
            var eng = Python.CreateEngine();
            var scope = eng.CreateScope();
            eng.Execute(sauce, scope);
            var u = new Utility();
            dynamic greetings = scope.GetVariable("greetings", u);
            var result = greetings("world");
            Console.WriteLine("end");
            Console.WriteLine(result);
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

    public class Log
    {
        public int testInt = 0;

        public void debugLog()
        {
            Debug.Log("Debug Log " + testInt);
        }
    }
}