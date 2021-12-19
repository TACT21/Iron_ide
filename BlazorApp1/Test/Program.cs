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
            Python_R();
        }

        static void Ex()
        {
            Console.WriteLine("calc");
            Match matche = Regex.Match("a = input('a')", "\\u0028.*?\\u0029");
            Console.WriteLine(matche);
        }

        static void Python_R()
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
            ThreadPool.GetMaxThreads(out int workerThreads, out int portThreads);
            Console.WriteLine("Worker threads={0}, Completion port threads={1}", workerThreads, portThreads);
            eng.Execute(sauce, scope);
            ThreadPool.GetMaxThreads(out workerThreads, out portThreads);
            Console.WriteLine("Worker threads={0}, Completion port threads={1}", workerThreads, portThreads);
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