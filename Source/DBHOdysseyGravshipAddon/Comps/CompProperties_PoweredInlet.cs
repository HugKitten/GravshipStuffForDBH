using System.Collections.Generic;
using Verse;

namespace DBHOdysseyGravshipAddon;

public class CompProperties_PoweredWaterInlet : CompProperties_TreatedWaterInlet
{
    public float CapacityPowered { get; set; }

    public float CapacityUnpowered { get; set; }
        
    public CompProperties_PoweredWaterInlet()
    {
        this.compClass = typeof(CompPoweredWaterInlet);
    }
    
    public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
    {
        foreach (var configError in base.ConfigErrors(parentDef))
            yield return configError;
    }
    
    public override void PostLoadSpecial(ThingDef parent)
    {
        base.PostLoadSpecial(parent);
        
        // A bit of a hack
        this.Capacity = this.CapacityPowered;
    }
}