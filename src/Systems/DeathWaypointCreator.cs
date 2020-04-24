using System;
using System.Drawing;
using System.Linq;
using Foundation.Extensions;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace GraveMod.Systems
{
    public class DeathWaypointCreator : ModSystem
    {
        private WorldMapManager _mapManager;
        private const string WaypointIcon = "circle";

        public override bool ShouldLoad(EnumAppSide side) => side == EnumAppSide.Server;

        public override void StartServerSide(ICoreServerAPI api)
        {
            if (GraveModConfig.Current.ShouldCreateDeathWaypoint)
            {
                api.Event.PlayerDeath += OnPlayerDeath;
                _mapManager = api.ModLoader.GetModSystem<WorldMapManager>();
            }
        }

        private void OnPlayerDeath(IServerPlayer player, DamageSource damageSource)
        {
          _mapManager.AddWaypointToPlayer(CreateDeathWaypoint(player),player);
        }

        private static Waypoint CreateDeathWaypoint(IServerPlayer player)
        {
            var timeString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return new Waypoint()
            {
                Color = Color.Black.ToArgb(),
                Icon = WaypointIcon,
                Position = player.Entity.Pos.XYZ,
                OwningPlayerGroupId = player.Groups.FirstOrDefault()?.GroupUid ?? 0,
                OwningPlayerUid = player.Entity.PlayerUID,
                Title = $"{Lang.Get($"{Constants.DomainName}:waypoint_death")} -- ({timeString})"
            };
        }
    }
}
