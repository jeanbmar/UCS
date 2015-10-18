using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using UCS.Core;

namespace UCS.GameFiles
{
    class CombatItemData : Data
    {

        public CombatItemData(CSVRow row, DataTable dt)
            : base(row, dt)
        {
        }

        public virtual int GetCombatItemType()
        {
            return -1;
        }

        public virtual int GetHousingSpace()
        {
            return -1;
        }

        public virtual int GetRequiredLaboratoryLevel(int level)
        {
            return -1;
        }

        public virtual int GetRequiredProductionHouseLevel()
        {
            return -1;
        }

        public virtual ResourceData GetTrainingResource()
        {
            return null;
        }

        public virtual int GetTrainingCost(int level)
        {
            return -1;
        }

        public virtual int GetTrainingTime(int level)
        {
            return -1;
        }

        public virtual int GetUpgradeCost(int level)
        {
            return -1;
        }

        public virtual int GetUpgradeLevelCount()
        {
            return -1;
        }

        public virtual ResourceData GetUpgradeResource(int level)
        {
            return null;
        }

        public virtual int GetUpgradeTime(int level)
        {
            return -1;
        }

    }
}
