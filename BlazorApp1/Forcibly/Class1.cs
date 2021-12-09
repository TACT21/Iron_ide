namespace Forcibly
{
    [Serializable]
    public class Class1
    {
        public Func<string, string> func;
        public string Run_func()
        {
            var a =  func("");
            return a.ToString();
        }
    }
}