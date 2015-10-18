using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using UCS.Logic;
using UCS.Helpers;
using UCS.GameFiles;
using UCS.Core;

namespace UCS.PacketProcessing
{
    //Commande 0x226
    class RemoveUnitsCommand : Command
    {
        public RemoveUnitsCommand(BinaryReader br)
        {
            Unknown1 = br.ReadUInt32WithEndian();
            UnitTypesCount = br.ReadInt32WithEndian();

            UnitsToRemove = new List<UnitToRemove>();
            for (int i = 0; i < UnitTypesCount; i++)
            {
                CharacterData unit = (CharacterData)br.ReadDataReference();
                int count = br.ReadInt32WithEndian();
                int level = br.ReadInt32WithEndian();
                UnitsToRemove.Add(new UnitToRemove() { Data = unit, Count = count, Level = level });
            }

            Unknown2 = br.ReadUInt32WithEndian();
        }

        //00 00 17 D6 24 B8 F6 5B 00 00 00 01 
        //00 00 02 26 00 00 00 00 00 00 00 02 00 3D 09 00 00 00 00 03 00 00 00 01 00 3D 09 08 00 00 00 02 00 00 00 03 00 00 17 98

        public uint Unknown1 { get; set; }
        public int UnitTypesCount { get; set; }
        public List<UnitToRemove> UnitsToRemove { get; set; }
        public uint Unknown2 { get; set; }

        public override void Execute(Level level)
        {
            foreach(var unit in UnitsToRemove)
            {
                List<Component> components = level.GetComponentManager().GetComponents(0);
                for (int i = 0; i < components.Count; i++)
                {
                    UnitStorageComponent c = (UnitStorageComponent)components[i];
                    if (c.GetUnitTypeIndex(unit.Data) != -1)
                    {
                        var storageCount = c.GetUnitCountByData(unit.Data);
                        if(storageCount >= unit.Count)
                        {
                            c.RemoveUnits(unit.Data, unit.Count);
                            break;
                        }
                        else
                        {
                            c.RemoveUnits(unit.Data, storageCount);
                            unit.Count -= storageCount;
                        }
                    }
                }
            }
        }
    }

    class UnitToRemove
    {
        public UnitToRemove() { }
        public CharacterData Data { get; set; }
        public int Count { get; set; }
        public int Level { get; set; }
    }
}
