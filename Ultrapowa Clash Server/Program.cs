using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Configuration;
using UCS.Network;
using UCS.PacketProcessing;
using UCS.Core;

namespace UCS
{
    class Program
    {
        static void Main(string[] args)
        {
        Console.WriteLine(
@"
888     888  .d8888b.   .d8888b.  
888     888 d88P  Y88b d88P  Y88b 
888     888 888    888 Y88b.      
888     888 888         ""Y888b.   
888     888 888            ""Y88b. 
888     888 888    888       ""888 
Y88b. .d88P Y88b  d88P Y88b  d88P 
 ""Y88888P""   ""Y8888P""   ""Y8888P""  
        ");
            Console.WriteLine("Ultrapowa Clash Server");
            Console.WriteLine("version 0.6.1.0");
            Console.WriteLine("www.ultrapowa.com");
            Console.WriteLine("");
            Console.WriteLine("Server starting...");
            Gateway g = new Gateway();
            PacketManager ph = new PacketManager();
            MessageManager dp = new MessageManager();
            ResourcesManager rm = new ResourcesManager();
            ObjectManager pm = new ObjectManager();
            dp.Start();
            ph.Start();
            g.Start();
            ApiManager api = new ApiManager();
            Debugger.SetLogLevel(Int32.Parse(ConfigurationManager.AppSettings["loggingLevel"]));
            Logger.SetLogLevel(Int32.Parse(ConfigurationManager.AppSettings["loggingLevel"]));
            Console.WriteLine("Server started, let's play Clash of Clans!");
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
