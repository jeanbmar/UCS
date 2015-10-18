using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using UCS.Network;
using UCS.Logic;

namespace UCS.PacketProcessing
{
    class Client
    {
        private Level m_vLevel;
        private long m_vSocketHandle;

        private byte[] m_vEnDecryptKey = {
		    0x2b, 0x00, 0x00, 0x00, 0xda, 0x00, 0x00, 0x00, 0x58, 0x24, 0x32, 0x6a, 0xc3, 0x06, 0x1b, 0x2c,
		    0x6b, 0x61, 0x3e, 0xaf, 0xec, 0x31, 0x60, 0x38, 0x05, 0x46, 0x04, 0x0c, 0x2b, 0xb9, 0x6e, 0x2e,
		    0x07, 0x5d, 0xff, 0xe8, 0x64, 0xe3, 0x70, 0x40, 0xdb, 0x93, 0xda, 0xcc, 0x01, 0x18, 0x22, 0x5e,
		    0xdf, 0x94, 0x5a, 0x4f, 0x25, 0xbe, 0x1d, 0xb5, 0x4b, 0xb4, 0x17, 0x8b, 0xf4, 0x57, 0x39, 0xb3,
		    0xfe, 0x91, 0x63, 0x7d, 0x1c, 0x9f, 0x7a, 0xca, 0x3a, 0x1e, 0x65, 0xa7, 0x68, 0xd1, 0x89, 0x78,
		    0x67, 0x43, 0xf3, 0xb2, 0x8d, 0xd9, 0x8e, 0x9e, 0x9c, 0xc9, 0xe9, 0x9b, 0xb6, 0xba, 0x75, 0x20,
		    0x74, 0xd3, 0x3f, 0x88, 0x56, 0x6d, 0x41, 0x62, 0x2a, 0xc6, 0xa2, 0xc5, 0x5f, 0x7b, 0x33, 0xc4,
		    0xf6, 0x3b, 0xef, 0x97, 0x95, 0x92, 0x87, 0xc7, 0xf7, 0x52, 0x7f, 0x10, 0xa8, 0xdc, 0x45, 0xd0,
		    0xfd, 0x99, 0x9d, 0xe0, 0x0e, 0x29, 0x6c, 0x81, 0x83, 0xd5, 0xe7, 0xee, 0xfa, 0x59, 0xa9, 0x27,
		    0x26, 0xcd, 0xdd, 0xae, 0x09, 0x44, 0x30, 0x47, 0xe5, 0xf0, 0x37, 0x5b, 0x13, 0x4a, 0x96, 0x4e,
		    0x72, 0xa6, 0x79, 0xd4, 0xa4, 0x98, 0x36, 0xe6, 0x86, 0xd7, 0x28, 0x02, 0xf9, 0x2d, 0xbc, 0x12,
		    0xed, 0xbf, 0xad, 0x48, 0xce, 0x84, 0x82, 0x7c, 0xfb, 0x53, 0xe2, 0xea, 0x0d, 0x14, 0x1f, 0x54,
		    0xaa, 0x08, 0x23, 0xf2, 0x21, 0x4c, 0x8c, 0x3d, 0xbd, 0xa1, 0x35, 0xb7, 0x55, 0x42, 0x4d, 0xa5,
		    0x8f, 0x3c, 0x85, 0xc8, 0x77, 0x03, 0x0a, 0x1a, 0x11, 0x15, 0x00, 0xc0, 0xb1, 0xeb, 0x5c, 0x71,
		    0xd8, 0xc2, 0x8a, 0x0f, 0x76, 0x50, 0xa0, 0x19, 0x73, 0xf5, 0xfc, 0x0b, 0x69, 0xe1, 0x90, 0xd2,
		    0xab, 0xc1, 0x9a, 0x51, 0x6f, 0x34, 0x80, 0xb8, 0xa3, 0xd6, 0xcb, 0xe4, 0xcf, 0x2f, 0xf8, 0xbb,
		    0x16, 0x66, 0xb0, 0x49, 0xac, 0xde, 0xf1, 0x7e
	    };

        private byte[] m_vPrivateKey = {
            0x66, 0x68, 0x73, 0x64, 0x36, 0x66, 0x38, 0x36, 0x66, 0x36, 0x37, 0x72, 0x74, 0x38, 0x66, 0x77,
            0x37, 0x38, 0x66, 0x77, 0x37, 0x38, 0x39, 0x77, 0x65, 0x37, 0x38, 0x72, 0x39, 0x37, 0x38, 0x39,
            0x77, 0x65, 0x72, 0x36, 0x72, 0x65
        };

        public Client(Socket so)
        {
            this.Socket = so;
            m_vSocketHandle = so.Handle.ToInt64();
            IncomingPacketsKey = new byte[m_vEnDecryptKey.Length];
            Array.Copy(m_vEnDecryptKey, IncomingPacketsKey, m_vEnDecryptKey.Length);
            OutgoingPacketsKey = new byte[m_vEnDecryptKey.Length];
            Array.Copy(m_vEnDecryptKey, OutgoingPacketsKey, m_vEnDecryptKey.Length);
            DataStream = new List<Byte>();
        }

        public Socket Socket
        {
            get;
            set;
        }

        public uint ClientSeed
        {
            get;
            set;
        }

        public byte[] IncomingPacketsKey
        {
            get;
            set;
        }

        public byte[] OutgoingPacketsKey
        {
            get;
            set;
        }

        public static void TransformSessionKey(int clientSeed, byte[] sessionKey)
        {
            int[] buffer = new int[624];
            initialize_generator(clientSeed, buffer);
            int byte100 = 0;
            for (int i = 0; i < 100; i++)
            {
                byte100 = extract_number(buffer, i);
            }

            for (int i = 0; i < sessionKey.Length; i++)
            {
                sessionKey[i] ^= (byte)(extract_number(buffer, i + 100) & byte100);
            }

        }

        // Initialize the generator from a seed
        public static void initialize_generator(int seed, int[] buffer)
        {
            buffer[0] = seed;
            for (int i = 1; i < 624; ++i)
            {
                buffer[i] = (int)(1812433253 * ((buffer[i - 1] ^ (buffer[i - 1] >> 30)) + 1));
            }
        }


        // Extract a tempered pseudorandom number based on the index-th value,
        // calling generate_numbers() every 624 numbers

        public static int extract_number(int[] buffer, int ix)
        {
            if (ix == 0)
            {
                generate_numbers(buffer);
            }

            int y = buffer[ix];
            y ^= (y >> 11);
            y ^= (int)(y << 7 & (2636928640)); // 0x9d2c5680
            y ^= (int)(y << 15 & (4022730752)); // 0xefc60000
            y ^= (y >> 18);

            if ((y & (1 << 31)) != 0)
            {
                y = ~y + 1;
            }

            ix = (ix + 1) % 624;
            return y % 256;
        }

        // Generate an array of 624 untempered numbers
        public static void generate_numbers(int[] buffer)
        {
            for (int i = 0; i < 624; i++)
            {
                int y = (int)((buffer[i] & 0x80000000) + (buffer[(i + 1) % 624] & 0x7fffffff));
                buffer[i] = (int)(buffer[(i + 397) % 624] ^ (y >> 1));
                if ((y % 2) != 0)
                {
                    buffer[i] = (int)(buffer[i] ^ (2567483615));
                }

            }

        }

        public unsafe void UpdateKey(byte[] sessionKey)
        {
            TransformSessionKey((int)ClientSeed, sessionKey);

            byte[] newKey = new byte[264];
            byte[] clientKey = sessionKey;
            int v7 = m_vPrivateKey.Length;
            //byte[] v8 = privateKey;
            int v9 = m_vPrivateKey.Length + sessionKey.Length;
            byte[] completeSessionKey = new byte[m_vPrivateKey.Length + sessionKey.Length];
            Array.Copy(m_vPrivateKey, 0, completeSessionKey, 0, v7); //memcpy(v10, v8, v7);
            Array.Copy(clientKey, 0, completeSessionKey, v7, sessionKey.Length); //memcpy(v10 + v7, clientKey, sessionKeySize);
            uint v11 = 0;
            uint v16;
            uint v12;//attention type
            byte v13;//attention type
            uint v14;
            byte* v15;
            uint v17;
            uint v18;
            byte v19;
            byte* v20;
            byte v21;
            uint v22;
            byte* v23;

            fixed (byte* v5 = newKey, v8 = m_vPrivateKey, v10 = completeSessionKey)
            {
                do
                {
                    *(byte*)(v5 + v11 + 8) = (byte)v11;
                    ++v11;
                }
                while (v11 != 256);
                *v5 = 0;
                *(v5 + 4) = 0;
                while (true)
                {
                    v16 = *v5;
                    //if (v16 == 255)//if ( *v5 > 255 )
                    //    break;
                    v12 = *((byte*)v10 + v16 % v9) + *(uint*)(v5 + 4);
                    *(uint*)v5 = v16 + 1;
                    v13 = *(byte*)(v5 + v16 + 8);
                    v14 = (byte)(v12 + *(byte*)(v5 + v16 + 8));
                    *(uint*)(v5 + 4) = v14;
                    v15 = v5 + v14;
                    *(byte*)(v5 + v16 + 8) = *(byte*)(v15 + 8);
                    *(byte*)(v15 + 8) = v13;
                    if (v16 == 255)//if ( *v5 > 255 )
                        break;
                }
                v17 = 0;
                *v5 = 0;
                *(v5 + 4) = 0;
                while (v17 < v9)
                {
                    ++v17;
                    v18 = *(uint*)(v5 + 4);
                    v19 = (byte)(*(uint*)v5 + 1);
                    *(uint*)v5 = v19;
                    v20 = v5 + v19;
                    v21 = *(byte*)(v20 + 8);
                    v22 = (byte)(v18 + v21);
                    *(uint*)(v5 + 4) = v22;
                    v23 = v5 + v22;
                    *(byte*)(v20 + 8) = *(byte*)(v23 + 8);
                    *(byte*)(v23 + 8) = v21;
                }
            }
            Array.Copy(newKey, IncomingPacketsKey, newKey.Length);
            Array.Copy(newKey, OutgoingPacketsKey, newKey.Length);

        }

        public void EnDecrypt(Byte[] key, Byte[] data)
        {
            int dataLen;

            if (data != null)
            {
                dataLen = data.Length;

                if (dataLen >= 1)
                {

                    do
                    {
                        dataLen--;
                        byte index = (byte)(key[0] + 1);
                        key[0] = index;
                        byte num2 = (byte)(key[4] + key[index + 8]);
                        key[4] = num2;
                        byte num3 = key[index + 8];
                        key[index + 8] = key[num2 + 8];
                        key[key[4] + 8] = num3;
                        byte num4 = key[((byte)(key[key[4] + 8] + key[key[0] + 8])) + 8];
                        data[(data.Length - dataLen) - 1] = (byte)(data[(data.Length - dataLen) - 1] ^ num4);
                    }
                    while (dataLen > 0);
                }
            }
        }

        public void Decrypt(Byte[] data)
        {
            EnDecrypt(this.IncomingPacketsKey, data);
        }

        public void Encrypt(Byte[] data)
        {
            EnDecrypt(this.OutgoingPacketsKey, data);
        }

        public List<Byte> DataStream
        {
            get;
            set;
        }

        public Level GetLevel()
        {
            return m_vLevel;
        }

        public long GetSocketHandle()
        {
            return m_vSocketHandle;
        }

        public void SetLevel(Level l)
        {
            m_vLevel = l;
        }

        public bool TryGetPacket(out Message p)
        {
            p = null;
            bool result = false;

            if(DataStream.Count() >= 5)
            {
                int length = ((0x00 << 24) | (DataStream[2] << 16) | (DataStream[3] << 8) | DataStream[4]);
                ushort type = (ushort)((DataStream[0] << 8) | DataStream[1]);

                if ((DataStream.Count - 7) >= length)
                {

                    object obj = null;
                    byte[] packet = DataStream.Take(7 + length).ToArray();

                    using (var br = new BinaryReader(new MemoryStream(packet)))
                    {
                        obj = MessageFactory.Read(this, br, type);
                    }

                    if (obj != null)
                    {
                        p = (Message)obj;
                        result = true;
                    }  
                    else
                    {
                        //Update Decryption Key
                        byte[] data = DataStream.Skip(7).Take(length).ToArray();
                        Decrypt(data);
                    }
                    DataStream.RemoveRange(0, 7 + length);
                }
            }
            return result;       
        }
    }
}
