using System.Linq;
using DubsBadHygiene;

namespace GravshipStuffForDubsBadHygiene
{
    public static class PlumbingNetHelper
    {
        public static bool IsTreated(this PlumbingNet plumbingNet) =>
            plumbingNet.HasFilter ||
            plumbingNet.WaterWells.All(w => w is CompTreatedWaterInlet { EnablesContaminationIncident: false, RequiresFilter: false });

        public static bool RequiresFilter(this CompBaseWell comp) =>
            comp is CompTreatedWaterInlet treated
                ? treated.RequiresFilter
                : comp.props is CompProperties_WaterInlet { Deep: false };

        public static bool EnablesContaminationIncident(this CompBaseWell comp) => 
            comp is not CompTreatedWaterInlet treated || treated.EnablesContaminationIncident;

        public static bool RequiresFilter(this PlumbingNet plumbingNet) =>
            !plumbingNet.HasFilter &&
            plumbingNet.WaterWells.Any(RequiresFilter);

        public static bool EnablesContaminationIncident(this PlumbingNet plumbingNet) =>
            !plumbingNet.HasFilter ||
            plumbingNet.WaterWells.Any(EnablesContaminationIncident);
    }
}