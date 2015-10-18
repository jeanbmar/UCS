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
    class CharacterData : CombatItemData
    {

        public CharacterData(CSVRow row, DataTable dt)
            : base(row, dt)
        {
            LoadData(this, this.GetType(), row);
        }

        public override int GetHousingSpace()
        {
            return HousingSpace;
        }

        public override int GetCombatItemType()
        {
            return 0;
        }

        public override int GetRequiredLaboratoryLevel(int level)
        {
            return LaboratoryLevel[level];
        }

        public override int GetRequiredProductionHouseLevel()
        {
            return BarrackLevel;
        }

        public override int GetTrainingTime(int level)
        {
            return TrainingTime[level];
        }

        public override int GetTrainingCost(int level)
        {
            return TrainingCost[level];
        }

        public override ResourceData GetTrainingResource()
        {
            return ObjectManager.DataTables.GetResourceByName(TrainingResource);
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
        public String SWF { get; set; }
        public int HousingSpace { get; set; }
        public int BarrackLevel { get; set; }
        public List<int> LaboratoryLevel { get; set; }
        public int Speed { get; set; }
        public int Hitpoints { get; set; }
        public List<int> TrainingTime { get; set; }
        public String TrainingResource { get; set; }
        public List<int> TrainingCost { get; set; }
        public List<int> UpgradeTimeH { get; set; }
        public List<String> UpgradeResource { get; set; }
        public List<int> UpgradeCost { get; set; }
        public int AttackRange { get; set; }
        public int AttackSpeed { get; set; }
        public int Damage { get; set; }
        public int PreferedTargetDamageMod { get; set; }
        public int DamageRadius { get; set; }
        public bool AreaDamageIgnoresWalls { get; set; }
        public bool SelfAsAoeCenter { get; set; }
        public String IconSWF { get; set; }
        public String IconExportName { get; set; }
        public String BigPicture { get; set; }
        public String BigPictureSWF { get; set; }
        public String Projectile { get; set; }
        public String PreferedTargetBuilding { get; set; }
        public String PreferedTargetBuildingClass { get; set; }
        public String DeployEffect { get; set; }
        public String AttackEffect { get; set; }
        public String HitEffect { get; set; }
        public bool IsFlying { get; set; }
        public bool AirTargets { get; set; }
        public bool GroundTargets { get; set; }
        public int AttackCount { get; set; }
        public String DieEffect { get; set; }
        public String Animation { get; set; }
        public int UnitOfType { get; set; }
        public bool IsJumper { get; set; }
        public int MovementOffsetAmount { get; set; }
        public int MovementOffsetSpeed { get; set; }
        public String TombStone { get; set; }
        public int DieDamage { get; set; }
        public int DieDamageRadius { get; set; }
        public String DieDamageEffect { get; set; }
        public int DieDamageDelay { get; set; }
        public bool DisableProduction { get; set; }
        public String SecondaryTroop { get; set; }
        public bool IsSecondaryTroop { get; set; }
        public int SecondaryTroopCnt { get; set; }
        public int SecondarySpawnDist { get; set; }
        public bool RandomizeSecSpawnDist { get; set; }
        public bool PickNewTargetAfterPushback { get; set; }
        public int PushbackSpeed { get; set; }
        public String SummonTroop { get; set; }
        public int SummonTroopCount { get; set; }
        public int SummonCooldown { get; set; }
        public String SummonEffect { get; set; }
        public int SummonLimit { get; set; }
        public int SpawnIdle { get; set; }
        public int StrengthWeight { get; set; }
        public String ChildTroop { get; set; }
        public int ChildTroopCount { get; set; }
        public int SpeedDecreasePerChildTroopLost { get; set; }
        public int ChildTroop0_X { get; set; }
        public int ChildTroop0_Y { get; set; }
        public int ChildTroop1_X { get; set; }
        public int ChildTroop1_Y { get; set; }
        public int ChildTroop2_X { get; set; }
        public int ChildTroop2_Y { get; set; }
        public bool AttackMultipleBuildings { get; set; }
        public bool IncreasingDamage { get; set; }
        public int DamageLv2 { get; set; }
        public int DamageLv3 { get; set; }
        public int DamageLv4 { get; set; }
        public int Lv2SwitchHits { get; set; }
        public int Lv3SwitchHits { get; set; }
        public int Lv4SwitchHits { get; set; }
        public int AttackSpeedLv2 { get; set; }
        public int AttackSpeedLv3 { get; set; }
        public int AttackSpeedLv4 { get; set; }
        public String AttackEffectLv2 { get; set; }
        public String AttackEffectLv3 { get; set; }
        public String AttackEffectLv4 { get; set; }
        public String TransitionEffectLv2 { get; set; }
        public String TransitionEffectLv3 { get; set; }
        public String TransitionEffectLv4 { get; set; }
        public int HitEffectOffset { get; set; }
        public int TargetedEffectOffset { get; set; }
        public int SecondarySpawnOffset { get; set; }

    }
}
