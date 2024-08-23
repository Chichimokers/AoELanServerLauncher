using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
namespace TestWininet
{
    internal class Program
    {
        // Importar la función InternetGetConnectedState de la biblioteca wininet.dll
        [DllImport("wininet.dll")]
        public static extern bool InternetGetConnectedState(out int lpdwFlags, int dwReserved);

        static void Main()
        {
            //Process currentProcess = Process.GetCurrentProcess();

            //int pid = currentProcess.Id;

            //string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            //string injectorPath = currentDirectory + "HookWininet.exe";


            //ProcessStartInfo startInfo = new ProcessStartInfo
            //{
            //    FileName = injectorPath,
            //    Arguments = pid.ToString(), // Pasar el ID del proceso como argumento
            //    UseShellExecute = false, // Opcional: usar el shell del sistema

            //};

            //try
            //{
            //    // Lanzar el proceso
            //    Process injectorProcess = Process.Start(startInfo);
            //    injectorProcess.WaitForExit(); // Esperar a que el proceso termine si es necesario
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Error al lanzar el inyector: " + ex.Message);
            //}

            // Variable para almacenar el estado de la conexión
            while (true)
            {
                Process currentProcess = Process.GetCurrentProcess();

                int pid = currentProcess.Id;
        
                Thread.Sleep(3000);
                Console.WriteLine(pid);
                int flags;

                // Llamar a la función para verificar el estado de la conexión
                bool isConnected = InternetGetConnectedState(out flags, 0);

                // Mostrar el resultado
                if (isConnected)
                {
                    Console.WriteLine("Hay conexión a Internet.");
                    // Puedes verificar los flags para más detalles
                    Console.WriteLine($"Flags: {flags}");
                }
                else
                {
                    Console.WriteLine("No hay conexión a Internet.");
                }

            }
        }
    }
}
