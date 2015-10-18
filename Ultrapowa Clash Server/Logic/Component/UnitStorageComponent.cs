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
    class UnitStorageComponent : Component
    {
        private List<UnitSlot> m_vUnits;//a1 + 12;
        private int m_vMaxCapacity;//a1 + 16
        public bool IsSpellForge;//a1 + 20


        public UnitStorageComponent(GameObject go, int capacity) : base(go)
        {
            m_vUnits = new List<UnitSlot>();
            m_vMaxCapacity = capacity;
            SetStorageType(go);   
        }

        public void AddUnit(CombatItemData cd)

        {
            AddUnitImpl(cd, -1);
        }

        public void AddUnitImpl(CombatItemData cd, int level)
        {
            if(CanAddUnit(cd))
            {
                int unitIndex = GetUnitTypeIndex(cd, level);
                if(unitIndex == -1)
                {
                    UnitSlot us = new UnitSlot(cd, level, 1);
                    m_vUnits.Add(us);
                }
                else
                {
                    m_vUnits[unitIndex].Count++;
                }
                ClientAvatar ca = GetParent().GetLevel().GetPlayerAvatar();
                int unitCount = ca.GetUnitCount(cd);
                ca.SetUnitCount(cd, unitCount + 1);
            }
        }

        public bool CanAddUnit(CombatItemData cd)
        {
            bool result = false;
            if(cd != null)
            {
                if(IsSpellForge)
                {
                    result = (GetMaxCapacity() >= GetUsedCapacity() + cd.GetHousingSpace());
                }
                else
                {
                    var cm = GetParent().GetLevel().GetComponentManager();
                    int maxCapacity = cm.GetTotalMaxHousing();//GetMaxCapacity();
                    int usedCapacity = cm.GetTotalUsedHousing();//GetUsedCapacity();
                    int housingSpace = cd.GetHousingSpace();
                    if (GetUsedCapacity() < GetMaxCapacity())
                        result = (maxCapacity >= usedCapacity + housingSpace);
                } 
            }   
            return result;
        }

        public int GetMaxCapacity()
        {
            return m_vMaxCapacity;
        }

        public int GetUnitCount(int index)
        {
            return m_vUnits[index].Count;
        }

        public int GetUnitCountByData(CombatItemData cd)
        {
            int count = 0;
            for (int i = 0; i < m_vUnits.Count; i++)
            {
                if (m_vUnits[i].UnitData == cd)
                    count += m_vUnits[i].Count;
            }
            return count;
        }

        public int GetUnitLevel(int index)
        {
            return m_vUnits[index].Level;
        }

        public CombatItemData GetUnitType(int index)
        {
            return m_vUnits[index].UnitData;
        }

        public int GetUnitTypeIndex(CombatItemData cd)
        {
            int index = -1;
            for(int i=0;i<m_vUnits.Count;i++)
            {
                if (m_vUnits[i].UnitData == cd)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public int GetUnitTypeIndex(CombatItemData cd, int level)
        {
            int index = -1;
            for (int i = 0; i < m_vUnits.Count; i++)
            {
                if (m_vUnits[i].UnitData == cd)
                {
                    if(m_vUnits[i].Level == level)
                    {
                        index = i;
                        break;
                    } 
                }
            }
            return index;
        }

        public int GetUsedCapacity()
        {
            int count = 0;
            if (m_vUnits.Count >= 1)
            {
                for (int i = 0; i < m_vUnits.Count; i++)
                {
                    int cnt = m_vUnits[i].Count;
                    int housingSpace = m_vUnits[i].UnitData.GetHousingSpace();
                    count += cnt * housingSpace;
                }
            }
            return count;
        }

        public void RemoveUnits(CombatItemData cd, int count)
        {
            RemoveUnitsImpl(cd,-1,count);
        }

        public void RemoveUnitsImpl(CombatItemData cd, int level, int count)
        {
            int unitIndex = GetUnitTypeIndex(cd, level);
            if (unitIndex == -1)
            {
                //Do nothing, should be empty yet
            }
            else
            {
                UnitSlot us = m_vUnits[unitIndex];
                if(us.Count <= count)
                {
                    m_vUnits.Remove(us);
                }
                else
                {
                    us.Count -= count;
                }
                ClientAvatar ca = GetParent().GetLevel().GetPlayerAvatar();
                int unitCount = ca.GetUnitCount(cd);
                ca.SetUnitCount(cd, unitCount - count);
            }
        }

        public override void Load(JObject jsonObject)
        {
            IsSpellForge = (jsonObject["storage_type"].ToObject<int>() == 1);
            JArray unitArray = (JArray)jsonObject["units"];
            if (unitArray != null)
            {
                if(unitArray.Count > 0)
                {
                    foreach (JArray unitSlotArray in unitArray)
                    {
                        int id = unitSlotArray[0].ToObject<int>();
                        int cnt = unitSlotArray[1].ToObject<int>();
                        m_vUnits.Add(new UnitSlot((CombatItemData)ObjectManager.DataTables.GetDataById(id), -1, cnt));
                    }
                }  
            }
        }

        public override JObject Save(JObject jsonObject)
        {
            //{"data":1000000,"lvl":7,"x":10,"y":35,"units":[[4000005,11],[4000000,1],[4000001,1]],"storage_type":0,"l1x":10,"l1y":35}

            JArray unitJsonArray = new JArray();
            if(m_vUnits.Count > 0)
            { 
                foreach (var unit in m_vUnits)
                {
                    JArray unitSlotJsonArray = new JArray();
                    unitSlotJsonArray.Add(unit.UnitData.GetGlobalID());
                    unitSlotJsonArray.Add(unit.Count);
                    unitJsonArray.Add(unitSlotJsonArray);
                }
            }
            jsonObject.Add("units", unitJsonArray);
            
            if (IsSpellForge)
                jsonObject.Add("storage_type", 1);
            else
                jsonObject.Add("storage_type", 0);

            return jsonObject;
        }

        public void SetMaxCapacity(int capacity)
        {
            m_vMaxCapacity = capacity;
        }

        public void SetStorageType(GameObject go)
        {
            Building b = (Building)GetParent();
            BuildingData bd = b.GetBuildingData();
            IsSpellForge = bd.IsSpellForge();
        }

        public override int Type
        {
            get { return 0; }
        }
    }
}
