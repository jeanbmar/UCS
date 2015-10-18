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
    class TrapData : ConstructionItemData
    {

        public TrapData(CSVRow row, DataTable dt)
            : base(row, dt)
        {
            LoadData(this, this.GetType(), row);
        }

        public override int GetBuildCost(int level)
        {
            return BuildCost[level];
        }

        public override ResourceData GetBuildResource(int level)
        {
            return ObjectManager.DataTables.GetResourceByName(BuildResource[level]);
        }

        public override int GetConstructionTime(int level)
        {
            return BuildTimeM[level] * 60 + BuildTimeH[level] * 60 * 60 + BuildTimeD[level] * 60 * 60 * 24;
        }

        public override int GetRequiredTownHallLevel(int level)
        {
            return TownHallLevel[level] - 1;//-1 à ajouter obligatoirement (checké il est retranché au moment de l'init client)
        }

        public int GetSellPrice(int level)
        {
            int calculation = (int)(((long)BuildCost[level] * 2 * (long)1717986919) >> 32);
            return ((calculation >> 2) + (calculation >> 31));
        }

        public override int GetUpgradeLevelCount()
        {
            return BuildCost.Count;
        }

        public String TID { get; set; }
        public String InfoTID { get; set; }
        public String SWF { get; set; }
        public String ExportName { get; set; }
        public String ExportNameBuildAnim { get; set; }
        public String ExportNameBroken { get; set; }
        public String BigPicture { get; set; }
        public String BigPictureSWF { get; set; }
        public String EffectBroken { get; set; }
        public List<int> Damage { get; set; }
        public int DamageRadius { get; set; }
        public int TriggerRadius { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public String Effect { get; set; }
        public String Effect2 { get; set; }
        public String DamageEffect { get; set; }
        public bool Passable { get; set; }
        public List<String> BuildResource { get; set; }
        public List<int> BuildTimeD { get; set; }
        public List<int> BuildTimeH { get; set; }
        public List<int> BuildTimeM { get; set; }
        public List<int> BuildCost { get; set; }
        public List<int> RearmCost { get; set; }
        public List<int> TownHallLevel { get; set; }
        public bool EjectVictims { get; set; }
        public int MinTriggerHousingLimit { get; set; }
        public int EjectHousingLimit { get; set; }
        public String ExportNameTriggered { get; set; }
        public int ActionFrame { get; set; }
        public String PickUpEffect { get; set; }
        public String PlacingEffect { get; set; }
        public String AppearEffect { get; set; }
        public int DurationMS { get; set; }
        public int SpeedMod { get; set; }
        public int DamageMod { get; set; }
        public bool AirTrigger { get; set; }
        public bool GroundTrigger { get; set; }
        public int HitDelayMS { get; set; }
        public int HitCnt { get; set; }
        public String Projectile { get; set; }
        public String Spell { get; set; }
        public int StrengthWeight { get; set; }
        public int PreferredTargetDamageMod { get; set; }
        public String PreferredTarget { get; set; }

    }
}
