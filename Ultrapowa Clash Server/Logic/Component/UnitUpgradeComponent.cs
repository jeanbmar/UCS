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
    class UnitUpgradeComponent : Component
    {
        private Timer m_vTimer;//a1 + 12
        private CombatItemData m_vCurrentlyUpgradedUnit;//a1 + 16
        //a1 + 20 -- Listener?

        public UnitUpgradeComponent(GameObject go) : base(go)
        {
            m_vTimer = null;
            m_vCurrentlyUpgradedUnit = null;
        }

        public bool CanStartUpgrading(CombatItemData cid)
        {
            bool result = false;
            if (m_vCurrentlyUpgradedUnit == null)
            {
                Building b = (Building)GetParent();
                ClientAvatar ca = GetParent().GetLevel().GetHomeOwnerAvatar();
                ComponentManager cm = GetParent().GetLevel().GetComponentManager();
                int maxProductionBuildingLevel;
                if(cid.GetCombatItemType() == 1)
                    maxProductionBuildingLevel = cm.GetMaxSpellForgeLevel();
                else
                    maxProductionBuildingLevel = cm.GetMaxBarrackLevel();
                if (ca.GetUnitUpgradeLevel(cid) < cid.GetUpgradeLevelCount() - 1)
                {
                    if(maxProductionBuildingLevel >= cid.GetRequiredProductionHouseLevel() - 1)
                    {
                        result = (b.GetUpgradeLevel() >= (cid.GetRequiredLaboratoryLevel(ca.GetUnitUpgradeLevel(cid) + 1)) - 1);
                    }
                }
            }
            return result;
        }

        public void FinishUpgrading()
        {
            if(m_vCurrentlyUpgradedUnit != null)
            {
                ClientAvatar ca = GetParent().GetLevel().GetHomeOwnerAvatar();
                int level = ca.GetUnitUpgradeLevel(m_vCurrentlyUpgradedUnit);
                ca.SetUnitUpgradeLevel(m_vCurrentlyUpgradedUnit, level + 1);
            }
            m_vTimer = null;
            m_vCurrentlyUpgradedUnit = null;
        }

        public CombatItemData GetCurrentlyUpgradedUnit()
        {
            return m_vCurrentlyUpgradedUnit;
        }

        public int GetRemainingSeconds()
        {
            int result = 0;
            if (m_vTimer != null)
            {
                result = m_vTimer.GetRemainingSeconds(GetParent().GetLevel().GetTime());
            }
            return result;
        }

        public int GetTotalSeconds()
        {
            int result = 0;
            if (m_vCurrentlyUpgradedUnit != null)
            {
                ClientAvatar ca = GetParent().GetLevel().GetHomeOwnerAvatar();
                int level = ca.GetUnitUpgradeLevel(m_vCurrentlyUpgradedUnit);
                result = m_vCurrentlyUpgradedUnit.GetUpgradeTime(level);
            }
            return result;
        }

        public override void Load(JObject jsonObject)
        {
            JObject unitUpgradeObject = (JObject)jsonObject["unit_upg"];
            if (unitUpgradeObject != null)
            {
                m_vTimer = new Timer();
                int remainingTime = unitUpgradeObject["t"].ToObject<int>();
                m_vTimer.StartTimer(remainingTime, GetParent().GetLevel().GetTime());

                int id = unitUpgradeObject["id"].ToObject<int>();
                m_vCurrentlyUpgradedUnit = (CombatItemData)ObjectManager.DataTables.GetDataById(id);
            }
        }

        public override JObject Save(JObject jsonObject)
        {
            //{"data":1000007,"lvl":7,"x":4,"y":4,"unit_upg":{"unit_type":0,"t":591612,"id":4000001},"l1x":32,"l1y":32}

            if(m_vCurrentlyUpgradedUnit != null)
            {
                JObject unitUpgradeObject = new JObject();

                unitUpgradeObject.Add("unit_type", m_vCurrentlyUpgradedUnit.GetCombatItemType());
                unitUpgradeObject.Add("t", m_vTimer.GetRemainingSeconds(GetParent().GetLevel().GetTime()));
                unitUpgradeObject.Add("id", m_vCurrentlyUpgradedUnit.GetGlobalID());
                jsonObject.Add("unit_upg", unitUpgradeObject);
            }
            return jsonObject;
        }

        public void SpeedUp()
        {
            if(m_vCurrentlyUpgradedUnit != null)
            {
                int remainingSeconds = 0;
                if(m_vTimer != null)
                {
                    remainingSeconds = m_vTimer.GetRemainingSeconds(GetParent().GetLevel().GetTime());
                }
                int cost = GamePlayUtil.GetSpeedUpCost(remainingSeconds);
                var ca = GetParent().GetLevel().GetPlayerAvatar();
                if (ca.HasEnoughDiamonds(cost))
                {
                    ca.UseDiamonds(cost);
                    FinishUpgrading();
                }
            }
        }

        public void StartUpgrading(CombatItemData cid)
        {
            if(CanStartUpgrading(cid))
            {
                m_vCurrentlyUpgradedUnit = cid;
                m_vTimer = new Timer();
                m_vTimer.StartTimer(GetTotalSeconds(), GetParent().GetLevel().GetTime());
            }
        }

        public override void Tick()
        {
            if (m_vTimer != null)
            {
                if (m_vTimer.GetRemainingSeconds(GetParent().GetLevel().GetTime()) <= 0)
                {
                    FinishUpgrading();
                }
            }
        }

        public override int Type
        {
            get { return 9; }
        }
    }
}
