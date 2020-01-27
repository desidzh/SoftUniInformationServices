using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SIS.HTTP
{


    public class HttpsServer : IHttpServer
    {
        private readonly TcpListener tcpListener;

        //TODO: actions
        public HttpsServer(int port)
        {
            this.tcpListener = new TcpListener(IPAddress.Loopback,port);


        }

        public async Task ResetAsync()
        {
            this.Stop();
            await this.StartAsync();
        }

        public async Task StartAsync()
        {
            this.tcpListener.Start();

            while (true)
            {
                TcpClient tcpClient = await this.tcpListener.AcceptTcpClientAsync();
                Task.Run(() => ProcessClientAsync(tcpClient));
            }

        }

        private async Task ProcessClientAsync(TcpClient tcpClient)
        {
            
                using NetworkStream networkStream = tcpClient.GetStream();
            try
            {
                byte[] requestBytes = new byte[1000000]; // TODO: Use buffer
                int bytesRead = await networkStream.ReadAsync(requestBytes, 0, requestBytes.Length);
                string requestAsString = Encoding.UTF8.GetString(requestBytes, 0, bytesRead);
                var request = new HttpRequest(requestAsString);

                string content = "<h1>random page</h1>";
                if (request.Path == "/")
                {
                    content = "<h1>home page</h1>";
                }
                else if (request.Path == "/users/login")
                {
                    content = "<h1>login page</h1>";
                }

                //byte[] fileContent = Encoding.UTF8.GetBytes("<form method='post'><input name='username' /><input type='submit'/></form><h1>Hello, World</h1>");
                byte[] fileContent = Encoding.UTF8.GetBytes(content);
                var response = new HttpResponse(HttpResponseCode.Ok, fileContent);
                response.Headers.Add(new Header("Server", "SoftUniServer/1.0"));
                response.Headers.Add(new Header("Content-Type", "text/html"));
                response.Cookies.Add(new ResponseCookie("sid", Guid.NewGuid().ToString())
                { HttpOnly = true, MaxAge = 3600 });

                byte[] responseBytes = Encoding.UTF8.GetBytes(response.ToString());
                await networkStream.WriteAsync(responseBytes, 0, responseBytes.Length);
                await networkStream.WriteAsync(response.Body, 0, response.Body.Length);

                Console.WriteLine(requestAsString);
                Console.WriteLine(new string('=', 60));
            }
            catch (Exception ex)
            {
                var errorResponse = new HttpResponse(HttpResponseCode.InternalServerError, Encoding.UTF8.GetBytes(ex.Message));
                errorResponse.Headers.Add(new Header ("Content-Type", "text/plaint" ));

                byte[] responseBytes = Encoding.UTF8.GetBytes(errorResponse.ToString());
                await networkStream.WriteAsync(responseBytes, 0, responseBytes.Length);
                await networkStream.WriteAsync(errorResponse.Body, 0, errorResponse.Body.Length);
            }
            
        }

        public void Stop()
        {
            this.tcpListener.Stop();
        }
    }
}
