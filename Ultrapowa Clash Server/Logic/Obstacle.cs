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
using UCS.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UCS.Logic
{
    class Obstacle : GameObject
    {
        private Timer m_vTimer;
        private Level m_vLevel;

        public override int ClassId
        {
            get { return 3; }
        }

        public Obstacle(Data data, Level l) : base(data, l)
        {
            m_vLevel = l;
        }

        public ObstacleData GetObstacleData()
        {
            return (ObstacleData)GetData();
        }

        public void StartClearing()
        {
            int constructionTime = GetObstacleData().ClearTimeSeconds;
            if (constructionTime < 1)
            {
                ClearingFinished();
            }
            else
            {
                m_vTimer = new Timer();
                m_vTimer.StartTimer(constructionTime, m_vLevel.GetTime());
                m_vLevel.WorkerManager.AllocateWorker(this);
            }
        }

        public void CancelClearing()
        {
            m_vLevel.WorkerManager.DeallocateWorker(this);
            m_vTimer = null;
            var od = GetObstacleData();
            var rd = od.GetClearingResource();
            int cost = od.ClearCost;
            GetLevel().GetPlayerAvatar().CommodityCountChangeHelper(0, rd, cost);
        }

        public int GetRemainingClearingTime()
        {
            return m_vTimer.GetRemainingSeconds(m_vLevel.GetTime());
        }

        public override void Tick()
        {
            if (IsClearingOnGoing())
            {
                if (m_vTimer.GetRemainingSeconds(m_vLevel.GetTime()) <= 0)
                    ClearingFinished();
            }
        }

        public void ClearingFinished()
        {
            //gérer achievement
            //gérer xp reward
            //gérer obstacleclearcounter
            //gérer diamond reward
            m_vLevel.WorkerManager.DeallocateWorker(this);
            m_vTimer = null;
        }

        public void SpeedUpClearing()
        {
            int remainingSeconds = 0;
            if(IsClearingOnGoing())
            {
                remainingSeconds = m_vTimer.GetRemainingSeconds(m_vLevel.GetTime());
            }
            int cost = GamePlayUtil.GetSpeedUpCost(remainingSeconds);
            var ca = GetLevel().GetPlayerAvatar();
            if (ca.HasEnoughDiamonds(cost))
            {
                ca.UseDiamonds(cost);
                ClearingFinished();
            }
        }

        public bool IsClearingOnGoing()
        {
            return (m_vTimer != null);
        }

        public JObject ToJson()
        {
            JObject jsonObject = new JObject();
            jsonObject.Add("data", GetObstacleData().GetGlobalID());
            //const_t à vérifier pour un obstacle
            if (IsClearingOnGoing())
                jsonObject.Add("const_t", m_vTimer.GetRemainingSeconds(m_vLevel.GetTime()));
            jsonObject.Add("x", this.X);
            jsonObject.Add("y", this.Y);
            return jsonObject;
        }
    }
}
