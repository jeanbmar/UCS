using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Concurrent;
//using System.Data.SQLite;
//using System.Data.SQLite.Linq;
using System.Data.Linq;
using System.Data.Entity;
using MySql.Data;
using System.IO;
using System.Configuration;
using Newtonsoft.Json;
using UCS.PacketProcessing;
using UCS.Logic;
using UCS.GameFiles;

namespace UCS.Core

{
    class ObjectManager
    {
        public System.Threading.Timer TimerReference;
        public bool m_vTimerCanceled;
        private static readonly object m_vDatabaseLock = new object();
        private static long m_vAvatarSeed;
        private static long m_vAllianceSeed;
        private static DatabaseManager m_vDatabase;
        private static string m_vHomeDefault;
        private static Random m_vRandomSeed;
        private static Dictionary<long, Alliance> m_vAlliances;
        //private static ConcurrentDictionary<long, Level> m_vInMemoryPlayers { get; set; }

        public ObjectManager()
        {
            m_vTimerCanceled = false;
            m_vDatabase = new DatabaseManager();
            NpcLevels = new Dictionary<int, string>();
            DataTables = new DataTables();
            m_vAlliances = new Dictionary<long, Alliance>();

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["useCustomPatch"]))
            {
                LoadFingerPrint();
            }

            using (StreamReader sr = new StreamReader(@"gamefiles/default/home.json"))
            {
                m_vHomeDefault = sr.ReadToEnd();
            }

            m_vAvatarSeed = m_vDatabase.GetMaxPlayerId() + 1;
            m_vAllianceSeed = m_vDatabase.GetMaxAllianceId() + 1;
            LoadGameFiles();
            LoadNpcLevels();

            System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(Save);
            System.Threading.Timer TimerItem = new System.Threading.Timer(TimerDelegate, null, 60000, 60000);
            TimerReference = TimerItem;

            Console.WriteLine("Database Sync started");
            m_vRandomSeed = new Random();
        }

        public static Alliance CreateAlliance(long seed)
        {
            Alliance alliance;
            lock (m_vDatabaseLock)
            {
                if (seed == 0)
                    seed = m_vAllianceSeed;
                alliance = new Alliance(seed);
                m_vAllianceSeed++;
            }
            m_vDatabase.CreateAlliance(alliance);
            m_vAlliances.Add(alliance.GetAllianceId(), alliance);
            return alliance;
        }

        public static Level CreateAvatar(long seed)
        {
            Level pl;
            lock (m_vDatabaseLock)
            {
                if (seed == 0)
                    seed = m_vAvatarSeed;
                pl = new Level(seed);
                m_vAvatarSeed++;
            }
            pl.LoadFromJSON(m_vHomeDefault);
            m_vDatabase.CreateAccount(pl);
            return pl;
        }

        public static Alliance GetAlliance(long allianceId)
        {
            Alliance alliance = null;
            if (m_vAlliances.ContainsKey(allianceId))
            {
                alliance = m_vAlliances[allianceId];
            }
            else
            {
                alliance = m_vDatabase.GetAlliance(allianceId);
                if (alliance != null)
                {
                    m_vAlliances.Add(alliance.GetAllianceId(), alliance);
                }
            }
            return alliance;
        }

        public static List<Alliance> GetInMemoryAlliances()
        {
            List<Alliance> alliances = new List<Alliance>();
            alliances.AddRange(m_vAlliances.Values);
            return alliances;
        }

        public static Level GetRandomPlayer()
        {
            int index = m_vRandomSeed.Next(0, ResourcesManager.GetInMemoryLevels().Count);//accès concurrent KO
            return ResourcesManager.GetInMemoryLevels().ElementAt(index);
        }

        public static void LoadFingerPrint()
        {
            FingerPrint = new FingerPrint(@"gamefiles/fingerprint.json");    
        }

        public static void LoadNpcLevels()
        {
            Console.Write("Loading Npc levels... ");
            for(int i=0;i<50;i++)
            {
                using(StreamReader sr = new StreamReader(@"gamefiles/pve/level" + ( i + 1) + ".json"))
                {
                    NpcLevels.Add(i, sr.ReadToEnd());
                }
            }
            Console.WriteLine("done");
            
        }

        public static void LoadGameFiles()
        {
            List<Tuple<string, string, int>> gameFiles = new List<Tuple<string, string, int>>();
            gameFiles.Add(new Tuple<string, string, int>("Achievements", @"gamefiles/logic/achievements.csv", 22));
            gameFiles.Add(new Tuple<string, string, int>("Buildings", @"gamefiles/logic/buildings.csv", 0));
            gameFiles.Add(new Tuple<string, string, int>("Characters", @"gamefiles/logic/characters.csv", 3));
            gameFiles.Add(new Tuple<string, string, int>("Decos", @"gamefiles/logic/decos.csv", 17));
            gameFiles.Add(new Tuple<string, string, int>("Experience Levels", @"gamefiles/logic/experience_levels.csv", 10));
            gameFiles.Add(new Tuple<string, string, int>("Globals", @"gamefiles/logic/globals.csv", 13));
            gameFiles.Add(new Tuple<string, string, int>("Heroes", @"gamefiles/logic/heroes.csv", 27));
            gameFiles.Add(new Tuple<string, string, int>("Leagues", @"gamefiles/logic/leagues.csv", 12));
            gameFiles.Add(new Tuple<string, string, int>("NPCs", @"gamefiles/logic/npcs.csv", 16));
            gameFiles.Add(new Tuple<string, string, int>("Obstacles", @"gamefiles/logic/obstacles.csv", 7));
            gameFiles.Add(new Tuple<string, string, int>("Shields", @"gamefiles/logic/shields.csv", 19));
            gameFiles.Add(new Tuple<string, string, int>("Spells", @"gamefiles/logic/spells.csv", 25));
            gameFiles.Add(new Tuple<string, string, int>("Townhall Levels", @"gamefiles/logic/townhall_levels.csv", 14));
            gameFiles.Add(new Tuple<string, string, int>("Traps", @"gamefiles/logic/traps.csv", 11));
            gameFiles.Add(new Tuple<string, string, int>("Resources", @"gamefiles/logic/resources.csv", 2));

            Console.WriteLine("Loading server data...");
            foreach(var data in gameFiles)
            {
                Console.Write("\t" + data.Item1);
                DataTables.InitDataTable(new CSVTable(data.Item2),data.Item3);
                Console.WriteLine(" done");
            }
        }

        private void Save(object state)
        {
            m_vDatabase.Save(ResourcesManager.GetInMemoryLevels());
            m_vDatabase.Save(m_vAlliances.Values.ToList());
            if (m_vTimerCanceled)
            {
                TimerReference.Dispose();
            }
        }

        //Todo Cleanup
        //Remove disc clients
        //Remove InMemoryPlayers after a certain time

        //public static ConcurrentDictionary<Client, Level> OnlinePlayers { get; set; }
        //public static ConcurrentDictionary<Level, Client> OnlineClients { get; set; }
        //public static ConcurrentDictionary<Socket, Client> Clients { get; set; }
        public static DataTables DataTables { get; set; }
        public static FingerPrint FingerPrint { get; set; }
        public static Dictionary<int,string> NpcLevels {get;set;}
        
    }
}
