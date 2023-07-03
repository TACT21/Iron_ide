namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            dynamic a = "";
            Console.WriteLine($"{a},{a.GetType().FullName}");
            a = 1;
            Console.WriteLine($"{a},{a.GetType().FullName}");
        }
    }
}