// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
var a = new Utility();
a.Test();
Console.WriteLine(DateTime.Now.ToString("HH:mm:ss(s)") + a.input);
public class Utility
{
    public string input = string.Empty;
    public void Test()
    {
        Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "　Called");
        Task<string> task = Task.Run(() =>
        {
            return Getinput();
        });
        string message = task.Result;
        input = message;
    }
    async Task<string> Getinput()
    {
        await Task.Delay(1000);
        Console.WriteLine("Get input");
        return "Something";
    }
}