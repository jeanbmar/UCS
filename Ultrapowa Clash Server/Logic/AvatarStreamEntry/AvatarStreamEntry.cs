using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.PacketProcessing;
using UCS.Helpers;

namespace UCS.Logic
{
    class AvatarStreamEntry
    {
        private int m_vId;
        private long m_vSenderId;
        private string m_vSenderName;
        private int m_vSenderLevel;
        private int m_vSenderLeagueId;
        private DateTime m_vCreationTime;
        private byte m_vIsNew;
        //private byte m_vIsRemoved;

        public AvatarStreamEntry()
        {
            m_vCreationTime = DateTime.UtcNow;
        }

        public int GetAgeSeconds()
        {
            return (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds - (int)m_vCreationTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public int GetId()
        {
            return m_vId;
        }

        public long GetSenderAvatarId()
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

        public virtual int GetStreamEntryType()
        {
            return -1;
        }

        public virtual byte[] Encode()
        {
            List<Byte> data = new List<Byte>();

            data.AddInt32(GetStreamEntryType());//alliancemailstreamentry
            data.AddInt32(0);
            data.AddInt32(m_vId);
            data.AddInt64(m_vSenderId);
            data.AddString(m_vSenderName);
            data.AddInt32(m_vSenderLevel);
            data.AddInt32(m_vSenderLeagueId);
            data.Add(m_vIsNew);

            return data.ToArray();
        }

        public byte IsNew()
        {
            return m_vIsNew;
        }

        public void SetAvatar(ClientAvatar avatar)
        {
            m_vSenderId = avatar.GetId();
            m_vSenderName = avatar.GetAvatarName();
            m_vSenderLevel = avatar.GetAvatarLevel();
            m_vSenderLeagueId = avatar.GetLeagueId();
        }

        public void SetId(int id)
        {
            m_vId = id;
        }

        public void SetIsNew(byte isNew)
        {
            m_vIsNew = isNew;
        }

        public void SetSenderLeagueId(int id)
        {
            m_vSenderLeagueId = id;
        }

        /*public void SetRemoved(byte removed)
        {
            m_vIsRemoved = removed;
        }*/

        public void SetSenderAvatarId(long id)
        {
            m_vSenderId = id;
        }

        public void SetSenderLevel(int level)
        {
            m_vSenderLevel = level;
        }

        public void SetSenderName(string name)
        {
            m_vSenderName = name;
        }
    }    
}
