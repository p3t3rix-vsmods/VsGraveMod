using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace GraveMod.Blocks
{
    public class GraveBlock : Block
    {
        /// <summary>
        /// This override is necessary because the HorizontalOrientable behaviour prevents the "drops" json-attribute from working 
        /// </summary>
        public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1)
        {
            return null;
        }

        /// <summary>
        /// This override is necessary because the HorizontalOrientable behaviour prevents the "drops" json-attribute from working 
        /// </summary>
        public override BlockDropItemStack[] GetDropsForHandbook(ItemStack handbookStack, IPlayer forPlayer)
        {
            return new BlockDropItemStack[0];
        }
    }
}
