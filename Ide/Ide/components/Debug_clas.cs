using System;
using Microsoft.JSInterop;

namespace Ide.Components
{
    public static class Debug_class
    {
        private static string cmd = "";
        private static bool islocked = false;
        [JSInvokable]
        public static string GetCmd()
        {
            return cmd;
        }
        public static void SetCmd(string set)
        {
            if (islocked)
            {
                cmd = set;
                islocked = true;
            }
            else
            {
                throw new AccessViolationException();
            }
        }
        public static void CleCmd()
        {
            cmd = "";
            islocked = false;
        }

    }
}
