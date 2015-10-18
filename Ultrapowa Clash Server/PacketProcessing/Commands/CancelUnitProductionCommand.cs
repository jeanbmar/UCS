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
    //Commande 0x1FD
    class CancelUnitProductionCommand : Command
    {
        public CancelUnitProductionCommand(BinaryReader br)
        {
            BuildingId = br.ReadInt32WithEndian(); //buildingId - 0x1DCD6500;
            Unknown1 = br.ReadUInt32WithEndian();
            UnitType = br.ReadInt32WithEndian();
            Count = br.ReadInt32WithEndian();
            Unknown3 = br.ReadUInt32WithEndian();
            Unknown4 = br.ReadUInt32WithEndian();
        }

        public int BuildingId { get; set; }
        public uint Unknown1 { get; set; } //00 00 00 00
        public int UnitType { get; set; } //00 3D 09 00
        public int Count { get; set; } //00 00 00 01
        public uint Unknown3 { get; set; } //00 00 00 00
        public uint Unknown4 { get; set; } //00 00 34 E4

        //00 00 01 FD 1D CD 65 05 00 00 00 00 00 3D 09 09 00 00 00 01 00 00 00 00 00 00 04 24 

        public override void Execute(Level level)
        {
            GameObject go = level.GameObjectManager.GetGameObjectByID(BuildingId);
            if (Count > 0)
            {
                Building b = (Building)go;
                UnitProductionComponent c = b.GetUnitProductionComponent();
                CombatItemData cd = (CombatItemData)ObjectManager.DataTables.GetDataById(UnitType);
                do
                {
                    //Ajouter gestion remboursement ressources
                    c.RemoveUnit(cd);
                    Count--;
                }
                while (Count > 0);
            }
        }
    }
}
