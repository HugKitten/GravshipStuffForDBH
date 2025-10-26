namespace GravshipStuffForDubsBadHygiene
{
    public class CompProperties_AtmosphericInlet : CompProperties_PoweredWaterInlet
    {
        public CompProperties_AtmosphericInlet()
        {
            this.compClass = typeof(CompAtmosphericWaterInlet);
        }
    }
}