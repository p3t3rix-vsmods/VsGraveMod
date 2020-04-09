using System;
using System.Collections.Generic;
using Foundation.Util;
using Vintagestory.API.Common;

namespace GraveMod.Util
{
    public class GraveModConfig 
    {
        public static GraveModConfig Current { get; set; }

        public bool ShouldCreateDeathWaypoint { get; set; } = true;
        public List<string> InventoryPrefixes { get; set; } = new List<string>()
        {
            "hotbar",
            "backpack"
        };
    }
}
