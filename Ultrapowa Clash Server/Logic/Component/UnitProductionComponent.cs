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
    class UnitProductionComponent : Component
    {
        private List<DataSlot> m_vUnits;//a1 + 12
        private Timer m_vTimer;//a1 + 16
        private bool m_vIsWaitingForSpace;//a1 + 20
        private bool m_vIsSpellForge;//a1 + 24

        public UnitProductionComponent(GameObject go) : base(go)
        {
            m_vUnits = new List<DataSlot>();
            SetUnitType(go);
            m_vTimer = null;
            m_vIsWaitingForSpace = false;
        }

        public void AddUnitToProductionQueue(CombatItemData cd)
        {
            if(CanAddUnitToQueue(cd))
            {
                for(int i=0;i<GetSlotCount();i++)
                {
                    if ((CombatItemData)m_vUnits[i].Data == cd)
                    {
                        m_vUnits[i].Value++;
                        return;
                    }
                }
                DataSlot ds = new DataSlot(cd, 1);
                m_vUnits.Add(ds);
                if(m_vTimer == null)
                {
                    ClientAvatar ca = GetParent().GetLevel().GetHomeOwnerAvatar();
                    m_vTimer = new Timer();
                    int trainingTime = cd.GetTrainingTime(ca.GetUnitUpgradeLevel(cd));
                    m_vTimer.StartTimer(trainingTime, GetParent().GetLevel().GetTime());
                }
            }
        }

        public bool CanAddUnitToQueue(CombatItemData cd)
        {
            //Console.WriteLine(GetMaxTrainCount());
            //Console.WriteLine(GetTotalCount());
            //Console.WriteLine(cd.GetHousingSpace());
            return (GetMaxTrainCount() >= GetTotalCount() + cd.GetHousingSpace());
        }

        public CombatItemData GetCurrentlyTrainedUnit()
        {
            CombatItemData cd = null;
            if (m_vUnits.Count >= 1)
                cd = (CombatItemData)m_vUnits[0].Data;
            return cd;
        }

        public int GetMaxTrainCount()
        {
            Building b = (Building)GetParent();
            BuildingData bd = b.GetBuildingData();
            return bd.GetUnitProduction(b.GetUpgradeLevel());
        }

        public int GetSlotCount()
        {
            return m_vUnits.Count;
        }

        public int GetTotalCount()
        {
            int count = 0;
            if(GetSlotCount() >= 1)
            {
                for(int i=0;i<GetSlotCount();i++)
                {
                    int cnt = m_vUnits[i].Value;
                    int housingSpace = ((CombatItemData)m_vUnits[i].Data).GetHousingSpace();
                    count += cnt * housingSpace;
                }
            }
            if (m_vIsSpellForge)
            {
                count += GetParent().GetLevel().GetComponentManager().GetTotalUsedHousing(true);
            }
            return count;
        }

        public int GetTotalRemainingSeconds()
        {
            int result = 0;
            bool firstUnit = true;
            if(m_vUnits.Count > 0)
            {
                foreach(var ds in m_vUnits)
                {
                    CombatItemData cd = (CombatItemData)ds.Data;
                    if(cd != null)
                    {
                        int count = ds.Value;
                        if(count >= 1)
                        {
                            if(firstUnit)
                            {
                                if (m_vTimer != null)
                                    result += m_vTimer.GetRemainingSeconds(GetParent().GetLevel().GetTime());
                                count--;
                                firstUnit = false;
                            }
                            ClientAvatar ca = GetParent().GetLevel().GetHomeOwnerAvatar();
                            result += count * cd.GetTrainingTime(ca.GetUnitUpgradeLevel(cd));
                        }
                    }
                }
            }
            return result;
        }

        public int GetTrainingCount(int index)
        {
            return m_vUnits[index].Value;
        }

        public CombatItemData GetUnit(int index)
        {
            return (CombatItemData)m_vUnits[index].Data;
        }

        public bool HasHousingSpaceForSpeedUp()
        {
            int totalRoom = 0;
            if(m_vUnits.Count >= 1)
            {
                foreach(var ds in m_vUnits)
                {
                    CombatItemData cd = (CombatItemData)ds.Data;
                    totalRoom += cd.GetHousingSpace() * ds.Value;
                }
            }
            ComponentManager cm = GetParent().GetLevel().GetComponentManager();
            int usedHousing = cm.GetTotalUsedHousing(m_vIsSpellForge);
            int maxHousing = cm.GetTotalMaxHousing(m_vIsSpellForge);
            return (totalRoom <= maxHousing - usedHousing);
        }

        public bool IsSpellForge()
        {
            return m_vIsSpellForge;
        }

        public bool IsWaitingForSpace()
        {
            bool result = false;
            if(m_vUnits.Count > 0)
            {
                if(m_vTimer != null)
                {
                    if (m_vTimer.GetRemainingSeconds(GetParent().GetLevel().GetTime()) == 0)
                    {
                        result = m_vIsWaitingForSpace;
                    }
                }
            }
            return result;
        }

        public bool ProductionCompleted()
        {
            bool result = false;
            //localiser le camp le plus proche pour envoyer l'unit
            //incrementer ce camp
            ComponentFilter cf = new ComponentFilter(0);
            int x = GetParent().X;
            int y = GetParent().Y;
            ComponentManager cm = GetParent().GetLevel().GetComponentManager();
            Component c = cm.GetClosestComponent(x, y, cf);

            while (c != null)
            {
                Data d = null;
                if (m_vUnits.Count > 0)
                    d = m_vUnits[0].Data;
                if (!((UnitStorageComponent)c).CanAddUnit((CombatItemData)d))
                {
                    //Storage camp is full
                    cf.AddIgnoreObject(c.GetParent());
                    c = cm.GetClosestComponent(x, y, cf);
                }
                else
                    break;
            }

            if(c != null)
            {
                var cd = (CombatItemData)m_vUnits[0].Data;
                ((UnitStorageComponent)c).AddUnit(cd);
                StartProducingNextUnit();             
                result = true;
            }
            else
            {
                m_vIsWaitingForSpace = true;
            }
            return result;
        }

        public void RemoveUnit(CombatItemData cd)
        {
            int index = -1;
            if(GetSlotCount() >= 1)
            {
                for(int i=0;i<GetSlotCount();i++)
                {
                    if (m_vUnits[i].Data == cd)
                        index = i;
                }
            }
            if(index != -1)
            {
                if (m_vUnits[index].Value >= 1)
                {
                    m_vUnits[index].Value--;
                    if (m_vUnits[index].Value == 0)
                    {
                        m_vUnits.RemoveAt(index);
                        if(GetSlotCount() >= 1)
                        {
                            DataSlot ds = m_vUnits[0];
                            CombatItemData newcd = (CombatItemData)m_vUnits[0].Data;
                            ClientAvatar ca = GetParent().GetLevel().GetHomeOwnerAvatar();
                            m_vTimer = new Timer();
                            int trainingTime = newcd.GetTrainingTime(ca.GetUnitUpgradeLevel(newcd));
                            m_vTimer.StartTimer(trainingTime, GetParent().GetLevel().GetTime());
                        }
                    }
                }
            }    
        }

        public override void Load(JObject jsonObject)
        {
            JObject unitProdObject = (JObject)jsonObject["unit_prod"];
            m_vIsSpellForge = (unitProdObject["unit_type"].ToObject<int>() == 1);
            var timeToken = unitProdObject["t"];
            if (timeToken != null)
            {
                m_vTimer = new Timer();
                int remainingTime = timeToken.ToObject<int>();
                m_vTimer.StartTimer(remainingTime, GetParent().GetLevel().GetTime());
            }
            JArray unitJsonArray = (JArray)unitProdObject["slots"];
            if (unitJsonArray != null)
            {
                foreach (JObject unitJsonObject in unitJsonArray)
                {
                    int id = unitJsonObject["id"].ToObject<int>();
                    int cnt = unitJsonObject["cnt"].ToObject<int>();
                    m_vUnits.Add(new DataSlot(ObjectManager.DataTables.GetDataById(id),cnt));
                }
            }
        }

        public override JObject Save(JObject jsonObject)
        {
            //{"data":1000006,"lvl":3,"x":12,"y":34,"unit_prod":{"unit_type":0,"t":0,"slots":[{"id":4000000,"cnt":19}]}}
            JObject unitProdObject = new JObject();
            if(m_vIsSpellForge)
                unitProdObject.Add("unit_type",1);
            else
                unitProdObject.Add("unit_type",0);

            if(m_vTimer != null)
            {
                unitProdObject.Add("t", m_vTimer.GetRemainingSeconds(GetParent().GetLevel().GetTime()));
            }

            if(GetSlotCount()>=1)
            {
                JArray unitJsonArray = new JArray();
                foreach(var unit in m_vUnits)
                {
                    JObject unitJsonObject = new JObject();
                    unitJsonObject.Add("id", unit.Data.GetGlobalID());
                    unitJsonObject.Add("cnt", unit.Value);
                    unitJsonArray.Add(unitJsonObject);
                }
                unitProdObject.Add("slots",unitJsonArray);
            }
            jsonObject.Add("unit_prod", unitProdObject);
            return jsonObject;
        }

        public void SetUnitType(GameObject go)
        {
            Building b = (Building)GetParent();
            BuildingData bd = b.GetBuildingData();
            m_vIsSpellForge = bd.IsSpellForge();
        }

        public void SpeedUp()
        {
            while(m_vUnits.Count >= 1 && ProductionCompleted())
            { }
        }

        public void StartProducingNextUnit()
        {
            m_vTimer = null;
            if (GetSlotCount() >= 1)
            {
                RemoveUnit((CombatItemData)m_vUnits[0].Data);
            }
        }

        public override void Tick()
        {
            if(m_vTimer != null)
            {
                if (m_vTimer.GetRemainingSeconds(GetParent().GetLevel().GetTime()) <= 0)
                {
                    ProductionCompleted();
                }
            }  
        }

        public override int Type
        {
            get { return 3; }
        }
    }
}
