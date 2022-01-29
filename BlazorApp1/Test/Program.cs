using System;
using System.Collections.Generic;
using System.Linq;
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
import threading
import time
def loop():
    x = 2
    for i in range (10):
        x = x ** 2
        print(x)
        time.sleep(1)

t = threading.Thread(target=loop)
t.start()
for i in range (8):
    print('waiting')
    time.sleep(1)
t.join()
print('end')
";
            try
            {
                var eng = Python.CreateEngine();
                var scope = eng.CreateScope();
                eng.Execute(sauce, scope);
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

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