using System;
using System.Collections.Generic;
using DubsBadHygiene;
using RimWorld;
using UnityEngine;
using Verse;

namespace GravshipStuffForDubsBadHygiene
{
    public class CompCompressedWaterStorage : CompWaterStorage
    {
        // Exposes dynamic data from the static properties
        public virtual float  PoweredWaterStorageCap { get; set; }
        public virtual float  UnpoweredWaterStorageCap { get; set; }
        public virtual float AutoGenRate { get; set; }
        public virtual int Tickrate { get; set; }
        public virtual bool Auto { get; set; }
        public virtual bool AutoOnRain { get; set; }
        
        /// <summary>
        /// How much water that can be stored.
        /// Dubs if you see this, please include a virtual WaterStorageCap ❤️
        /// </summary>
        public virtual float WaterStorageCap => this.WorkingNow ? this.PoweredWaterStorageCap : this.UnpoweredWaterStorageCap;
        public virtual bool IsFull => this.WaterStorage >= this.WaterStorageCap;
        public virtual bool IsAccepting => this.WaterStorage <= this.WaterStorageCap;
        public virtual bool IsEmpty => this.WaterStorage <= 0;
        public new virtual bool LowCapacity => this.WaterStorage < this.WaterStorageCap * 0.2;

        public new virtual float CapPercent => this.WaterStorageCap == 0.0 ? 0.0f : this.WaterStorage / this.WaterStorageCap;
        public new float space => Mathf.Max(0.0f, this.WaterStorageCap - this.WaterStorage);
        
        public new CompProperties_CompressedWaterStorage Props => (CompProperties_CompressedWaterStorage) this.props ?? throw new InvalidOperationException("Props not set");
        public new void FillTank() => this.WaterStorage = this.WaterStorageCap;

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (var gizmo in base.CompGetGizmosExtra())
            {
                if (gizmo is Command_Action { defaultLabel: "Fill" } ca)
                    ca.action = this.FillTank;
                yield return gizmo;
            }
        }
        
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);

            this.PoweredWaterStorageCap = Props.WaterStorageCapPowered;
            this.UnpoweredWaterStorageCap = Props.WaterStorageCapUnpowered;
            this.AutoGenRate = this.Props.AutoGenRate;
            this.Tickrate = this.Props.Tickrate;
            this.Auto = this.Props.Auto;
            this.AutoOnRain = this.Props.AutoOnRain;
            
            /*
            var settings = LoadedModManager.GetMod<GravshipStuffForDubsBadHygieneMod>()
                .GetSettings<GravshipStuffForDubsBadHygieneSettings>();
            if (this.powerComp != null)
                this.powerComp.PowerOutput = -settings.tankPowerConsumption;
            */
        }

        public override string GetDescriptionPart()
        {
            return "GSSFDBH_TankDescription".Translate(this.PoweredWaterStorageCap, this.UnpoweredWaterStorageCap);
        }

        /// <summary>
        /// Base method uses water storage cap field which is not overridable.
        /// I suppose I *could* use CompPowerTrader, CompRefuelable and CompBreakdownable events, but this is easier
        /// </summary>
        public override void CompTick()
        {
            var waterStorageCap = this.WaterStorageCap;
            
            if (this.ParentHolder is MinifiedThing) 
              return;
            
            if (this.DrainTank)
            {
              if (!this.parent.IsHashIntervalTick(10))
                return;
              
              var waterBufferToDistribute = Mathf.Min(this.WaterStorage, 1f);
              if (waterBufferToDistribute <= 0.0) 
                  return;
              
              this.WaterStorage -= waterBufferToDistribute;
              this.WaterStorage += this.PipeComp.pipeNet.PushWater(waterBufferToDistribute);
            }
            else
            {
                if ((this.Auto || this.AutoOnRain && this.parent.Map.weatherManager.RainRate > 0.009999999776482582) && this.parent.IsHashIntervalTick(this.Tickrate) && this.WorkingNow)
                {
                    var autoGenRate = this.AutoGenRate;
                    if (this.AutoOnRain)
                        autoGenRate *= this.parent.Map.weatherManager.RainRate;
                    this.WaterStorage += autoGenRate;
                    
                    if (this.WaterStorage > this.WaterStorageCap)
                      this.WaterStorage = this.WaterStorageCap;
                    if (float.IsNaN(this.WaterStorage))
                    {
                        this.WaterStorage = 0.0f;
                        Log.Warning("NaN on WaterStorage in CompTick1");
                    }
                }
                if (this.PipeComp?.pipeNet == null)
                  return;
                
                this.WaterStorage += this.PipeComp.pipeNet.WaterPumpedPerTick;
                if (ModOption.WaterPumpCapacity.Val == 0.0 || this.WaterStorage > this.WaterStorageCap)
                    this.WaterStorage = this.WaterStorageCap;
                if (this.WaterStorage < 0.0)
                    this.WaterStorage = 0.0f;
                if (float.IsNaN(this.WaterStorage))
                {
                    this.WaterStorage = 0.0f;
                    Log.Warning("NaN on WaterStorage in CompTick2");
                }
                
                if (!this.parent.IsHashIntervalTick(5000))
                    return;

                if (this.PipeComp.pipeNet.IsTreated())
                    this.WaterQuality = ContaminationLevel.Treated;
                else if (this.WaterQuality != ContaminationLevel.Contaminated)
                    this.WaterQuality = ContaminationLevel.Untreated;
            }
        }
    }
}