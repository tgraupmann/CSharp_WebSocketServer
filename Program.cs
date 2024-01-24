using System;
using System.Net;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace CSharp_WebSocketServer
{
    public class WebSocketClientHandler : WebSocketBehavior
    {
        private bool _mConnected = false;

        protected async override void OnOpen()
        {
            Console.WriteLine("{0} Client connected.", DateTime.Now);

            _mConnected = true;
            base.OnOpen();

            while (_mConnected)
            {
                try
                {
                    Send(string.Format(@"{{ ""message"": ""hello"", ""timestamp"": ""{0}"" }}", DateTime.Now));
                }
                catch
                {
                }
                await Task.Delay(1000 / 30);
            }
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Console.WriteLine("{0} Client disconnected.", DateTime.Now);

            _mConnected = false;

            base.OnClose(e);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine("Received: {0}", e.Data);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            const int port = 5001;
            var wssv = new WebSocketServer(IPAddress.Any, port);

            wssv.AddWebSocketService<WebSocketClientHandler>("/");
            wssv.Start();
            Console.WriteLine("WebSocketServer listening on ws://localhost:{0}", port);

            Console.ReadKey(true);
            wssv.Stop();
            Console.WriteLine("WebSocketServer shutdown.");
        }
    }
}
