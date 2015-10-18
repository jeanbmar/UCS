using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing
{
    //Packet 20103
    class LoginFailedMessage : Message
    {
        private int m_vErrorCode;//48
        private string m_vResourceFingerprintData;//52
        private string m_vRedirectDomain;//56
        private string m_vContentURL;//60
        private string m_vUpdateURL;//64
        private string m_vReason;//68
        
        public LoginFailedMessage(Client client) : base(client)
        {
            SetMessageType(20103);
            //SetMessageVersion(3);

            //errorcodes:
            //9: removeredirectdomain
            //8: new game version available (removeupdateurl)
            //7: removeresourcefingerprintdata
            //10: maintenance
            //11: banni temporairement
            //12: played too much
            //13: compte verrouillé
        }

        public override void Encode()
        {
            List<Byte> pack = new List<Byte>();

            pack.AddInt32(m_vErrorCode);
            pack.AddString(m_vResourceFingerprintData);
            pack.AddString(m_vRedirectDomain);
            pack.AddString(m_vContentURL);
            pack.AddString(m_vUpdateURL);
            pack.AddString(m_vReason);
            pack.AddInt32(-1);
            pack.Add(0);

            SetData(pack.ToArray());
        }

        public void SetContentURL(string url)
        {
            m_vContentURL = url;
        }

        public void SetErrorCode(int code)
        {
            m_vErrorCode = code;
        }

        public void SetReason(string reason)
        {
            m_vReason = reason;
        }

        public void SetRedirectDomain(string domain)
        {
            m_vRedirectDomain = domain;
        }

        public void SetResourceFingerprintData(string data)
        {
            m_vResourceFingerprintData = data;
        }

        public void SetUpdateURL(string url)
        {
            m_vUpdateURL = url;
        }

    }
}
