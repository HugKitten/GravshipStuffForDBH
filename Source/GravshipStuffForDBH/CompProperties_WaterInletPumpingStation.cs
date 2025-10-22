using DubsBadHygiene;

namespace GravshipStuffForDubsBadHygiene
{
    public class CompProperties_WaterInletPumpingStation : CompProperties_WaterPumpingStation
    {
        public CompProperties_WaterInletPumpingStation()
        {
            this.compClass = typeof(CompWaterInletPumpingStation);
            base.Capacity = float.NaN;
        }
    }
}