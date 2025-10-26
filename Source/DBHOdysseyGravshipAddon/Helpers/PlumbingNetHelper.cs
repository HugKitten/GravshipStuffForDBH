using System.Linq;
using DubsBadHygiene;

namespace DBHOdysseyGravshipAddon
{
    public static class PlumbingNetHelper
    {
        /// <summary>
        /// If all water sources are treated or there is a filter to filter the bad water
        /// </summary>
        public static bool IsTreated(this PlumbingNet plumbingNet) =>
            plumbingNet.HasFilter ||
            plumbingNet.WaterWells.All(w => w is CompTreatedWaterInlet { EnablesContaminationIncident: false, RequiresFilter: false });
        
        /// <summary>
        /// More intuitive way to push water
        /// </summary>
        /// <param name="plumbingNet">net to push to</param>
        /// <param name="max">Max amount of water</param>
        /// <returns>Total amount pushed</returns>
        public static float BetterPushWater(this PlumbingNet plumbingNet, float max = float.MaxValue) => 
            max - plumbingNet.PushWater(max);
    }
}