using System;
using DubsBadHygiene;
using JetBrains.Annotations;

namespace GravshipStuffForDubsBadHygiene
{
    public class ExternalWaterInlet : CompWaterInlet
    {
        [CanBeNull] public Func<float, float> GetCapacity;

        public override float GetGroundWaterCapacity => GetCapacity?.Invoke(this.GroundWaterCapacity) ?? this.GroundWaterCapacity;

        public new CompProperties_WaterInlet Props
        {
            get
            {
                var inletProps = this.props ??= new CompProperties_WaterInlet();
                return (CompProperties_WaterInlet)inletProps;
            }
            set => props = value;
        }
    
        public ExternalWaterInlet()
        {
            this.props = new CompProperties_WaterInlet();
            this.Pollution = 0F;
            this.PolutionLog = "";
            this.DrawOverlay = false;
        }

        public override void UpdateCap()
        {
            this.GroundWaterCapacity = Props.Capacity;
        }

        public virtual void UpdateCap(float capacity)
        {
            this.GroundWaterCapacity = capacity;
        }

        public override void CompTick()
        {
            this.Pollution = 0F;
            this.DrawOverlay = false;
        }
    }
}