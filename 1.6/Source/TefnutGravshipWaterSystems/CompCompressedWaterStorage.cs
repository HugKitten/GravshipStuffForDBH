using DubsBadHygiene;

namespace GravshipStuffForDubsBadHygiene
{
    public class CompCompressedWaterStorage : CompWaterStorage
    {
        public new CompProperties_CompressedWaterStorage Props => (CompProperties_CompressedWaterStorage) this.props;
        
        public override void CompTick()
        {
            base.CompTick();
            if (!base.WorkingNow && this.WaterStorage > this.Props.UnpoweredWaterStorageCap)
            {
                this.WaterStorage = this.Props.UnpoweredWaterStorageCap;
            }
        }
    }
}