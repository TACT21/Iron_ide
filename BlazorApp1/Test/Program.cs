using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IronPython.Hosting;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        [STAThread]
        public static async Task Main(string[] args)
        {
            ThreadPool.GetMaxThreads(out int workerThreads, out int portThreads);
            Console.WriteLine("Worker threads={0}, Completion port threads={1}", workerThreads, portThreads);
            var a = ThreadPool.SetMaxThreads(1, 1);
            Console.WriteLine(a);
            ThreadPool.GetMaxThreads(out workerThreads, out portThreads);
            Console.WriteLine("Worker threads={0}, Completion port threads={1}", workerThreads, portThreads);
            Stopwatch sw = new Stopwatch();
            //Python_R();
            Console.ReadLine();
            sw.Start();
            Sleepsync();
            sw.Stop();
            Console.WriteLine(sw.Elapsed.ToString()); 
            for(int i = 0; i < 10; i++)
            {
                sw.Reset();
                sw.Start();
                await VAsync(i);
                sw.Stop();
                Console.WriteLine(sw.Elapsed.ToString());
            }
            Console.ReadLine();
            Python_R();
            Console.ReadLine();
        }

        static int Sleepsync()
        {
            Thread.Sleep(1000);
            return 1;
        }

        static async Task<int> VAsync(int a = 0)
        {
            await Task.Delay(1000);
            Console.WriteLine(a);
            return 1;
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
print(Test.Read())
";
            try
            {
                var eng = Python.CreateEngine();
                var scope = eng.CreateScope();
                var t = new Temp();
                scope.SetVariable("Test", t);
                eng.Execute(sauce, scope);
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Type y = typeof(Task<string>);
            MemberInfo[] members = y.GetMembers(
            BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.Instance | BindingFlags.Static |
            BindingFlags.DeclaredOnly);
            foreach (MemberInfo m in members)
            {
                //メンバの型と、名前を表示する
                Console.WriteLine("{0} - {1}", m.MemberType, m.Name);
            }
            Console.WriteLine();
            Console.WriteLine("end");
        }
    }
    public class Temp
    {
        public async Task<string> Read()
        {
            Console.WriteLine("console");
            await Task.Delay(1000);
            return "End?";
        }
    }

}