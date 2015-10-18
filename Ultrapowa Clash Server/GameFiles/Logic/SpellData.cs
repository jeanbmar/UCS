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
    class SpellData : CombatItemData
    {

        public SpellData(CSVRow row, DataTable dt)
            : base(row, dt)
        {
            LoadData(this, this.GetType(), row);
        }

        public override int GetCombatItemType()
        {
            return 1;
        }

        public override int GetHousingSpace()
        {
            return HousingSpace;
        }

        public override int GetRequiredLaboratoryLevel(int level)
        {
            return LaboratoryLevel[level];
        }

        public override int GetRequiredProductionHouseLevel()
        {
            return SpellForgeLevel;
        }

        public override int GetTrainingCost(int level)
        {
            return TrainingCost[level];
        }

        public override ResourceData GetTrainingResource()
        {
            return ObjectManager.DataTables.GetResourceByName(TrainingResource);
        }

        public override int GetTrainingTime(int level)
        {
            return TrainingTime[level];
        }

        public override int GetUpgradeCost(int level)
        {
            return UpgradeCost[level];
        }

        public override int GetUpgradeLevelCount()
        {
            return UpgradeCost.Count;
        }

        public override ResourceData GetUpgradeResource(int level)
        {
            return ObjectManager.DataTables.GetResourceByName(UpgradeResource[level]);
        }

        public override int GetUpgradeTime(int level)
        {
            return UpgradeTimeH[level] * 3600;
        }

        public String TID { get; set; }
        public String InfoTID { get; set; }
        public bool DisableProduction { get; set; }
        public int SpellForgeLevel { get; set; }
        public List<int> LaboratoryLevel { get; set; }
        public String TrainingResource { get; set; }
        public List<int> TrainingCost { get; set; }
        public int HousingSpace { get; set; }
        public List<int> TrainingTime { get; set; }
        public int DeployTimeMS { get; set; }
        public int ChargingTimeMS { get; set; }
        public int HitTimeMS { get; set; }
        public List<int> UpgradeTimeH { get; set; }
        public List<String> UpgradeResource { get; set; }
        public List<int> UpgradeCost { get; set; }
        public int BoostTimeMS { get; set; }
        public int SpeedBoost { get; set; }
        public int SpeedBoost2 { get; set; }
        public int JumpHousingLimit { get; set; }
        public int JumpBoostMS { get; set; }
        public int DamageBoostPercent { get; set; }
        public int BuildingDamageBoostPercent { get; set; }
        public int Damage { get; set; }
        public int Radius { get; set; }
        public int NumberOfHits { get; set; }
        public int RandomRadius { get; set; }
        public int TimeBetweenHitsMS { get; set; }
        public String IconSWF { get; set; }
        public String IconExportName { get; set; }
        public String BigPicture { get; set; }
        public String PreDeployEffect { get; set; }
        public String DeployEffect { get; set; }
        public int DeployEffect2Delay { get; set; }
        public String DeployEffect2 { get; set; }
        public String ChargingEffect { get; set; }
        public String HitEffect { get; set; }
        public bool RandomRadiusAffectsOnlyGfx { get; set; }
        public int FreezeTimeMS { get; set; }
        public String SpawnObstacle { get; set; }
        public int NumObstacles { get; set; }
        public int StrengthWeight { get; set; }


    }
}
