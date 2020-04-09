using System.Collections.Generic;
using System.Linq;
using GraveMod.BlockEntities;
using GraveMod.Util;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;

namespace GraveMod.Systems
{
    public class PlaceGraveOnDeath : ModSystem
    {
        public override bool ShouldLoad(EnumAppSide forSide) => true;
        
        private ICoreServerAPI _serverApi;
        private List<string> _inventoryPrefixes => GraveModConfig.Current.InventoryPrefixes;

        public override void StartServerSide(ICoreServerAPI api)
        {
            _serverApi = api;
            _serverApi.Event.PlayerJoin += OnPlayerJoin;
            _serverApi.Event.PlayerDeath += OnPlayerDeath;
        }

        private void OnPlayerJoin(IServerPlayer player)
        {
            var attributes = player.Entity.Properties.Server.Attributes;
            if (attributes == null)
            {
                attributes = new TreeAttribute();
                player.Entity.Properties.Server.Attributes = attributes;
            }
            attributes.SetBool("keepContents", true);
        }

        private void OnPlayerDeath(IServerPlayer player, DamageSource damageSource)
        {
            var placeForGrave = player.Entity.PositionBeforeFalling.AsBlockPos;
            _serverApi.World.BlockAccessor.BreakBlock(placeForGrave, player);
            var grave = _serverApi.World.GetBlock(new AssetLocation(Constants.DomainName,"grave-north"));
            _serverApi.World.BlockAccessor.SetBlock(grave.BlockId, placeForGrave);

            if (_serverApi.World.BlockAccessor.GetBlockEntity(placeForGrave) is GraveBlockEntity graveEntity)
            {
                var items = player.InventoryManager.Inventories
                    .Where(i => _inventoryPrefixes.Any(i.Key.StartsWith))
                    .SelectMany(i => i.Value.ToList())
                    .Where(i => !i.Empty)
                    .ToList();
                foreach (var itemSlot in items)
                {
                    graveEntity.Items.Add(itemSlot.TakeOutWhole());
                }
            }
        }
    }
}
