using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;


namespace InternetSimulation
{
   
    public class SimpleHttpServer
    {
        private HttpListener _listener;

        public SimpleHttpServer(int port)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://*:{port}/");
            _listener.Start();
        }

        public async Task RunAsync()
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    var context = await _listener.GetContextAsync();
                    HandleRequestAsync(context);
                }
            });
        }

        private async Task HandleRequestAsync(HttpListenerContext context)
        {
            string filePath = context.Request.Url.AbsolutePath.TrimStart('/');

            Console.WriteLine($"Solicitud recibida para: {filePath}"); // Agregar un log para depuración

            if (filePath == "ncsi.txt" || filePath == "connecttest.txt")
            {
                await ServeFileAsync(context, filePath);
            }
            else
            {
                context.Response.StatusCode = 404;
                context.Response.Close();
            }
        }
        private static async Task ServeFileAsync(HttpListenerContext context, string fileName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            if (File.Exists(filePath))
            {
                context.Response.ContentType = "text/plain";
                context.Response.StatusCode = 200;
                using (Stream fileStream = File.OpenRead(filePath))
                {
                    await fileStream.CopyToAsync(context.Response.OutputStream);
                }
            }
            else
            {
                context.Response.StatusCode = 404;
            }

            context.Response.Close();
        }
        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }
    }

}
