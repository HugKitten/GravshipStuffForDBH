using System;
using DubsBadHygiene;
using RimWorld;
using Verse;

namespace GravshipStuffForDubsBadHygiene
{
    public class CompTreatedWaterInlet : CompWaterInlet
    {
        public virtual bool EnablesContaminationIncident { get; set; } 
        public virtual bool RequiresFilter { get; set; } 
        
        // Renamed to WaterCapacity since not all water sources are groundwater.
        public virtual float WaterCapacity
        {
            get => base.GroundWaterCapacity;
            set => base.GroundWaterCapacity = value;
        }
        
        // Really should have been named like the property above.
        [Obsolete("Use WaterCapacity instead")]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
        public sealed override float GetGroundWaterCapacity => this.WaterCapacity;
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
        
        public new CompProperties_TreatedWaterInlet Props => (CompProperties_TreatedWaterInlet)base.Props;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            this.EnablesContaminationIncident = this.Props.EnablesContaminationEvent;
            this.RequiresFilter = this.Props.RequiresFilter;
        }

        // Overrides to skip ticking if filter is not required
        public override void CompTick()
        {
            if (this.RequiresFilter)
                base.CompTick();
            else
                this.Pollution = 0.0f;
        }
        
        /// <summary>
        /// This is overriden to use virtual WaterCapacity since base method uses protected non-virtual field instead of GetGroundWaterCapacity
        /// Dubs if you see this, please update your code so that a virtual accessor is used here ️
        /// </summary>
        public override string CompInspectStringExtra()
        {
            if (this.ParentHolder is MinifiedThing)
                return base.CompInspectStringExtra();

            var groundCapacity = "GroundCapacity".Translate(this.WaterCapacity.ToString("0"));
            var pollutionLevel = "PollutionLevel".Translate(this.PollutionPct.ToStringPercent());
            
            var pipeComp = this.PipeComp;
            if (pipeComp == null)
            {
                return string.Join(Environment.NewLine, groundCapacity, pollutionLevel);
            }
            else
            {
                var waterStorage =
                    "TotalWaterStorage".Translate(pipeComp.pipeNet.WaterStorage.ToString("0.0"));
                var pipedPumpCapacity = "PipedPumpCapacity".Translate(
                    pipeComp.pipeNet.PumpingCapacitySum.ToString("0"),
                    pipeComp.pipeNet.GroundWaterCapacitySum.ToString("0"),
                    pipeComp.pipeNet.WaterCap.ToStringPercent("0.0"));
                
                return string.Join(Environment.NewLine, groundCapacity, waterStorage, pipedPumpCapacity, pollutionLevel);
            }
        }
    }
}