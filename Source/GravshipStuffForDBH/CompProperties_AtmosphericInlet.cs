namespace GravshipStuffForDubsBadHygiene
{
    public class CompProperties_AtmosphericInlet : CompProperties_TreatedWaterInlet
    {
        public CompProperties_AtmosphericInlet()
        {
            this.compClass = typeof(CompAtmosphericWaterInlet);
        }
    }
}