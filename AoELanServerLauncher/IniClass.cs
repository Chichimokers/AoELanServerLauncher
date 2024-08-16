using System;
using System.IO;
using System.Text;
namespace AoELanServerLauncher
{
    public class IniClass
    {
        private const string ConfigFilePath = @"aoe2DELanServer_1.2.2_win_x86-64/launcher/resources/config.ini";
        public EventHandler ConfigLoaded;
        public bool CanAddHost { get; set; } = true;
        public string CanTrustCertificate { get; set; } = "local";
        public bool CanBroadcastBattleServer { get; set; } = false;
        public bool IsolateMetadata { get; set; } = true;
        public bool IsolateProfiles { get; set; } = true;

        public string Start { get; set; } = "false";
        public string Executable { get; set; } = "auto";
        public string ExecutableArgs { get; set; } = "";
        public string Host { get; set; } = "127.0.0.1";
        public string Stop { get; set; } = "auto";
        public int AnnouncePorts { get; set; } = 31978;
        public string ClientExecutable { get; set; } = "auto";
        public string ClientExecutableArgs { get; set; } = "";

        public IniClass()
        {
          
        }
        protected virtual void OnConfigLoaded()
        {
            ConfigLoaded?.Invoke(this, EventArgs.Empty);
        }
        public void  LoadConfig()
        {
          
                if (!File.Exists(ConfigFilePath))
                {
                    SaveConfig();
                }
                else
                {
                    var lines = File.ReadAllLines(ConfigFilePath);
                    string currentSection = string.Empty;

                    foreach (var line in lines)
                    {
                        // Ignorar líneas vacías
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        // Cambiar la sección actual si se encuentra un encabezado
                        if (line.StartsWith("["))
                        {
                            currentSection = line.Trim();
                            continue;
                        }

                        // Cargar valores según la sección actual
                        switch (currentSection)
                        {
                            case "[Config]":
                                if (line.StartsWith("CanAddHost"))
                                    CanAddHost = bool.Parse(GetValue(line));
                                else if (line.StartsWith("CanTrustCertificate"))
                                    CanTrustCertificate = GetValue(line);
                                else if (line.StartsWith("CanBroadcastBattleServer"))
                                    CanBroadcastBattleServer = bool.Parse(GetValue(line));
                                else if (line.StartsWith("IsolateMetadata"))
                                    IsolateMetadata = bool.Parse(GetValue(line));
                                else if (line.StartsWith("IsolateProfiles"))
                                    IsolateProfiles = bool.Parse(GetValue(line));
                                break;

                            case "[Server]":
                                if (line.StartsWith("Start"))
                                    Start = GetValue(line);
                                else if (line.StartsWith("Executable"))
                                    Executable = GetValue(line);
                                else if (line.StartsWith("ExecutableArgs"))
                                    ExecutableArgs = GetValue(line);
                                else if (line.StartsWith("Host"))
                                    Host = GetValue(line);
                                else if (line.StartsWith("Stop"))
                                    Stop = GetValue(line);
                                else if (line.StartsWith("AnnouncePorts"))
                                    AnnouncePorts = int.Parse(GetValue(line));
                                break;

                            case "[Client]":
                                if (line.Contains("ExecutableArgs"))
                                    ClientExecutableArgs = GetValue(line);
                               else if (line.Contains("Executable"))
                                ClientExecutable = GetValue(line);
                            
                                break;
                        }
                    }
                }
            OnConfigLoaded();
        }

        private string GetValue(string line)
        {
            return line.Split('=')[1].Trim();
        }

        public void SaveConfig()
        {
            string directoryPath = Path.GetDirectoryName(ConfigFilePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            var sb = new StringBuilder();

            sb.AppendLine("[Config]");
            sb.AppendLine($"CanAddHost = {CanAddHost.ToString().ToLower()}");
            sb.AppendLine($"CanTrustCertificate = {CanTrustCertificate}");
            sb.AppendLine($"CanBroadcastBattleServer = {CanBroadcastBattleServer.ToString().ToLower()}");
            sb.AppendLine($"IsolateMetadata = {IsolateMetadata.ToString().ToLower()}");
            sb.AppendLine($"IsolateProfiles = {IsolateProfiles.ToString().ToLower()}");

            sb.AppendLine("[Server]");
            sb.AppendLine($"Start = {Start.ToString().ToLower()}");
            sb.AppendLine($"Executable = {Executable}");
            sb.AppendLine($"ExecutableArgs = {ExecutableArgs}");
            sb.AppendLine($"Host = {Host}");
            sb.AppendLine($"Stop = {Stop.ToString().ToLower()}");
            sb.AppendLine($"AnnouncePorts = {AnnouncePorts}");

            sb.AppendLine("[Client]");
            sb.AppendLine($"Executable = {ClientExecutable}");
            sb.AppendLine($"ExecutableArgs = {ClientExecutableArgs}");

            // Guardar el archivo
            File.WriteAllText(ConfigFilePath, sb.ToString());
        }
    }
}