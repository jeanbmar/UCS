using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Configuration;
using Newtonsoft.Json;
using UCS.Core;
using UCS.PacketProcessing;
using UCS.GameFiles;

namespace UCS.Logic
{
    class Avatar
    {

        private int m_vCastleLevel;
        private int m_vCastleTotalCapacity;
        private int m_vCastleUsedCapacity;
        private int m_vTownHallLevel;//a1 + 88
        //private int m_vRemainingShieldTime;
        protected List<DataSlot> m_vResources;//a1 + 20
        protected List<DataSlot> m_vHeroHealth;//a1 + 8
        protected List<DataSlot> m_vHeroUpgradeLevel;//a1 + 12
        protected List<DataSlot> m_vHeroState;//a1 + 16
        protected List<DataSlot> m_vUnitCount;//a1 + 24
        protected List<DataSlot> m_vSpellCount;//a1 + 28
        protected List<DataSlot> m_vResourceCaps;//a1 + 36
        //protected List<DataSlot> m_vNpcStars { get; set; }//a1 + 56
        //protected List<DataSlot> m_vLootedNpcGold { get; set; }//a1 + 60
        //protected List<DataSlot> m_vLootedNpcElixir { get; set; }//a1 + 64
        protected List<DataSlot> m_vUnitUpgradeLevel;//a1 + 40
        protected List<DataSlot> m_vSpellUpgradeLevel;//a1 + 44


        public Avatar() 
        {
            this.m_vResources = new List<DataSlot>();
            this.m_vResourceCaps = new List<DataSlot>();
            this.m_vUnitCount = new List<DataSlot>();
            this.m_vUnitUpgradeLevel = new List<DataSlot>();
            this.m_vHeroHealth = new List<DataSlot>();
            this.m_vHeroUpgradeLevel = new List<DataSlot>();
            this.m_vHeroState = new List<DataSlot>();
            this.m_vSpellCount = new List<DataSlot>();
            this.m_vSpellUpgradeLevel = new List<DataSlot>();
        }

        public void CommodityCountChangeHelper(int commodityType, Data data, int count)
        {
            if(data.GetDataType() == 2)
            {
                if(commodityType == 0)
                {
                    int resourceCount = GetResourceCount((ResourceData)data);
                    int newResourceValue = Math.Max(resourceCount + count, 0);
                    if(count >= 1)
                    {
                        int resourceCap = GetResourceCap((ResourceData)data);
                        if (resourceCount < resourceCap)
                        {
                            if (newResourceValue > resourceCap)
                            {
                                newResourceValue = resourceCap;
                            }
                        }
                    }
                    SetResourceCount((ResourceData)data, newResourceValue);
                }
            }
        }

        public int GetAllianceCastleLevel()
        {
            return m_vCastleLevel;
        }

        public int GetAllianceCastleTotalCapacity()
        {
            return m_vCastleTotalCapacity;
        }

        public int GetAllianceCastleUsedCapacity()
        {
            return m_vCastleUsedCapacity;
        }

        public int GetDataIndex(List<DataSlot> dsl, Data d)
        {
            return dsl.FindIndex(ds => ds.Data == d);
        }

        public List<DataSlot> GetResourceCaps()
        {
            return m_vResourceCaps;
        }

        public List<DataSlot> GetResources()
        {
            return m_vResources;
        }

        public int GetResourceCap(ResourceData rd)
        {
            int index = GetDataIndex(m_vResourceCaps, rd);
            int count = 0;
            if (index != -1)
                count = m_vResourceCaps[index].Value;
            return count;
        }

        public int GetResourceCount(ResourceData rd)
        {
            int index = GetDataIndex(m_vResources, rd);
            int count = 0;
            if (index != -1)
                count = m_vResources[index].Value;
            return count;
        }

        public List<DataSlot> GetSpells()
        {
            return m_vSpellCount;
        }

        public int GetTownHallLevel()
        {
            return m_vTownHallLevel;
        }

        public List<DataSlot> GetUnits()
        {
            return m_vUnitCount;
        }

        public int GetUnitCount(CombatItemData cd)
        {
            int result = 0;
            if(cd.GetCombatItemType() == 1)
            {
                int index = GetDataIndex(m_vSpellCount, cd);
                if (index != -1)
                    result = m_vSpellCount[index].Value;
            }
            else
            {
                int index = GetDataIndex(m_vUnitCount, cd);
                if (index != -1)
                    result = m_vUnitCount[index].Value;
            }
            return result;
        }

        public int GetUnitUpgradeLevel(CombatItemData cd)
        {
            int result = 0; 
            switch(cd.GetCombatItemType())
            {
                case 2:
                    {
                        int index = GetDataIndex(m_vHeroUpgradeLevel, cd);
                        if (index != -1)
                            result = m_vHeroUpgradeLevel[index].Value;
                        break;
                    }
                case 1:
                    {
                        int index = GetDataIndex(m_vSpellUpgradeLevel, cd);
                        if (index != -1)
                            result = m_vSpellUpgradeLevel[index].Value;
                        break;
                    }
                    
                default:
                    {
                        int index = GetDataIndex(m_vUnitUpgradeLevel, cd);
                        if (index != -1)
                            result = m_vUnitUpgradeLevel[index].Value;
                        break;
                    }        
            }
            return result;
        }

        public int GetUnusedResourceCap(ResourceData rd)
        {
            int resourceCount = GetResourceCount((ResourceData)rd);
            int resourceCap = GetResourceCap((ResourceData)rd);
            return Math.Max(resourceCap - resourceCount, 0);
        }

        public void SetAllianceCastleLevel(int level)
        {
            m_vCastleLevel = level;
        }

        public void SetAllianceCastleTotalCapacity(int totalCapacity)
        {
            m_vCastleTotalCapacity = totalCapacity;
        }

        public void SetAllianceCastleUsedCapacity(int usedCapacity)
        {
            m_vCastleUsedCapacity = usedCapacity;
        }

        public void SetHeroHealth(HeroData hd, int health)
        {
            int index = GetDataIndex(m_vHeroHealth, hd);
            if (index == -1)
            {
                DataSlot ds = new DataSlot(hd, health);
                m_vHeroHealth.Add(ds);
            }
            else
            {
                m_vHeroHealth[index].Value = health;
            }
        }

        public void SetHeroState(HeroData hd, int state)
        {
            int index = GetDataIndex(m_vHeroState, hd);
            if (index == -1)
            {
                DataSlot ds = new DataSlot(hd, state);
                m_vHeroState.Add(ds);
            }
            else
            {
                m_vHeroState[index].Value = state;
            }
        }

        public void SetResourceCap(ResourceData rd, int value)
        {
            int index = GetDataIndex(m_vResourceCaps, rd);
            if(index == -1)
            {
                var ds = new DataSlot(rd, value);
                m_vResourceCaps.Add(ds);
            }
            else
            {
                m_vResourceCaps[index].Value = value;
            }
        }

        public void SetResourceCount(ResourceData rd, int value)
        {
            int index = GetDataIndex(m_vResources, rd);
            if (index == -1)
            {
                var ds = new DataSlot(rd, value);
                m_vResources.Add(ds);
            }
            else
            {
                m_vResources[index].Value = value;
            }
            //LogicLevel::getComponentManager(v18);
            //LogicComponentManager::divideAvatarResourcesToStorages(v19)
        }

        public void SetTownHallLevel(int level)
        {
            m_vTownHallLevel = level;
        }

        public void SetUnitCount(CombatItemData cd, int count)
        {
            switch (cd.GetCombatItemType())
            {
                case 1:
                    {
                        int index = GetDataIndex(m_vSpellCount, cd);
                        if (index != -1)
                            m_vSpellCount[index].Value = count;
                        else
                        {
                            DataSlot ds = new DataSlot(cd, count);
                            m_vSpellCount.Add(ds);
                        }
                        break;
                    }
                default:
                    {
                        int index = GetDataIndex(m_vUnitCount, cd);
                        if (index != -1)
                            m_vUnitCount[index].Value = count;
                        else
                        {
                            DataSlot ds = new DataSlot(cd, count);
                            m_vUnitCount.Add(ds);
                        }
                        break;
                    }
            }
        }

        public void SetUnitUpgradeLevel(CombatItemData cd, int level)
        {
            switch(cd.GetCombatItemType())
            {
                case 2:
                    {
                        int index = GetDataIndex(m_vHeroUpgradeLevel, cd);
                        if (index != -1)
                            m_vHeroUpgradeLevel[index].Value = level;
                        else
                        {
                            DataSlot ds = new DataSlot(cd, level);
                            m_vHeroUpgradeLevel.Add(ds);
                        }
                        break;
                    }
                case 1:
                    {
                        int index = GetDataIndex(m_vSpellUpgradeLevel, cd);
                        if (index != -1)
                            m_vSpellUpgradeLevel[index].Value = level;
                        else
                        {
                            DataSlot ds = new DataSlot(cd, level);
                            m_vSpellUpgradeLevel.Add(ds);
                        }
                        break;
                    }
                default:
                    {
                        int index = GetDataIndex(m_vUnitUpgradeLevel, cd);
                        if (index != -1)
                            m_vUnitUpgradeLevel[index].Value = level;
                        else
                        {
                            DataSlot ds = new DataSlot(cd, level);
                            m_vUnitUpgradeLevel.Add(ds);
                        }
                        break;
                    }       
            }
        }

    }
}
