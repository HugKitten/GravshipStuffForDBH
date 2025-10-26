using DubsBadHygiene;
using Verse;

namespace GravshipStuffForDubsBadHygiene;

public class CompProperties_PoweredWaterStorage : CompProperties_WaterStorage
{
    public float WaterStorageCapPowered { get; set; }
    public float WaterStorageCapUnpowered { get; set; }
        
    public CompProperties_PoweredWaterStorage()
    {
        this.compClass = typeof(CompPoweredWaterStorage);
    }

    public override void PostLoadSpecial(ThingDef parent)
    {
        base.PostLoadSpecial(parent);
        
        // A bit of a hack
        this.WaterStorageCap = this.WaterStorageCapPowered;
    }
}