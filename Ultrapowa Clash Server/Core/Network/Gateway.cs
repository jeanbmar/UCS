using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using UCS.PacketProcessing;
using UCS.Core;

namespace UCS.Network
{
    class Gateway
    {
        const int 
            kPort = 9339,
            kHostConnectionBacklog = 30;
        private static Socket m_vServerSocket;
        IPAddress ip;

        public void Start()
        {
            if (Host(kPort))
            {
                Console.WriteLine("Gateway started on port " + kPort);
            }         
        }

        void Disconnect()
        {
            if (m_vServerSocket != null)
            {
                m_vServerSocket.BeginDisconnect(false, new System.AsyncCallback(OnEndHostComplete), m_vServerSocket);
            }
        }

        public static Socket Socket
        {
            get
            {
                return m_vServerSocket;
            }
        }

        void OnClientConnect(System.IAsyncResult result)
        {
            try
            {
                Socket clientSocket = m_vServerSocket.EndAccept(result);
                Console.WriteLine("Client connected (" + ((IPEndPoint)clientSocket.RemoteEndPoint).Address.ToString() + ":" + ((IPEndPoint)clientSocket.RemoteEndPoint).Port.ToString() + ")");
                ResourcesManager.AddClient(new Client(clientSocket));
                SocketRead.Begin(clientSocket, OnReceive, OnReceiveError);
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Exception when accepting incoming connection: " + e);
            }
            try
            {
                m_vServerSocket.BeginAccept(new System.AsyncCallback(OnClientConnect), m_vServerSocket);
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Exception when starting new accept process: " + e);
            }
        }

        void OnReceive(SocketRead read, byte[] data)
        {
            
            try
            {
                long socketHandle = read.Socket.Handle.ToInt64();
                Client c = ResourcesManager.GetClient(socketHandle);
                    //Ajoute les données au stream client
                c.DataStream.AddRange(data);

                Message p;
                while (c.TryGetPacket(out p))
                {
                    PacketManager.ProcessIncomingPacket(p);
                }
            }
            catch(Exception)
            {
                //Client may not exist anymore
            }
        }

        void OnReceiveError(SocketRead read, System.Exception exception)
        {
            //Console.WriteLine("Error received: " + exception);
        }

        void OnEndHostComplete(System.IAsyncResult result)
        {
            m_vServerSocket = null;
        }

        public IPAddress IP
        {
            get
            {
                if (ip == null)
                {
                    ip = (
                        from entry in Dns.GetHostEntry(Dns.GetHostName()).AddressList
                        where entry.AddressFamily == AddressFamily.InterNetwork
                        select entry
                    ).FirstOrDefault();
                }

                return ip;
            }
        }

        public bool Host(int port)
        {
            //Console.WriteLine("Hosting on port " + port);

            m_vServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                m_vServerSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                m_vServerSocket.Listen(kHostConnectionBacklog);
                m_vServerSocket.BeginAccept(new System.AsyncCallback(OnClientConnect), m_vServerSocket);
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Exception when attempting to host (" + port + "): " + e);

                m_vServerSocket = null;

                return false;
            }

            return true;
        }
    }
}