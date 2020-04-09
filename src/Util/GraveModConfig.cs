using System;
using System.Collections.Generic;
using Vintagestory.API.Common;

namespace GraveMod.Util
{
    //TODO: extract to abstract class
    public class GraveModConfig
    {
        private const string Filename = "GraveModConfig.json";
        public static GraveModConfig Current { get; private set; } = new GraveModConfig();

        public bool ShouldCreateDeathWaypoint { get; set; } = true;
        public List<string> InventoryPrefixes { get; set; } = new List<string>()
        {
            "hotbar",
            "backpack"
        };

        public static void Load(ICoreAPI api)
        {
            try
            {
                var loadedConfig = api.LoadModConfig<GraveModConfig>(Filename);
                if (loadedConfig != null)
                {
                    Current = loadedConfig;
                }
            }
            catch (Exception e)
            {
                api.World.Logger.Error($"Failed loading {Filename}, error {e}. Will initialize new one");
                api.StoreModConfig(new GraveModConfig(), Filename);
            }
            api.StoreModConfig(Current, Filename);
        }

    }
}
