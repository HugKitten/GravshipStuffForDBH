using Verse;

namespace GravshipStuffForDubsBadHygiene
{
    public class CompProperties_CompressedWaterStorage : CompProperties_PoweredWaterStorage
    {
        public CompProperties_CompressedWaterStorage()
        {
            this.compClass = typeof(CompCompressedWaterStorage);
        }
    }
}