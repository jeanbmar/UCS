using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing
{
    //Packet 20104
    class LoginOkMessage : Message
    {
        private long m_vAccountId;
        private string m_vPassToken;
        private string m_vFacebookId;
        private string m_vGamecenterId;
        private int m_vServerMajorVersion;
        private int m_vServerBuild;
        private int m_vContentVersion;
        private string m_vServerEnvironment;
        private int m_vSessionCount;
        private int m_vDaysSinceStartedPlaying;
        private string m_vServerTime;
        private int m_vPlayTimeSeconds;
        private string m_vAccountCreatedDate;
        private int m_vStartupCooldownSeconds;
        private string m_vCountryCode;
        
        public LoginOkMessage(Client client) : base (client)
        {
            SetMessageType(20104);
            SetMessageVersion(1);

            Unknown11 = "someid2";//"108457211027966753069";
        }

        public override void Encode()
        {
            List<Byte> pack = new List<Byte>();

            pack.AddInt64(m_vAccountId);
            pack.AddInt64(m_vAccountId);
            pack.AddString(m_vPassToken);
            pack.AddString(m_vFacebookId);
            pack.AddString(m_vGamecenterId);
            pack.AddInt32(m_vServerMajorVersion);
            pack.AddInt32(m_vServerBuild);
            pack.AddInt32(m_vContentVersion);
            pack.AddString(m_vServerEnvironment);
            pack.AddInt32(m_vDaysSinceStartedPlaying);
            pack.AddInt32(m_vPlayTimeSeconds);
            pack.AddInt32(m_vSessionCount);
            pack.AddString("someid1");//"297484437009394";
            pack.AddString(m_vServerTime);
            pack.AddString(m_vAccountCreatedDate);
            pack.AddInt32(m_vStartupCooldownSeconds);
            pack.AddString("someid2");//"108457211027966753069";
            pack.AddString(m_vCountryCode);

            SetData(pack.ToArray());
        }

        public void SetAccountCreatedDate(string date)
        {
            m_vAccountCreatedDate = date;
        }

        public void SetAccountId(long id)
        {
            m_vAccountId = id;
        }

        public void SetContentVersion(int version)
        {
            m_vContentVersion = version;
        }

        public void SetCountryCode(string code)
        {
            m_vCountryCode = code;
        }

        public void SetDaysSinceStartedPlaying(int days)
        {
            m_vDaysSinceStartedPlaying = days;
        }

        public void SetFacebookId(string id)
        {
            m_vFacebookId = id;
        }

        public void SetGamecenterId(string id)
        {
            m_vGamecenterId = id;
        }

        public void SetPassToken(string token)
        {
            m_vPassToken = token;
        }

        public void SetPlayTimeSeconds(int seconds)
        {
            m_vPlayTimeSeconds = seconds;
        }

        public void SetServerBuild(int build)
        {
            m_vServerBuild = build;
        }

        public void SetServerEnvironment(string env)
        {
            m_vServerEnvironment = env;
        }

        public void SetServerMajorVersion(int version)
        {
            m_vServerMajorVersion = version;
        }

        public void SetServerTime(string time)
        {
            m_vServerTime = time;
        }

        public void SetSessionCount(int count)
        {
            m_vSessionCount = count;
        }

        public void SetStartupCooldownSeconds(int seconds)
        {
            m_vStartupCooldownSeconds = seconds;
        }

        public String Unknown9 { get; set; } //32 39 37 34 38 34 34 33 37 30 30 39 33 39 34
        public String Unknown11 { get; set; }
    }
}
