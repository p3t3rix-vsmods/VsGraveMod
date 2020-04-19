using System.Collections.Generic;

namespace GraveMod
{
    public class GraveModConfig 
    {
        public static GraveModConfig Current { get; set; }

        public bool ShouldCreateDeathWaypoint { get; set; } = true; // if a waypoint should be created
        public List<string> InventoryPrefixes { get; set; } = new List<string>() // inventories that should be transferred to the grave
        {
            "hotbar",
            "backpack"
        };

        public int GraveRadius { get; set; } = 1; // radius to search for valid blocks
    }
}
