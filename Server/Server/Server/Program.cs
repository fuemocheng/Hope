using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using NetFrame.Coding;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleServer server = new SimpleServer(1000);
            server.handlerCenter = new HandlerCenter();
            server.messageEncode = MessageEncoding.Encode;
            server.messageDecode = MessageEncoding.Decode;
            server.lengthEncode = LengthEncoding.Encode;
            server.lengthDecode = LengthEncoding.Decode;
            server.Start(8001);

            Console.WriteLine("Server starts success!");
            Console.WriteLine("Start listening....");

            while (true) { }
        }
    }
}
