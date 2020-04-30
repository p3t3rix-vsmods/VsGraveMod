using System;
using System.Collections.Generic;
using System.Linq;
using GraveMod.BlockEntities;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace GraveMod.Systems
{
    public class PlaceGraveOnDeath : ModSystem
    {
        private ICoreServerAPI _serverApi;
        private Block _graveBlock;
        private List<string> InventoryPrefixes => GraveModConfig.Current.InventoryPrefixes;

        public override double ExecuteOrder() => 2.0; //can load after everything else

        public override void StartServerSide(ICoreServerAPI api)
        {
            _serverApi = api;
            _serverApi.Event.PlayerJoin += OnPlayerJoin;
            _serverApi.Event.PlayerDeath += OnPlayerDeath;
            _graveBlock = _serverApi.World.GetBlock(new AssetLocation(Constants.DomainName, "grave-north"));
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
            var placeForGrave = DetermineGraveSpot(player);
            _serverApi.World.BlockAccessor.BreakBlock(placeForGrave, player);
            _serverApi.World.BlockAccessor.SetBlock(_graveBlock.BlockId, placeForGrave);

            if (_serverApi.World.BlockAccessor.GetBlockEntity(placeForGrave) is GraveBlockEntity graveEntity)
            {
                PlaceItemsInGrave(player, graveEntity);
            }
        }

        private void PlaceItemsInGrave(IServerPlayer player, GraveBlockEntity graveEntity)
        {
            var inventories = player.InventoryManager.Inventories
                .Where(i => InventoryPrefixes.Any(i.Key.StartsWith));

            var items = inventories
                .SelectMany(i => i.Value.ToList())
                .Where(i => !i.Empty)
                .OrderBy(i => i.StorageType == EnumItemStorageFlags.Backpack) // so backpacks get taken out last (otherwise you access items that are not there anymore)
                .ToList();
            foreach (var itemSlot in items)
            {
                graveEntity.Items.Add(itemSlot.TakeOutWhole());
            }
        }

        private BlockPos DetermineGraveSpot(IServerPlayer player)
        {
            var radius = GraveModConfig.Current.GraveRadius;
            var gravePos = player?.Entity?.PositionBeforeFalling?.AsBlockPos ?? player?.Entity?.Pos?.AsBlockPos;
            if (gravePos != null)
            {
                //first check the current position
                if (_serverApi.World.BlockAccessor.GetBlock(gravePos)?.IsReplacableBy(_graveBlock) == true)
                {
                    return gravePos;
                }
                //search surrounding blocks for a useful location
                _serverApi.World.BlockAccessor.SearchBlocks(
                    gravePos.AddCopy(-radius, -radius, -radius),
                    gravePos.AddCopy(radius, radius, radius),
                    (block, blockPos) =>
                    {
                        if (block != null && block.IsReplacableBy(_graveBlock))
                        {
                            gravePos = blockPos;
                            return false;
                        }
                        return true;
                    });
            }
            return gravePos; //this is either a valid position in the radius or the last position
        }
    }
}
