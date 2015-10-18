using System.Collections;
//using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System;


namespace UCS.Network
{
    public class SocketRead
    {
        public delegate void IncomingReadHandler(SocketRead read, byte[] data);
        public delegate void IncomingReadErrorHandler(SocketRead read, Exception exception);
        public const int kBufferSize = 256;

        Socket socket;
        IncomingReadHandler readHandler;
        IncomingReadErrorHandler errorHandler;
        byte[] buffer = new byte[kBufferSize];


        public Socket Socket
        {
            get
            {
                return socket;
            }
        }


        SocketRead(Socket socket, IncomingReadHandler readHandler, IncomingReadErrorHandler errorHandler = null)
        {
            this.socket = socket;
            this.readHandler = readHandler;
            this.errorHandler = errorHandler;

            BeginReceive();
        }


        void BeginReceive()
        {
            socket.BeginReceive(buffer, 0, kBufferSize, SocketFlags.None, new AsyncCallback(OnReceive), this);
        }


        public static SocketRead Begin(Socket socket, IncomingReadHandler readHandler, IncomingReadErrorHandler errorHandler = null)
        {
            return new SocketRead(socket, readHandler, errorHandler);
        }


        void OnReceive(IAsyncResult result)
        {
            try
            {
                if (result.IsCompleted)
                {
                    int bytesRead = socket.EndReceive(result);

                    if (bytesRead > 0)
                    {
                        byte[] read = new byte[bytesRead];
                        Array.Copy(buffer, 0, read, 0, bytesRead);

                        readHandler(this, read);
                        Begin(socket, readHandler, errorHandler);
                    }
                    else
                    {
                        // Disconnect
                    }
                }
            }
            catch (Exception e)
            {
                if (errorHandler != null)
                {
                    errorHandler(this, e);
                }
            }
        }
    }
}