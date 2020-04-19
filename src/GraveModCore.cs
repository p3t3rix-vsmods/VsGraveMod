using Foundation.Util.Extensions;
using GraveMod.BlockEntities;
using GraveMod.Blocks;
using Vintagestory.API.Common;

namespace GraveMod
{
    /// <summary>
    /// Startup class that handles things all mod systems in this mod need e.g. config loading
    /// </summary>
    class GraveModCore : ModSystem
    {
        public override bool ShouldLoad(EnumAppSide forSide) => true;

        public override void StartPre(ICoreAPI api)
        {
            GraveModConfig.Current = api.LoadOrCreateConfig<GraveModConfig>("GraveModConfig.json");
        }

        public override void Start(ICoreAPI api)
        {
            api.RegisterBlockClass(nameof(GraveBlock), typeof(GraveBlock));
            api.RegisterBlockEntityClass(nameof(GraveBlockEntity), typeof(GraveBlockEntity));
        }
    }
}
