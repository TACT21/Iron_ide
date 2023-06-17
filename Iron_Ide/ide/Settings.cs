using System.Text;
namespace ide
{
    public static class Settings
    {
        public static Encoding defaultEncoding = Encoding.UTF8;
        public static bool takingRisk  = false;
        public static string FunctionBridgeName { get; } = "ActionBridge";
    }
}
