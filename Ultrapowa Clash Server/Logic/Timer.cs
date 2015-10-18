using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Configuration;
using UCS.PacketProcessing;
using UCS.Core;
using UCS.GameFiles;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UCS.Logic
{
    class Timer
    {
        private DateTime m_vStartTime;
        private int m_vSeconds;

        public Timer()
        {
            m_vStartTime = new DateTime(1970, 1, 1);
            m_vSeconds = 0;
        }

        public void FastForward(int seconds)
        {
            m_vSeconds -= seconds;
        }

        public int GetRemainingSeconds(DateTime time)
        {
            int result = m_vSeconds - (int)time.Subtract(m_vStartTime).TotalSeconds;
            if (result <= 0)
                result = 0;
            return result;
        }

        public void StartTimer(int seconds, DateTime time)
        {
            m_vStartTime = time;
            m_vSeconds = seconds;
        }
    }
}
