using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Entity;
using System.Collections.Concurrent;
using UCS.Database;
using UCS.Logic;
using System.Configuration;
using MySql.Data;
using Newtonsoft.Json;

namespace UCS.Core
{
    class DatabaseManager
    {
        private string m_vConnectionString;

        public DatabaseManager()
        {
            m_vConnectionString = ConfigurationManager.AppSettings["databaseConnectionName"];
        }

        public void CreateAccount(Level l)
        {
            try
            {
                Debugger.WriteLine("Saving new account to database (player id: " + l.GetPlayerAvatar().GetId() + ")");
                using (var db = new Database.ucsdbEntities(m_vConnectionString))
                {
                    db.player.Add(
                        new Database.player
                        {
                            PlayerId = l.GetPlayerAvatar().GetId(),
                            AccountStatus = l.GetAccountStatus(),
                            AccountPrivileges = l.GetAccountPrivileges(),
                            LastUpdateTime = l.GetTime(),
                            Avatar = l.GetPlayerAvatar().SaveToJSON(),
                            GameObjects = l.SaveToJSON()
                        }
                    );
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Debugger.WriteLine("An exception occured during CreateAccount processing:", ex);
            }
        }

        public void CreateAlliance(Alliance a)
        {
            try
            {
                Debugger.WriteLine("Saving new Alliance to database (alliance id: " + a.GetAllianceId() + ")");
                using (var db = new Database.ucsdbEntities(m_vConnectionString))
                {
                    db.clan.Add(
                        new Database.clan
                        {
                            ClanId = a.GetAllianceId(),
                            LastUpdateTime = DateTime.Now,
                            Data = a.SaveToJSON()
                        }
                    );
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Debugger.WriteLine("An exception occured during CreateAlliance processing:", ex);
            }
        }

        public Level GetAccount(long playerId)
        {
            Level account = null;
            try
            {  
                using (var db = new Database.ucsdbEntities(m_vConnectionString))
                {
                    var p = db.player.Find(playerId);

                    //Check if player exists
                    if (p != null)
                    {
                        account = new Level();
                        account.SetAccountStatus(p.AccountStatus);
                        account.SetAccountPrivileges(p.AccountPrivileges);
                        account.SetTime(p.LastUpdateTime);
                        account.GetPlayerAvatar().LoadFromJSON(p.Avatar);
                        account.LoadFromJSON(p.GameObjects);
                    }
                }
            }
            catch (Exception ex)
            {
                Debugger.WriteLine("An exception occured during GetAccount processing:", ex);
            }
            return account;
        }

        public Alliance GetAlliance(long allianceId)
        {
            Alliance alliance = null;
            try
            {
                using (var db = new Database.ucsdbEntities(m_vConnectionString))
                {
                    var p = db.clan.Find(allianceId);

                    //Check if player exists
                    if (p != null)
                    {
                        alliance = new Alliance();
                        alliance.LoadFromJSON(p.Data);
                    }
                }
            }
            catch (Exception ex)
            {
                Debugger.WriteLine("An exception occured during GetAlliance processing:", ex);
            }
            return alliance;
        }

        public long GetMaxAllianceId()
        {
            long max = 0;
            using (var db = new Database.ucsdbEntities(m_vConnectionString))
            {
                max = (from alliance in db.clan
                       select (long?)alliance.ClanId ?? 0).DefaultIfEmpty().Max();
            }
            return max;
        }

        public long GetMaxPlayerId()
        {
            long max = 0;
            using (var db = new Database.ucsdbEntities(m_vConnectionString))
            {
                max = (from ep in db.player
                       select (long?)ep.PlayerId ?? 0).DefaultIfEmpty().Max();
            }
            return max;
        }

        public void Save(List<Level> avatars)
        {
            Debugger.WriteLine("Starting saving players from memory to database at " + DateTime.Now.ToString());
            try
            {
                using (var context = new Database.ucsdbEntities(m_vConnectionString))
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.Configuration.ValidateOnSaveEnabled = false;
                    int transactionCount = 0;
                    foreach (Level pl in avatars)
                    {
                        lock (pl)
                        {
                            var p = context.player.Find(pl.GetPlayerAvatar().GetId());
                            if (p != null)
                            {
                                p.LastUpdateTime = pl.GetTime();
                                p.AccountStatus = pl.GetAccountStatus();
                                p.AccountPrivileges = pl.GetAccountPrivileges();
                                p.Avatar = pl.GetPlayerAvatar().SaveToJSON();
                                p.GameObjects = pl.SaveToJSON();
                                context.Entry(p).State = EntityState.Modified;
                            }
                            else
                            {
                                context.player.Add(
                                    new Database.player
                                    {
                                        PlayerId = pl.GetPlayerAvatar().GetId(),
                                        AccountStatus = pl.GetAccountStatus(),
                                        AccountPrivileges = pl.GetAccountPrivileges(),
                                        LastUpdateTime = pl.GetTime(),
                                        Avatar = pl.GetPlayerAvatar().SaveToJSON(),
                                        GameObjects = pl.SaveToJSON()
                                    }
                                );
                            }
                        }
                        transactionCount++;
                        if (transactionCount >= 500)
                        {
                            context.SaveChanges();
                            transactionCount = 0;
                        }
                    }
                    context.SaveChanges();
                }
                Debugger.WriteLine("Finished saving players from memory to database at " + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                Debugger.WriteLine("An exception occured during Save processing for avatars:", ex);
            } 
        }

        public void Save(List<Alliance> alliances)
        {
            Debugger.WriteLine("Starting saving alliances from memory to database at " + DateTime.Now.ToString());
            try
            {
                using (var context = new Database.ucsdbEntities(m_vConnectionString))
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.Configuration.ValidateOnSaveEnabled = false;
                    int transactionCount = 0;
                    foreach (Alliance alliance in alliances)
                    {
                        lock (alliance)
                        {
                            var c = context.clan.Find((int)alliance.GetAllianceId());
                            if (c != null)
                            {
                                c.LastUpdateTime = DateTime.Now;
                                c.Data = alliance.SaveToJSON();
                                context.Entry(c).State = EntityState.Modified;
                            }
                            else
                            {
                                context.clan.Add(
                                    new Database.clan
                                    {
                                        ClanId = alliance.GetAllianceId(),
                                        LastUpdateTime = DateTime.Now,
                                        Data = alliance.SaveToJSON()
                                    }
                                );
                            }
                        }
                        transactionCount++;
                        if (transactionCount >= 500)
                        {
                            context.SaveChanges();
                            transactionCount = 0;
                        }
                    }
                    context.SaveChanges();
                }
                Debugger.WriteLine("Finished saving alliances from memory to database at " + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                Debugger.WriteLine("An exception occured during Save processing for alliances:", ex);
            }
        }
    }
}
