using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
namespace NetworkHook
{
    internal class Program
    {
        [DllImport("netprofm.dll", SetLastError = true)]
        public static extern int NetworkIsAvailable();

        public static bool IsInternetAvailable()
        {
            int result = NetworkIsAvailable();
 
            return result == 0; 
        }

        static void Main()
        {
            bool isConnected = IsInternetAvailable();
            Console.WriteLine("Conexión a Internet: " + (isConnected ? "Disponible" : "No disponible"));
        }
    }
}
