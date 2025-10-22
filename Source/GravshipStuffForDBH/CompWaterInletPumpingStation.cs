using System;
using DubsBadHygiene;

namespace GravshipStuffForDubsBadHygiene
{
    /// <summary>
    /// Pumping station that pumps the total water inlet
    /// </summary>
    public class CompWaterInletPumpingStation : CompWaterPumpingStation
    {
        private CompWaterInlet _inlet;
        public override float Capacity => _inlet.GetGroundWaterCapacity;
        
        public new CompProperties_WaterInletPumpingStation Props 
            => (CompProperties_WaterInletPumpingStation)base.props ?? throw new InvalidOperationException();
        
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            _inlet = this.parent.GetComp<CompWaterInlet>();
            base.PostSpawnSetup(respawningAfterLoad);
        }

        // The pumping station inspect string includes duplicate data that is already exposed by the water inlet.
        public override string CompInspectStringExtra() => null;
    }
}