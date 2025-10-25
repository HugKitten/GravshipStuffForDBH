using System;
using System.Collections.Generic;
using System.Linq;
using DubsBadHygiene;
using Verse;

namespace GravshipStuffForDubsBadHygiene
{
    public class CompProperties_WaterInletPumpingStation : CompProperties_WaterPumpingStation
    {
        public CompProperties_WaterInletPumpingStation() => 
            this.compClass = typeof(CompWaterInletPumpingStation);

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            foreach (var configError in base.ConfigErrors(parentDef))
                yield return configError;
            
            if (!parentDef.comps.OfType<CompProperties_WaterInlet>().Any())
                yield return "Can't use CompWaterInletPumpingStation without CompAtmosphericWaterInlet.";
        }
        
        public override void PostLoadSpecial(ThingDef parent)
        {
            base.PostLoadSpecial(parent);
        
            // A bit of a hack
            var comp = parent.comps.OfType<CompProperties_WaterInlet>().FirstOrDefault();
            if (comp is CompProperties_PoweredWaterInlet poweredWaterInletProps)
                this.Capacity = poweredWaterInletProps.CapacityPowered;
            else if (comp != null) 
                this.Capacity = comp.Capacity;
        }
    }
}