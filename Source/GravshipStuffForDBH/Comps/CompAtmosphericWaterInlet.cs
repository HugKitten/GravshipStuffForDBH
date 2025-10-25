using System;
using RimWorld;
using Verse;

namespace GravshipStuffForDubsBadHygiene
{
    /// <summary>
    /// Powered water storage that uses mod settings
    /// Inherit from CompTreatedWaterInlet instead in your own mod.
    /// </summary>
    public sealed class CompAtmosphericWaterInlet : CompPoweredWaterInlet
    {
        private readonly GravshipStuffForDubsBadHygieneSettings _settings = LoadedModManager.GetMod<GravshipStuffForDubsBadHygieneMod>()
            .GetSettings<GravshipStuffForDubsBadHygieneSettings>();
        
        public new CompProperties_AtmosphericInlet Props => (CompProperties_AtmosphericInlet)base.Props;

        public override bool EnablesContaminationIncident => !_settings.atmosphericIsTreated;
        public override bool RequiresFilter => !_settings.atmosphericIsTreated;

        public override float GetGroundWaterCapacity =>
            this.WorkingNow ? _settings.atmosphericYieldPowered : _settings.atmosphericYieldUnpowered;

        public override void SpawnSetup()
        {
            UpdatePowerOutput();
        }

        public override void CompTick()
        {
            base.CompTick();
            if (this.parent.IsHashIntervalTick(250))
                UpdatePowerOutput();
            if (this.parent.IsHashIntervalTick(250))
                UpdateCap();
        }

        /// <summary>
        /// Update description with powered yield
        /// </summary>
        public override string GetDescriptionPart()
        {
            return "GSSFDBH_AtmosphericDescription".Translate(this._settings.atmosphericYieldPowered);
        }

        /// <summary>
        /// Renamed capacty to atmospheric yield
        /// </summary>
        public override string CompInspectStringExtra()
        {
            if (this.ParentHolder is MinifiedThing)
                return base.CompInspectStringExtra();

            var pipeNet = this.PipeComp.pipeNet;
            var atmosphericYield = "GSSFDBH_AtmosphericYield".Translate(this.GetGroundWaterCapacity.ToString("0.0"));
            var totalWaterStorage = pipeNet.WaterTowers.Any()
                ? "TotalWaterStorage".Translate(pipeNet.WaterStorage.ToString("0"))
                : "NoWaterTowers".Translate();
            var pipedPumpCapacity = "PipedPumpCapacity".Translate(
                pipeNet.PumpingCapacitySum.ToString("0"),
                pipeNet.GroundWaterCapacitySum.ToString("0"),
                pipeNet.WaterCap.ToStringPercent("0.0"));
            return string.Join(Environment.NewLine, atmosphericYield, totalWaterStorage, pipedPumpCapacity);
        }
        
        /// <summary>
        /// Overrides water capacity calculation
        /// </summary>
        public override void UpdateCap()
        {
            this.GroundWaterCapacity =
                this.WorkingNow ? _settings.atmosphericYieldPowered : _settings.atmosphericYieldUnpowered;
        }
        
        /// <summary>
        /// Update power usaage
        /// </summary>
        private void UpdatePowerOutput()
        {
            var powerComp = this.PowerComp;
            if (powerComp == null)
                return;
            powerComp.PowerOutput = -_settings.tankPowerConsumption;
        }
    }
}