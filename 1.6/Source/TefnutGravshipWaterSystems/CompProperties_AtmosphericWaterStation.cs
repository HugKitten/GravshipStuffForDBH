using DubsBadHygiene;

namespace GravshipStuffForDubsBadHygiene
{
    public class CompProperties_AtmosphericWaterStation : CompProperties_WaterPumpingStation
    {
        public CompProperties_AtmosphericWaterStation() => this.compClass = typeof (CompAtmosphericWaterStation);
    }
}