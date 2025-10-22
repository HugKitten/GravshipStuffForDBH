using DubsBadHygiene;
using Verse;

namespace GravshipStuffForDubsBadHygiene
{
    public class CompProperties_CompressedWaterStorage : CompProperties_WaterStorage
    {
        public float WaterStorageCapPowered { get; set; }

        public float WaterStorageCapUnpowered
        {
            get => base.WaterStorageCap;
            set => base.WaterStorageCap = value;
        }
        
        public CompProperties_CompressedWaterStorage()
        {
            this.compClass = typeof(CompCompressedWaterStorage);
            
            var settings = LoadedModManager.GetMod<GravshipStuffForDubsBadHygieneMod>()
                .GetSettings<GravshipStuffForDubsBadHygieneSettings>();
            
            this.WaterStorageCapPowered = settings.tankStorageCapPowered;
            this.WaterStorageCapUnpowered = settings.tankStorageCapUnpowered;
            base.WaterStorageCap = this.WaterStorageCapPowered;
        }
    }
}