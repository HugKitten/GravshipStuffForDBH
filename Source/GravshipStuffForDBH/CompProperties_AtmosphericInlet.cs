using Verse;

namespace GravshipStuffForDubsBadHygiene
{
    public class CompProperties_AtmosphericInlet : CompProperties_TreatedWaterInlet
    {
        public float YieldPowered { get; set; }

        public float YieldUnpowered { get; set; }
        
        public CompProperties_AtmosphericInlet()
        {
            this.compClass = typeof(CompAtmosphericWaterInlet);
            
            var settings = LoadedModManager.GetMod<GravshipStuffForDubsBadHygieneMod>()
                .GetSettings<GravshipStuffForDubsBadHygieneSettings>();

            this.Capacity = settings.atmosphericYieldPowered;
            this.EnablesContaminationEvent = !settings.atmosphericIsTreated;
            this.RequiresFilter = !settings.atmosphericIsTreated;
            this.YieldPowered = settings.atmosphericYieldPowered;
            this.YieldUnpowered = settings.atmosphericYieldUnpowered;
        }
    }
}