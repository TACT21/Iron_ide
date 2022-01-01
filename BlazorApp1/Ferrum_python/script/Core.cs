namespace Ferrum_python.script
{
    public partial class Core
    {
        public string mess;
        public void Inst(string s)
        {
            mess = s;
        }
        public void Exec()
        {
            System.Threading.Thread.Sleep(10000);           //時間のかかる処理
            mess += "end<br>";
        }
        public string Get()
        {
            return mess;
        }
    }
}
