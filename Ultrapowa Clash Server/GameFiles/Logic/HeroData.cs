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
    class HeroData : CombatItemData
    {

        public HeroData(CSVRow row, DataTable dt)
            : base(row, dt)
        {
            LoadData(this, this.GetType(), row);
        }

        public override int GetCombatItemType()
        {
            return 2;
        }

        public int GetRequiredTownHallLevel(int level)
        {
            return RequiredTownHallLevel[level];
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
        public int Speed { get; set; }
        public List<int> Hitpoints { get; set; }
        public List<int> UpgradeTimeH { get; set; }
        public List<String> UpgradeResource { get; set; }
        public List<int> UpgradeCost { get; set; }
        public List<int> RequiredTownHallLevel { get; set; }
        public int AttackRange { get; set; }
        public int AttackSpeed { get; set; }
        public int Damage { get; set; }
        public int PreferedTargetDamageMod { get; set; }
        public int DamageRadius { get; set; }
        public String IconSWF { get; set; }
        public String IconExportName { get; set; }
        public String BigPicture { get; set; }
        public String BigPictureSWF { get; set; }
        public String SmallPicture { get; set; }
        public String SmallPictureSWF { get; set; }
        public String Projectile { get; set; }
        public String RageProjectile { get; set; }
        public String PreferedTargetBuilding { get; set; }
        public String DeployEffect { get; set; }
        public String AttackEffect { get; set; }
        public String HitEffect { get; set; }
        public bool IsFlying { get; set; }
        public bool AirTargets { get; set; }
        public bool GroundTargets { get; set; }
        public int AttackCount { get; set; }
        public String DieEffect { get; set; }
        public String Animation { get; set; }
        public int MaxSearchRadiusForDefender { get; set; }
        public int HousingSpace { get; set; }
        public String SpecialAbilityEffect { get; set; }
        public int RegenerationTimeMinutes { get; set; }
        public int TrainingTime { get; set; }
        public String TrainingResource { get; set; }
        public int TrainingCost { get; set; }
        public String CelebrateEffect { get; set; }
        public int SleepOffsetX { get; set; }
        public int SleepOffsetY { get; set; }
        public int PatrolRadius { get; set; }
        public String AbilityTriggerEffect { get; set; }
        public bool AbilityAffectsHero { get; set; }
        public String AbilityAffectsCharacter { get; set; }
        public int AbilityRadius { get; set; }
        public int AbilityTime { get; set; }
        public bool AbilityOnce { get; set; }
        public int AbilityCooldown { get; set; }
        public int AbilitySpeedBoost { get; set; }
        public int AbilitySpeedBoost2 { get; set; }
        public int AbilityDamageBoostPercent { get; set; }
        public String AbilitySummonTroop { get; set; }
        public int AbilitySummonTroopCount { get; set; }
        public bool AbilityStealth { get; set; }
        public int AbilityDamageBoostOffset { get; set; }
        public int AbilityHealthIncrease { get; set; }
        public String AbilityTID { get; set; }
        public String AbilityDescTID { get; set; }
        public String AbilityIcon { get; set; }
        public String AbilityBigPictureExportName { get; set; }
        public int AbilityDelay { get; set; }
        public int StrengthWeight { get; set; }
        public int StrengthWeight2 { get; set; }
        public int AlertRadius { get; set; }


    }
}
