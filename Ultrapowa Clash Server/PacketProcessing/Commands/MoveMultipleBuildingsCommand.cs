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
    //Commande 0x215
    class MoveMultipleBuildingsCommand : Command
    {
        private List<BuildingToMove> m_vBuildingsToMove;

        public MoveMultipleBuildingsCommand(BinaryReader br)
        {
            m_vBuildingsToMove = new List<BuildingToMove>();
            int buildingCount = br.ReadInt32WithEndian();
            for (int i = 0; i < buildingCount; i++)
            {
                var buildingToMove = new BuildingToMove();
                buildingToMove.X = br.ReadInt32WithEndian();
                buildingToMove.Y = br.ReadInt32WithEndian();
                buildingToMove.GameObjectId = br.ReadInt32WithEndian();
                m_vBuildingsToMove.Add(buildingToMove);
            }
            br.ReadInt32WithEndian();
        }

        //00 00 02 15 00 00 00 09 00 00 00 1C 00 00 00 11 1D CD 65 07 00 00 00 1D 00 00 00 11 1D CD 65 06 00 00 00 1B 00 00 00 11 1D CD 65 08 00 00 00 1E 00 00 00 11 1D CD 65 05 00 00 00 1A 00 00 00 11 1D CD 65 09 00 00 00 1F 00 00 00 11 1D CD 65 04 00 00 00 19 00 00 00 11 1D CD 65 0A 00 00 00 20 00 00 00 11 1D CD 65 03 00 00 00 18 00 00 00 11 1D CD 65 0B 00 00 12 FC

        public override void Execute(Level level)
        {
            foreach (var buildingToMove in m_vBuildingsToMove)
            {
                GameObject go = level.GameObjectManager.GetGameObjectByID(buildingToMove.GameObjectId);
                go.SetPositionXY(buildingToMove.X, buildingToMove.Y);
            }
        }
    }

    class BuildingToMove
    {
        public BuildingToMove() { }
        public int GameObjectId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
