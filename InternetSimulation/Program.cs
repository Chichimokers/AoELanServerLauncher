using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace InternetSimulation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string hostsPath = @"C:\Windows\System32\drivers\etc\hosts";
            string dnsEntry = "127.0.0.1 www.msftncsi.com\n127.0.0.1 www.msftconnecttest.com\n";

            try
            {
                // Agregar las entradas DNS al archivo hosts
                File.AppendAllText(hostsPath, dnsEntry);
                Console.WriteLine("Entradas DNS añadidas al archivo hosts.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al modificar el archivo hosts: {ex.Message}");
                return;
            }
         
            
                int port = 80;
                SimpleHttpServer server = new SimpleHttpServer(port);
                Console.WriteLine($"Server running on http://localhost:{port}");
                await server.RunAsync();
            
        }
    }
}





