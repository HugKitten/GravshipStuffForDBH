using DubsBadHygiene;

namespace GravshipStuffForDubsBadHygiene
{
    public class CompProperties_TreatedWaterInlet : CompProperties_WaterInlet
    {
        public bool EnablesContaminationEvent { get; set; } = true;

        public new bool Deep
        {
            get => base.Deep;
            set => base.Deep = value;
        }
        
        public CompProperties_TreatedWaterInlet() => 
            this.compClass = typeof(CompTreatedWaterInlet);
    }
}