using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.IO;
namespace AoELanServerLauncher
{
    public class RunHandlers
    {
        string Serverpaht = "aoe2DELanServer_1.2.2_win_x86-64\\server";

        string Serverexecutablepath = "aoe2DELanServer_1.2.2_win_x86-64\\server\\server.exe";

        string Launcherexecutablepath = "aoe2DELanServer_1.2.2_win_x86-64\\launcher\\launcher.exe";

        string Launcherpath = "aoe2DELanServer_1.2.2_win_x86-64\\launcher";

        string configLauncher = "aoe2DELanServer_1.2.2_win_x86-64\\launcher\\resources\\config.ini";


        public RunHandlers()
        {
        }

        public async Task<bool> changeIPToConnect(string ip)
        {
            try
            {
         
                await Task.Run(() =>
                {
                    Form1.config.Host = ip;
                    Form1.config.Start = "false";
                    Form1.config.SaveConfig();

                });

          
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cambiar la IP: {ex.Message}");
                return false;
            }

        }
        public async Task<bool> RunClient()
        {
            Process process = new Process();
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            process.StartInfo.FileName = currentDirectory+Launcherexecutablepath;
            process.StartInfo.UseShellExecute = false;
           
            process.StartInfo.WorkingDirectory = currentDirectory + "aoe2DELanServer_1.2.2_win_x86-64\\launcher";

            try
            {
                bool started = process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string errorOutput = process.StandardError.ReadToEnd();
         
                return started;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al iniciar el proceso: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RunServer()
        {

        
            if (IsProcessRunning(Serverexecutablepath))
            {
             
                StopProcess(Serverexecutablepath);
              
            }
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory+Serverpaht+"\\server.exe.pid"))
            {
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + Serverpaht + "\\server.exe.pid");
            }


            Process process = new Process();
       
            process.StartInfo.FileName =currentDirectory+Serverexecutablepath;
            process.StartInfo.WorkingDirectory = currentDirectory + "aoe2DELanServer_1.2.2_win_x86-64\\server";
            process.StartInfo.RedirectStandardOutput = false;
            process.StartInfo.UseShellExecute = false;

            try
            {
             

                await changeIPToConnect("127.0.0.1");
                bool started = false;
                
                 started = process.Start();




                Thread.Sleep(10000);
                bool clientStarted = await RunClient();
                return started && clientStarted;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al iniciar el proceso: {ex.Message}");
                return false;
            }
        }

        private bool IsProcessRunning(string executablePath)
        {
            string processName = System.IO.Path.GetFileNameWithoutExtension(executablePath);
            Process[] processes = Process.GetProcessesByName(processName);
            return processes.Length > 0;
        }

        private void StopProcess(string executablePath)
        {
            string processName = System.IO.Path.GetFileNameWithoutExtension(executablePath);
            Process[] processes = Process.GetProcessesByName(processName);

            foreach (var process in processes)
            {
                try
                {
                    process.Kill(); // Detener el proceso
                    process.WaitForExit(); // Esperar a que el proceso se detenga
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al detener el proceso {processName}: {ex.Message}");
                }
            }
        }
    }
}


