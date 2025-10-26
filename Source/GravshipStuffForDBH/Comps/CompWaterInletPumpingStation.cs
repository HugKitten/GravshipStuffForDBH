using DubsBadHygiene;

namespace GravshipStuffForDubsBadHygiene
{
    /// <summary>
    /// Pumping station that pumps the total water of its inlet
    /// Feel free to extend this class if you want to include such a feature in your mod
    /// </summary>
    public class CompWaterInletPumpingStation : CompWaterPumpingStation
    {
        /// <summary>
        /// Inlet to get water capacity from
        /// </summary>
        private CompWaterInlet _inlet;
        
        /// <summary>
        /// Get the incoming water (Uses value from CompWaterInlet)
        /// </summary>
        public sealed override float Capacity => _inlet.GetGroundWaterCapacity;

        public new CompProperties_WaterInletPumpingStation Props
            => (CompProperties_WaterInletPumpingStation)base.props;
        
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            this._inlet = this.parent.GetComp<CompWaterInlet>();
            
            base.PostSpawnSetup(respawningAfterLoad);
        }

        // Dont output inspect string as inlet already provides enough data
        public override string CompInspectStringExtra() => null;
    }
}