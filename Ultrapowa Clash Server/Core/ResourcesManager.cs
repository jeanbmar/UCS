using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Concurrent;
using UCS.PacketProcessing;
using UCS.Logic;
using UCS.Helpers;

namespace UCS.Core
{
    class ResourcesManager
    {
        private static DatabaseManager m_vDatabase;
        private System.Threading.Timer TimerReference;
        private bool m_vTimerCanceled;
        private static ConcurrentDictionary<long, Client> m_vClients;
        private static List<Level> m_vOnlinePlayers;
        private static ConcurrentDictionary<long, Level> m_vInMemoryLevels;
        private static object m_vOnlinePlayersLock = new object();

        public ResourcesManager()
        {
            m_vDatabase = new DatabaseManager();
            m_vClients = new ConcurrentDictionary<long,Client>();
            m_vOnlinePlayers = new List<Level>();
            m_vInMemoryLevels = new ConcurrentDictionary<long,Level>();
            m_vTimerCanceled = false;
            System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(ReleaseOrphans);
            System.Threading.Timer TimerItem = new System.Threading.Timer(TimerDelegate, null, 60000, 60000);
            TimerReference = TimerItem;
        }

        public static void AddClient(Client c)
        {
            long socketHandle = c.Socket.Handle.ToInt64();
            if (!m_vClients.ContainsKey(socketHandle))
                m_vClients.TryAdd(socketHandle, c);
        }

        public static Client GetClient(long socketHandle)
        {
            return m_vClients[socketHandle];
        }

        public static List<Client> GetConnectedClients()
        {
            List<Client> clients = new List<Client>();
            clients.AddRange(m_vClients.Values);
            return clients;
        }

        public static List<Level> GetInMemoryLevels()
        {
            List<Level> levels = new List<Level>();
            lock (m_vOnlinePlayersLock)
            {
                levels.AddRange(m_vInMemoryLevels.Values);
            }
            return levels;
        }

        private static Level GetInMemoryPlayer(long id)
        {
            Level result = null;
            lock(m_vOnlinePlayersLock)
            {
                if (m_vInMemoryLevels.ContainsKey(id))
                    result = m_vInMemoryLevels[id];
            }
            return result;
        }

        public static List<Level> GetOnlinePlayers()
        {
            List<Level> onlinePlayers = new List<Level>();
            lock (m_vOnlinePlayersLock)
            {
                onlinePlayers = m_vOnlinePlayers.ToList();
            }
            return onlinePlayers;
        }

        public static Level GetPlayer(long id, bool persistent = false)
        {
            Level result = GetInMemoryPlayer(id);
            if(result == null)
            {
                result = m_vDatabase.GetAccount(id);
                if(persistent)
                    LoadLevel(result);
            }
            return result;
        }

        public static bool IsClientConnected(long socketHandle)
        {
            return m_vClients.ContainsKey(socketHandle);
        }

        public static bool IsPlayerOnline(Level l)
        {
            return m_vOnlinePlayers.Contains(l);
        }

        public static void DropClient(long socketHandle)
        {
            Client c;
            m_vClients.TryRemove(socketHandle, out c);
            if(c.GetLevel() != null)
                LogPlayerOut(c.GetLevel());
        }

        public static void LoadLevel(Level level)
        {
            var id = level.GetPlayerAvatar().GetId();
            if(!m_vInMemoryLevels.ContainsKey(id))
                m_vInMemoryLevels.TryAdd(id, level);
        }

        public static void LogPlayerIn(Level level, Client client)
        {
            level.SetClient(client);
            client.SetLevel(level);
            lock (m_vOnlinePlayersLock)
            {
                if (!m_vOnlinePlayers.Contains(level))
                {
                    m_vOnlinePlayers.Add(level);
                    LoadLevel(level);
                }
            }
        }

        public static void LogPlayerOut(Level level)
        {
            lock (m_vOnlinePlayersLock)
            {
                m_vOnlinePlayers.Remove(level);
            }
            m_vInMemoryLevels.TryRemove(level.GetPlayerAvatar().GetId());
        }

        private void ReleaseOrphans(object state)
        {
            if (m_vTimerCanceled)
            {
                TimerReference.Dispose();
            }
        }
    }
}