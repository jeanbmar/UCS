using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UCS.PacketProcessing;
using UCS.Helpers;

namespace UCS.Logic
{
    class Alliance
    {
        private long m_vAllianceId;
        private string m_vAllianceName;
        private string m_vAllianceDescription;
        private int m_vAllianceBadgeData;
        private int m_vAllianceType;
        private int m_vRequiredScore;
        private int m_vWarFrequency;
        private int m_vAllianceOrigin;
        private int m_vScore;
        private int m_vAllianceExperience;
        private int m_vAllianceLevel;
        private int m_vWonWars;
        private int m_vLostWars;
        private int m_vDrawWars;
        private List<StreamEntry> m_vChatMessages;
        private const int m_vMaxChatMessagesNumber = 30;
        private Dictionary<long, AllianceMemberEntry> m_vAllianceMembers;
        private const int m_vMaxAllianceMembers = 50;

        public Alliance() {
            m_vChatMessages = new List<StreamEntry>();
            m_vAllianceMembers = new Dictionary<long, AllianceMemberEntry>();
        }

        public Alliance(long id)
        {
            m_vAllianceId = id;
            m_vAllianceName = "Default";
            m_vAllianceDescription = "Default";
            m_vAllianceBadgeData = 0;
            m_vAllianceType = 0;
            m_vRequiredScore = 0;
            m_vWarFrequency = 0;
            m_vAllianceOrigin = 0x01E84857;
            m_vScore = 10000;
            m_vAllianceExperience = 8599;
            m_vAllianceLevel = 10;
            m_vWonWars = 0;
            m_vLostWars = 0;
            m_vDrawWars = 0;
            m_vChatMessages = new List<StreamEntry>();
            m_vAllianceMembers = new Dictionary<long, AllianceMemberEntry>();
        }

        public byte[] EncodeFullEntry()
        {
            List<Byte> data = new List<Byte>();

            data.AddInt64(m_vAllianceId);
            data.AddString(m_vAllianceName);
            data.AddInt32(m_vAllianceBadgeData);
            data.AddInt32(m_vAllianceType);
            data.AddInt32(m_vAllianceMembers.Count);
            data.AddInt32(m_vScore);
            data.AddInt32(m_vRequiredScore);
            data.AddInt32(m_vWonWars);
            data.AddInt32(m_vLostWars);
            data.AddInt32(m_vDrawWars);
            data.AddInt32(0x001E8481);
            data.AddInt32(m_vWarFrequency);
            data.AddInt32(m_vAllianceOrigin);
            data.AddInt32(m_vAllianceExperience);
            data.AddInt32(m_vAllianceLevel);

            return data.ToArray();
        }

        
        public byte[] EncodeHeader()
        {
            //00 00 00 46 00 03 46 FE 
            //00 00 00 0B 
            //4C 61 20 54 65 61 6D 20 54 44 41 
            //5E 00 2C 5A 
            //00 
            //00 00 00 02 
            //00 00 00 01 
            //FF FF FF FF

            List<Byte> data = new List<Byte>();

            data.AddInt64(m_vAllianceId);
            data.AddString(m_vAllianceName);
            data.AddInt32(m_vAllianceBadgeData);
            data.Add(0);
            data.AddInt32(m_vAllianceLevel);
            data.AddInt32(1);
            data.AddInt32(-1);

            return data.ToArray();
        }

        public byte[] EncodeMembers()
        {
            List<Byte> data = new List<Byte>();
            return data.ToArray();
        }

        public void AddAllianceMember(AllianceMemberEntry entry)
        {
            m_vAllianceMembers.Add(entry.GetAvatarId(), entry);
        }

        public void AddChatMessage(StreamEntry message)
        {
            while (m_vChatMessages.Count >= m_vMaxChatMessagesNumber)
            {
                m_vChatMessages.RemoveAt(0);
            }
            m_vChatMessages.Add(message);
        }

        public bool IsAllianceFull()
        {
            return (m_vAllianceMembers.Count >= m_vMaxAllianceMembers);
        }

        public int GetAllianceBadgeData()
        {
            return m_vAllianceBadgeData;
        }

        public string GetAllianceDescription()
        {
            return m_vAllianceDescription;
        }

        public int GetAllianceExperience()
        {
            return m_vAllianceExperience;
        }

        public long GetAllianceId()
        {
            return m_vAllianceId;
        }

        public int GetAllianceLevel()
        {
            return m_vAllianceLevel;
        }

        public List<AllianceMemberEntry> GetAllianceMembers()
        {
            return m_vAllianceMembers.Values.ToList();
        }

        public AllianceMemberEntry GetAllianceMember(long avatarId)
        {
            return m_vAllianceMembers[avatarId];
        }

        public string GetAllianceName()
        {
            return m_vAllianceName;
        }

        public int GetAllianceOrigin()
        {
            return m_vAllianceOrigin;
        }

        public int GetAllianceType()
        {
            return m_vAllianceType;
        }

        public List<StreamEntry> GetChatMessages()
        {
            return m_vChatMessages;
        }

        public int GetRequiredScore()
        {
            return m_vRequiredScore;
        }

        public int GetScore()
        {
            return m_vScore;
        }

        public int GetWarScore()
        {
            return m_vWonWars;
        }

        public int GetWarFrequency()
        {
            return m_vWarFrequency;
        }

        public void RemoveMember(long avatarId)
        {
            m_vAllianceMembers.Remove(avatarId);
        }

        public string SaveToJSON()
        {
            JObject jsonData = new JObject();

            jsonData.Add("alliance_id", m_vAllianceId);
            jsonData.Add("alliance_name", m_vAllianceName);
            jsonData.Add("alliance_badge", m_vAllianceBadgeData);
            jsonData.Add("alliance_type", m_vAllianceType);
            jsonData.Add("score", m_vScore);
            jsonData.Add("required_score", m_vRequiredScore);
            jsonData.Add("description", m_vAllianceDescription);
            jsonData.Add("alliance_experience", m_vAllianceExperience);
            jsonData.Add("alliance_level", m_vAllianceLevel);
            jsonData.Add("won_wars", m_vWonWars);
            jsonData.Add("lost_wars", m_vLostWars);
            jsonData.Add("draw_wars", m_vDrawWars);
            jsonData.Add("war_frequency", m_vWarFrequency);
            jsonData.Add("alliance_origin", m_vAllianceOrigin);

            JArray jsonMembersArray = new JArray();
            foreach(var member in m_vAllianceMembers.Values)
            {
                JObject jsonObject = new JObject();
                member.Save(jsonObject);
                jsonMembersArray.Add(jsonObject);
            }
            jsonData.Add("members", jsonMembersArray);

            return JsonConvert.SerializeObject(jsonData);
        }

        public void LoadFromJSON(string jsonString)
        {
            JObject jsonObject = JObject.Parse(jsonString);

            m_vAllianceId = jsonObject["alliance_id"].ToObject<long>();
            m_vAllianceName = jsonObject["alliance_name"].ToObject<string>();
            m_vAllianceBadgeData = jsonObject["alliance_badge"].ToObject<int>();
            m_vAllianceType = jsonObject["alliance_type"].ToObject<int>();
            if (jsonObject["required_score"] != null)
                m_vRequiredScore = jsonObject["required_score"].ToObject<int>();
            m_vScore = jsonObject["score"].ToObject<int>();
            m_vAllianceDescription = jsonObject["description"].ToObject<string>();
            m_vAllianceExperience = jsonObject["alliance_experience"].ToObject<int>();
            m_vAllianceLevel = jsonObject["alliance_level"].ToObject<int>();
            if (jsonObject["won_wars"] != null)
                m_vWonWars = jsonObject["won_wars"].ToObject<int>();
            if (jsonObject["lost_wars"] != null)
                m_vLostWars = jsonObject["lost_wars"].ToObject<int>();
            if (jsonObject["draw_wars"] != null)
                m_vDrawWars = jsonObject["draw_wars"].ToObject<int>();
            if (jsonObject["war_frequency"] != null)
                m_vWarFrequency = jsonObject["war_frequency"].ToObject<int>();
            if (jsonObject["alliance_origin"] != null)
                m_vAllianceOrigin = jsonObject["alliance_origin"].ToObject<int>();

            JArray jsonMembers = (JArray)jsonObject["members"];
            foreach (JObject jsonMember in jsonMembers)
            {
                long id = jsonMember["avatar_id"].ToObject<long>();
                AllianceMemberEntry member = new AllianceMemberEntry(id);
                member.Load(jsonMember);
                m_vAllianceMembers.Add(id, member);
            }
        }

        public void SetAllianceBadgeData(int data)
        {
            m_vAllianceBadgeData = data;
        }

        public void SetAllianceDescription(string description)
        {
            m_vAllianceDescription = description;
        }

        public void SetAllianceLevel(int level)
        {
            m_vAllianceLevel = level;
        }

        public void SetAllianceName(string name)
        {
            m_vAllianceName = name;
        }

        public void SetAllianceOrigin(int origin)
        {
            m_vAllianceOrigin = origin;
        }

        public void SetAllianceType(int status)
        {
            m_vAllianceType = status;
        }

        public void SetRequiredScore(int score)
        {
            m_vRequiredScore = score;
        }

        public void SetWarFrequency(int frequency)
        {
            m_vWarFrequency = frequency;
        }
    }
}
