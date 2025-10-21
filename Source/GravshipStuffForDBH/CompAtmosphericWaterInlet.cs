using System;
using GravshipStuffForDubsBadHygiene.HarmonyPatches;
using RimWorld;
using Verse;

namespace GravshipStuffForDubsBadHygiene
{
    public class CompAtmosphericWaterInlet : CompTreatedWaterInlet
    {
        private CompBreakdownable _breakdownableComp;
        private CompRefuelable _fuelComp;
        private CompPowerTrader _powerComp;
        
        // Exposes dynamic data from the static properties
        public virtual float YieldPowered { get; set; }
        public virtual float YieldUnpowered { get; set; }

        public bool WorkingNow
        {
            get
            {
                if (!FlickUtility.WantsToBeOn(this.parent) || this._powerComp != null && !this._powerComp.PowerOn || this._fuelComp != null && !this._fuelComp.HasFuel)
                    return false;
                return this._breakdownableComp == null || !this._breakdownableComp.BrokenDown;
            }
        }

        public sealed override float WaterCapacity
        {
            get => this.WorkingNow ? this.YieldPowered : this.YieldUnpowered;
            set => throw new NotSupportedException();
        }
        
        public new CompProperties_AtmosphericInlet Props => (CompProperties_AtmosphericInlet)base.Props;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            var settings = LoadedModManager.GetMod<GravshipStuffForDubsBadHygieneMod>()
                .GetSettings<GravshipStuffForDubsBadHygieneSettings>();
            
            base.PostSpawnSetup(respawningAfterLoad);
            this._powerComp = this.parent.GetComp<CompPowerTrader>();
            this._fuelComp = this.parent.GetComp<CompRefuelable>();
            this._breakdownableComp = this.parent.GetComp<CompBreakdownable>();

            this.EnablesContaminationIncident = !settings.atmosphericIsTreated;
            this.RequiresFilter = !settings.atmosphericIsTreated;
            this.YieldPowered = settings.atmosphericYieldPowered;
            this.YieldUnpowered = settings.atmosphericYieldUnpowered;
            this.Pollution = 0F;
            this.PolutionLog = "";
            
            UpdateCap();
        }
        
        public override string CompInspectStringExtra()
        {
            if (this.ParentHolder is MinifiedThing)
                return base.CompInspectStringExtra();

            var pipeNet = this.PipeComp.pipeNet;
            var atmosphericYield = "GSSFDBH_AtmosphericYield".Translate(this.WaterCapacity.ToString("0.0"));
            var totalWaterStorage = pipeNet.WaterTowers.Any()
                ? "TotalWaterStorage".Translate(pipeNet.WaterStorage.ToString("0"))
                : "NoWaterTowers".Translate();
            var pipedPumpCapacity = "PipedPumpCapacity".Translate(
                pipeNet.PumpingCapacitySum.ToString("0"),
                pipeNet.GroundWaterCapacitySum.ToString("0"),
                pipeNet.WaterCap.ToStringPercent("0.0"));
            return string.Join(Environment.NewLine, atmosphericYield, totalWaterStorage, pipedPumpCapacity);
        }
    }
}