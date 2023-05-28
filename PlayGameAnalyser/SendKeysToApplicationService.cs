using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PlayGameAnalyser
{
    public class SendKeysToApplicationService
    {
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public void SendKeysToApplication(string AppName, string textToSend)
        {
            IntPtr calcWindow = FindWindow(null, AppName);

            if (SetForegroundWindow(calcWindow))
                SendKeys.Send(textToSend);
        }
    }
}
