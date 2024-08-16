using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;
namespace AoELanServerLauncher
{
    public partial class Form1 : Form
    {
        public static IniClass config;
        public static RunHandlers handler;
        public Form1()
        {
            
            InitializeComponent();
        }
        void LoadIPS()
        {
            
            string ips = "";
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach(var a in networkInterfaces)
            {
              
                var adapterproperties = a.GetIPProperties();
                foreach(var ip in adapterproperties.UnicastAddresses)
                {
                    if(ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        if(a.OperationalStatus == OperationalStatus.Up)
                        {
                            ips += ip.Address.ToString() + " // ";
                        }
                      
                    }
                   
                }
            }
            label4.Text = ips;
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            LoadIPS();

            handler = new RunHandlers();

            config = new IniClass();

            config.ConfigLoaded += Config_ConfigLoaded;

            config.LoadConfig();
         
         
        }

        private void Config_ConfigLoaded(object sender, EventArgs e)
        {
            textBox2.Text = config.ClientExecutable;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await handler.RunServer();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
           await handler.changeIPToConnect(textBox1.Text);
           await  handler.RunClient();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Archivos ejecutables (*.exe)|*.exe|Todos los archivos (*.*)|*.*",
                Title = "Selecciona un archivo ejecutable"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
            
                string selectedFilePath = openFileDialog.FileName;
                config.ClientExecutable = '"'+selectedFilePath+'"';
                textBox2.Text = selectedFilePath;
                config.SaveConfig();
                config.LoadConfig();
            }        
        }
    }
}
