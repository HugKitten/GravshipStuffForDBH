using System;
using DubsBadHygiene;
using GravshipStuffForDubsBadHygiene.HarmonyPatches;
using Verse;

namespace GravshipStuffForDubsBadHygiene
{
    public class CompAtmosphericPumpingStation : CompWaterPumpingStation
    {
        public virtual float YieldPowered { get; set; }
        public virtual float YieldUnpowered { get; set; }
        public override float Capacity => this.WorkingNow ? this.YieldPowered : this.YieldUnpowered;
        
        public new CompProperties_AtmosphericPumpingStation Props 
            => (CompProperties_AtmosphericPumpingStation)base.props ?? throw new InvalidOperationException();
        
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            var settings = LoadedModManager.GetMod<GravshipStuffForDubsBadHygieneMod>()
                .GetSettings<GravshipStuffForDubsBadHygieneSettings>();
            
            this.YieldPowered = settings.atmosphericYieldPowered;
            this.YieldUnpowered = settings.atmosphericYieldUnpowered;
            this.powerComp.PowerOutput = -settings.atmosphericPowerConsumption;
            
            base.PostSpawnSetup(respawningAfterLoad);
        }

        // The pumping station inspect string includes duplicate data that is already exposed by the water inlet.
        public override string CompInspectStringExtra() => null;
    }
}