using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.PacketProcessing;
using UCS.Helpers;

namespace UCS.Logic
{
    class StreamEntry
    {
        private int m_vId;
        private long m_vSenderId;
        private long m_vHomeId;
        private string m_vSenderName;
        private int m_vSenderLeagueId;
        private int m_vSenderLevel;
        private int m_vSenderRole;
        private DateTime m_vMessageTime;
        
        public StreamEntry()
        {
            m_vMessageTime = DateTime.UtcNow;
        }

        public int GetAgeSeconds()
        {
            return (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds - (int)m_vMessageTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public int GetId()
        {
            return m_vId;
        }

        public long GetHomeId()
        {
            return m_vHomeId;
        }

        public int GetSenderLeagueId()
        {
            return m_vSenderLeagueId;
        }

        public long GetSenderId()
        {
            return m_vSenderId;
        }

        public int GetSenderLevel()
        {
            return m_vSenderLevel;
        }

        public string GetSenderName()
        {
            return m_vSenderName;
        }

        public int GetSenderRole()
        {
            return m_vSenderRole;
        }

        public virtual int GetStreamEntryType()
        {
            return -1;
        }

        public virtual byte[] Encode()
        {
            List<Byte> data = new List<Byte>();

            data.AddInt32(GetStreamEntryType());//chatstreamentry
            data.AddInt32(0);
            data.AddInt32(m_vId);
            data.Add(3);
            data.AddInt64(m_vSenderId);
            data.AddInt64(m_vHomeId);
            data.AddString(m_vSenderName);
            data.AddInt32(m_vSenderLevel);
            data.AddInt32(m_vSenderLeagueId);
            data.AddInt32(m_vSenderRole);
            data.AddInt32(GetAgeSeconds());

            return data.ToArray();
        }

        public void SetAvatar(ClientAvatar avatar)
        {
            m_vSenderId = avatar.GetId();
            m_vHomeId = avatar.GetId() ;
            m_vSenderName = avatar.GetAvatarName();
            m_vSenderLeagueId = avatar.GetLeagueId();
            m_vSenderLevel = avatar.GetAvatarLevel();
            m_vSenderRole = 1;
        }

        public void SetHomeId(long id)
        {
            m_vHomeId = id;
        }

        public void SetId(int id)
        {
            m_vId = id;
        }

        public void SetSenderId(long id)
        {
            m_vSenderId = id;
        }

        public void SetSenderLeagueId(int leagueId)
        {
            m_vSenderLeagueId = leagueId;
        }

        public void SetSenderLevel(int level)
        {
            m_vSenderLevel = level;
        }

        public void SetSenderName(string name)
        {
            m_vSenderName = name;
        }

        public void SetSenderRole(int role)
        {
            m_vSenderRole = role;
        }
    }    
}
