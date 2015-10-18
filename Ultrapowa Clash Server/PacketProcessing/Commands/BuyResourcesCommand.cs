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
    //Commande 0x206
    class BuyResourcesCommand : Command
    {
        private int m_vResourceId;
        private int m_vResourceCount;
        private byte m_vIsCommandEmbedded;
        private object m_vCommand;

        public BuyResourcesCommand(BinaryReader br)
        {
            m_vResourceId = br.ReadInt32WithEndian();
            m_vResourceCount = br.ReadInt32WithEndian();
            m_vIsCommandEmbedded = br.ReadByte();
            if (m_vIsCommandEmbedded >= 0x01)
            {
                m_vCommand = CommandFactory.Read(br);
            }
            br.ReadInt32WithEndian();//Unknown1 
        }

        public override void Execute(Level level)
        {
            ResourceData rd = (ResourceData)ObjectManager.DataTables.GetDataById(m_vResourceId);
            if(rd != null)
            {
                if (m_vResourceCount >= 1)
                {
                    if (!rd.PremiumCurrency)
                    {
                        var avatar = level.GetPlayerAvatar();
                        int diamondCost = GamePlayUtil.GetResourceDiamondCost(m_vResourceCount, rd);
                        int unusedResourceCap = avatar.GetUnusedResourceCap(rd);
                        if(m_vResourceCount <= unusedResourceCap)
                        {
                            if(avatar.HasEnoughDiamonds(diamondCost))
                            {
                                avatar.UseDiamonds(diamondCost);
                                avatar.CommodityCountChangeHelper(0, rd, m_vResourceCount);
                                if(m_vIsCommandEmbedded >= 1)
                                    ((Command)m_vCommand).Execute(level);
                            }
                        }
                    }
                }   
            }
        }
    }
}
