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
    class BuildingData : ConstructionItemData
    {

        public BuildingData(CSVRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, this.GetType(), row);
        }

        public string GetBuildingClass()
        {
            return BuildingClass;
        }

        public override int GetBuildCost(int level)
        {
            return BuildCost[level];
        }

        public override ResourceData GetBuildResource(int level)
        {
            return ObjectManager.DataTables.GetResourceByName(BuildResource[level]);
        }

        public ResourceData GetAltBuildResource(int level)
        {
            return ObjectManager.DataTables.GetResourceByName(this.AltBuildResource[level]);
        }

        public override int GetConstructionTime(int level)
        {
            return BuildTimeS[level] + BuildTimeM[level] * 60 + BuildTimeH[level] * 60 * 60 + BuildTimeD[level] * 60 * 60 * 24;
        }

        public List<int> GetMaxStoredResourceCounts(int level)
        {
            var maxStoredResourceCounts = new List<int>();
            var resourceDataTable = ObjectManager.DataTables.GetTable(2);
            for(int i=0;i<resourceDataTable.GetItemCount();i++)
            {
                int value = 0;
                var resourceData = (ResourceData)resourceDataTable.GetItemAt(i);
                string propertyName = "MaxStored" + resourceData.GetName();
                if (this.GetType().GetProperty(propertyName) != null)
                {
                    object obj = this.GetType().GetProperty(propertyName).GetValue(this, null);
                    value = ((List<int>)obj)[level];
                }
                maxStoredResourceCounts.Add(value);
            }
            return maxStoredResourceCounts;
        }

        public override int GetRequiredTownHallLevel(int level)
        {
            return TownHallLevel[level] - 1;//-1 à ajouter obligatoirement (checké il est retranché au moment de l'init client)
        }

        public int GetUnitProduction(int level)
        {
            return UnitProduction[level];
        }

        public int GetUnitStorageCapacity(int level)
        {
            return HousingSpace[level];
        }

        public override int GetUpgradeLevelCount()
        {
            return BuildCost.Count;
        }

        public bool IsSpellForge()
        {
            return this.ForgesSpells;
        }

        public override bool IsTownHall()
        {
            return BuildingClass == "Town Hall";
        }

        public bool IsWorkerBuilding()
        {
            return BuildingClass == "Worker";
        }

        public String TID { get; set; }
        public String InfoTID { get; set; }
        public String TID_Instructor { get; set; }
        public String BuildingClass { get; set; }
        public String SWF { get; set; }
        public String ExportName { get; set; }
        public String ExportNameNpc { get; set; }
        public String ExportNameConstruction { get; set; }
        public List<int> BuildTimeD { get; set; }
        public List<int> BuildTimeH { get; set; }
        public List<int> BuildTimeM { get; set; }
        public List<int> BuildTimeS { get; set; }
        public List<String> BuildResource { get; set; }
        public List<int> BuildCost { get; set; }
        public List<int> TownHallLevel { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public String Icon { get; set; }
        public String ExportNameBuildAnim { get; set; }
        public List<int> MaxStoredGold { get; set; }
        public List<int> MaxStoredElixir { get; set; }
        public List<int> MaxStoredDarkElixir { get; set; }
        public List<int> MaxStoredWarGold { get; set; }
        public List<int> MaxStoredWarElixir { get; set; }
        public List<int> MaxStoredWarDarkElixir { get; set; }
        public bool Bunker { get; set; }
        public List<int> HousingSpace { get; set; }
        public String ProducesResource { get; set; }
        public List<int> ResourcePerHour { get; set; }
        public List<int> ResourceMax { get; set; }
        public List<int> ResourceIconLimit { get; set; }
        public List<int> UnitProduction { get; set; }
        public bool UpgradesUnits { get; set; }
        public List<int> ProducesUnitsOfType { get; set; }
        public List<int> BoostCost { get; set; }
        public List<int> Hitpoints { get; set; }
        public List<int> RegenTime { get; set; }
        public int AttackRange { get; set; }
        public bool AltAttackMode { get; set; }
        public int AltAttackRange { get; set; }
        public int AttackSpeed { get; set; }
        public List<int> Damage { get; set; }
        public String PreferredTarget { get; set; }
        public int PreferredTargetDamageMod { get; set; }
        public bool RandomHitPosition { get; set; }
        public String DestroyEffect { get; set; }
        public List<String> AttackEffect { get; set; }
        public List<String> AttackEffect2 { get; set; }
        public List<String> HitEffect { get; set; }
        public List<String> Projectile { get; set; }
        public String ExportNameDamaged { get; set; }
        public int BuildingW { get; set; }
        public int BuildingH { get; set; }
        public String ExportNameBase { get; set; }
        public String ExportNameBaseNpc { get; set; }
        public String ExportNameBaseWar { get; set; }
        public bool AirTargets { get; set; }
        public bool GroundTargets { get; set; }
        public bool AltAirTargets { get; set; }
        public bool AltGroundTargets { get; set; }
        public bool AltMultiTargets { get; set; }
        public int AmmoCount { get; set; }
        public String AmmoResource { get; set; }
        public int AmmoCost { get; set; }
        public int MinAttackRange { get; set; }
        public int DamageRadius { get; set; }
        public bool PushBack { get; set; }
        public bool WallCornerPieces { get; set; }
        public String LoadAmmoEffect { get; set; }
        public String NoAmmoEffect { get; set; }
        public String ToggleAttackModeEffect { get; set; }
        public String PickUpEffect { get; set; }
        public String PlacingEffect { get; set; }
        public bool CanNotSellLast { get; set; }
        public String DefenderCharacter { get; set; }
        public int DefenderCount { get; set; }
        public int DefenderZ { get; set; }
        public List<int> DestructionXP { get; set; }
        public Boolean Locked { get; set; }
        public Boolean Hidden { get; set; }
        public String AOESpell { get; set; }
        public String AOESpellAlternate { get; set; }
        public int TriggerRadius { get; set; }
        public String ExportNameTriggered { get; set; }
        public String AppearEffect { get; set; }
        public bool ForgesSpells { get; set; }
        public bool IsHeroBarrack { get; set; }
        public String HeroType { get; set; }
        public Boolean IncreasingDamage { get; set; }
        public int DamageLv2 { get; set; }
        public int DamageLv3 { get; set; }
        public int DamageMulti { get; set; }
        public int Lv2SwitchTime { get; set; }
        public int Lv3SwitchTime { get; set; }
        public String AttackEffectLv2 { get; set; }
        public String AttackEffectLv3 { get; set; }
        public String TransitionEffectLv2 { get; set; }
        public String TransitionEffectLv3 { get; set; }
        public int AltNumMultiTargets { get; set; }
        public bool PreventsHealing { get; set; }
        public int StrengthWeight { get; set; }
        public int AlternatePickNewTargetDelay { get; set; }
        public List<String> AltBuildResource { get; set; }
     
    }
}
