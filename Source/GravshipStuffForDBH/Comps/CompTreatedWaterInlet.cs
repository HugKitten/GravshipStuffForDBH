using System;
using DubsBadHygiene;
using RimWorld;
using Verse;

namespace GravshipStuffForDubsBadHygiene
{
    /// <summary>
    /// Water inlet that supports overridable control over contamination incident and self containment ticking
    /// Feel free to extend this class if you want to include such a feature in your mod
    /// Dubs if you see this, please provide native support for
    /// contamination ticking and contamination event requirements 💕
    /// </summary>
    public class CompTreatedWaterInlet : CompWaterInlet
    {
        /// <summary>
        /// If this water inlet enables the contamination incent to trigger
        /// (small chance to trigger if any water inlet enables this incident and there is no filter)
        /// </summary>
        public virtual bool EnablesContaminationIncident => Props.EnablesContaminationEvent;

        /// <summary>
        /// If this water inlet will tick for its own contamination
        /// (chance stacks with other inlets)
        /// </summary>
        public virtual bool RequiresFilter => !Props.Deep;
        
        public new CompProperties_TreatedWaterInlet Props => (CompProperties_TreatedWaterInlet)base.Props;
        
        /// <summary>
        /// Ticks contamination if a filter is required
        /// </summary>
        public override void CompTick()
        {
            if (this.RequiresFilter)
                base.CompTick();
            else
                this.Pollution = 0.0f;
        }
        
        /// <summary>
        /// This is overriden to use virtual WaterCapacity since base method uses protected non-virtual field instead of GetGroundWaterCapacity
        /// Dubs if you see this, please update your code so that the virtual accessor is used here ️
        /// </summary>
        public override string CompInspectStringExtra()
        {
            if (this.ParentHolder is MinifiedThing)
                return base.CompInspectStringExtra();

            var groundCapacity = "GroundCapacity".Translate(this.GroundWaterCapacity.ToString("0"));
            var pollutionLevel = "PollutionLevel".Translate(this.PollutionPct.ToStringPercent());
            
            var pipeComp = this.PipeComp;
            if (pipeComp == null)
                return string.Join(Environment.NewLine, groundCapacity, pollutionLevel);

            var pipeNet = pipeComp.pipeNet;
            var waterStorage =
                "TotalWaterStorage".Translate(pipeNet.WaterStorage.ToString("0.0"));
            var pipedPumpCapacity = "PipedPumpCapacity".Translate(
                pipeNet.PumpingCapacitySum.ToString("0"),
                pipeNet.GroundWaterCapacitySum.ToString("0"),
                pipeNet.WaterCap.ToStringPercent("0.0"));
                
            return string.Join(Environment.NewLine, groundCapacity, waterStorage, pipedPumpCapacity, pollutionLevel);
        }
    }
}