using System;
using DubsBadHygiene;

namespace GravshipStuffForDubsBadHygiene
{
    public class CompProperties_TreatedWaterInlet : CompProperties_WaterInlet
    {
        public bool EnablesContaminationEvent = true;

        [Obsolete("Use TickContaminationChance instead")]
        public new bool Deep
        {
            get => base.Deep;
            set => base.Deep = value;
        }
        
        public bool RequiresFilter
        {
            get => !base.Deep;
            set => base.Deep = !value;
        }
        
        public CompProperties_TreatedWaterInlet()
        {
            this.compClass = typeof(CompTreatedWaterInlet);
        }
    }
}