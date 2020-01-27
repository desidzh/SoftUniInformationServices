using SIS.HTTP;
using System;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp
{

    //Meтоди, които получават информация за request-a и дават някакъв response
    //  Actions:
    // / => IndexPage(request)
    // /favicon.ico => favicon.ico
    // GET /Contact => response ShowContactFrom(request)
    // POST /Contact => response FillContactFrom(request)

    //new HttpServer(80, actions)
    // .Start()

    class Program
    {
        static async Task Main(string[] args)
        {
            var httpServer = new HttpsServer(1234);
            await httpServer.StartAsync();
        }

        public HttpResponse Index(HttpRequest request)
        {
            var content = "<h1>home page</h1>";
            byte[] stringContent = Encoding.UTF8.GetBytes(content);
            var response = new HttpResponse(HttpResponseCode.Ok, stringContent);
            return response;
        }

        public HttpResponse Login(HttpRequest request)
        {
            var content = "<h1>login page</h1>";
            byte[] stringContent = Encoding.UTF8.GetBytes(content);
            var response = new HttpResponse(HttpResponseCode.Ok, stringContent);
            return response;
        }

        public HttpResponse DoLogin(HttpRequest request)
        {
            var content = "<h1>login page</h1>";
            byte[] stringContent = Encoding.UTF8.GetBytes(content);
            var response = new HttpResponse(HttpResponseCode.Ok, stringContent);
            return response;
        }
    }
}
