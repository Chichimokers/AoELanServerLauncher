using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using EasyHook;

namespace HookWininet
{
    public class Program : MarshalByRefObject, IEntryPoint
    {
        // Declarar el delegado para la función original
        public delegate bool InternetGetConnectedStateDelegate(out int lpdwFlags, int dwReserved);


        // Constructor
        public Program(RemoteHooking.IContext context)
        {
            // Aquí puedes inicializar cualquier cosa necesaria
        }

        // Método para iniciaer el hooking
        public void Run(RemoteHooking.IContext context)
        {
            // Instalar el hook
            LocalHook hook = LocalHook.Create(
                LocalHook.GetProcAddress("wininet.dll", "InternetGetConnectedState"),
                new InternetGetConnectedStateDelegate(HookedInternetGetConnectedState),
                this);

            // Hacer que el hook sea efectivo en todos los hilos
            hook.ThreadACL.SetInclusiveACL(new int[1]);

            // Mantener el hook activo
            RemoteHooking.WakeUpProcess();
        }

        // Función que reemplaza a la original
        public bool HookedInternetGetConnectedState(out int lpdwFlags, int dwReserved)
        {
            // Forzar que siempre devuelva conectado
            lpdwFlags = 0; // Estado de conexión
            return true ; // Indica que hay conexión
        }

        // Método principal
        public static void Main(string[] args)
        {

            //// Verificar si se proporcionó un argumento para la PID
            //if (args.Length != 1)
            //{
            //    Console.WriteLine("Uso: Program.exe <PID>");
            //    return; // Salir si no se proporciona una PID válida
            //}

            // Intentar convertir el primer argumento a un entero
            //int pid = 0;
            //bool success = Int32.TryParse(args[0], out pid);

            //if (!success || pid <= 0)
            //{
            //    Console.WriteLine("La PID debe ser un número válido mayor a cero.");
            //    return; // Salir si la conversión falla o la PID es inválida
            //}

            // Instalar el hook usando la PID proporcionada
            int pid = Convert.ToInt32(Console.ReadLine());
                RemoteHooking.Inject(
                    pid,
                    typeof(Program).Assembly.Location,
                    typeof(Program).Assembly.Location,
                    null);

        }
    

    }
}
