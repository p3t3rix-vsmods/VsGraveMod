using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace GraveMod.BlockEntities
{
    class GraveBlockEntity : BlockEntity
    {
        public List<ItemStack> Items { get; set; } = new List<ItemStack>();

        public override void OnBlockBroken()
        {
            foreach (var item in Items)
            {
                Api.World.SpawnItemEntity(item, Pos.ToVec3d(),new Vec3d(0,0.1,0));
            }
        }

        public override void ToTreeAttributes(ITreeAttribute tree)
        {
            base.ToTreeAttributes(tree);
            var attr = tree.GetOrAddTreeAttribute("items");
            var i = 0;
            foreach (var itemStack in Items)
            {
                attr.SetItemstack($"IS_{i}", itemStack);
                i++;
            }
        }

        public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
        {
            base.FromTreeAttributes(tree, worldAccessForResolve);
            var items = tree.GetOrAddTreeAttribute("items");
            foreach (var saveItems in items)
            {
                Items.Add(items.GetItemstack(saveItems.Key));
            }
        }
    }
}