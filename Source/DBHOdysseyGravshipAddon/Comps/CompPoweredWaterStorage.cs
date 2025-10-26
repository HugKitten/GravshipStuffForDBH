using System.Collections.Generic;
using DubsBadHygiene;
using RimWorld;
using UnityEngine;
using Verse;

namespace DBHOdysseyGravshipAddon
{
    // Note for tomrrow
    // Consider having two tanks and deregistering when unpowered
    /// <summary>
    /// Water storage class that supports power/fuel and an overridable water cap
    /// Feel free to extend this class if you want to include such a feature in your mod
    /// Dubs if you see this, please consider adding a virtual WaterStorageCap property and having the constructor set it via props class 💕
    /// Also maybe move water gen to their own comps
    /// </summary>
    public class CompPoweredWaterStorage : CompWaterStorage
    {
        public new CompProperties_PoweredWaterStorage Props => (CompProperties_PoweredWaterStorage)this.props;
        
        /// <summary>
        /// Cap on amount of water that can be stored
        /// </summary>
        public virtual float WaterStorageCap =>
            this.WorkingNow
                ? Props.WaterStorageCapUnpowered
                : Props.WaterStorageCapPowered;

        /// <summary>
        /// Total amount of water that this tank can store  
        /// </summary>
        public virtual float AmountCanAccept => this.WaterStorageCap - this.WaterStorage;
        
        /// <summary>
        /// Update fill tank gizmo
        /// </summary>
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (var gizmo in base.CompGetGizmosExtra())
            {
                if (gizmo is Command_Action { action.Method.Name: nameof(CompWaterStorage.FillTank) } ca)
                    ca.action = this.FillTank;
                yield return gizmo;
            }
        }
        
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);

            // initial water quality update
            UpdateWaterQuality();
        }

        /// <summary>
        /// Base method uses water storage cap field which is not overridable.
        /// I suppose I *could* use CompPowerTrader, CompRefuelable and CompBreakdownable events, but this is easier
        /// </summary>
        public override void CompTick()
        {
            // Dont tick minified
            if (this.ParentHolder is MinifiedThing) 
              return;
            
            // Drank tank
            if (this.DrainTank)
            {
                CompTickDrain();
                return;
            }

            // Auto gen water
            CompTickAutoGen();
            
            // Supply water to pipe net
            CompTickAcceptPumpedWater();

            // Update water quality
            if (this.parent.IsHashIntervalTick(5000))
                UpdateWaterQuality();
        }

        public override void PostDraw()
        {
            if (this.DrawOverlay)
            {
                var r = new GenDraw.FillableBarRequest
                {
                    center = this.parent.DrawPos + Vector3.up * 0.1f,
                    size = CompWaterStorage.BarSize,
                    fillPercent = this.CapPercent,
                    filledMat = CompWaterStorage.WaterBarFilledMat,
                    unfilledMat = CompWaterStorage.BarUnfilledMat,
                    margin = 0.05f
                };
                var rotation = this.parent.Rotation;
                rotation.Rotate(RotationDirection.Clockwise);
                r.rotation = rotation;
                GenDraw.DrawFillableBar(r);
                this.DrawOverlay = false;
            }
            else if (this.DrainTank)
                DubUtils.RenderPulsingOverlay(this.parent, GraphicsCache.Draining, this.parent.DrawPos, MeshPool.plane08, Quaternion.identity);
            else if (this.WaterQuality == ContaminationLevel.Contaminated)
            {
                DubUtils.RenderPulsingOverlay(this.parent, CompWaterStorage.ContaminationMat, this.parent.DrawPos, MeshPool.plane08, Quaternion.identity);
            }
            else
            {
                if (!this.LowCapacity)
                    return;
                DubUtils.RenderPulsingOverlay(this.parent, CompWaterStorage.LowWaterMat, this.parent.DrawPos, MeshPool.plane08, Quaternion.identity);
            }
        }

        /// <summary>
        /// Fill water
        /// </summary>
        /// <param name="amount">Amount of water to fill by</param>
        public virtual void AddWater(float amount)
        {
            if (amount < 0)
            {
                Log.Error("Cannot add negative water " + amount);
                return;
            }
            
            if (amount > this.AmountCanAccept)
                amount = this.AmountCanAccept;

            this.WaterStorage += amount;
        }
        
        /// <summary>
        /// Fill tank fully
        /// </summary>
        public void DrawWater(float amount)
        {
            this.WaterStorage -= amount;
            if (this.WaterStorage >= 0.0)
                return;
            Log.Error("Drawing water we don't have from " + this.parent);
            this.WaterStorage = 0.0f;
        }
        
        /// <summary>
        /// Handle draining water on breakdown
        /// </summary>
        public override void ReceiveCompSignal(string signal)
        {
            base.ReceiveCompSignal(signal);

            if (signal != "Breakdown")
                return;
            this.DrawWater(this.WaterStorage);
        }

        public virtual void UpdateWaterQuality()
        {
            var pipeNet = this.PipeComp?.pipeNet;
            if (pipeNet == null)
                return;
            
            if (pipeNet.IsTreated())
                this.WaterQuality = ContaminationLevel.Treated;
            else if (this.WaterQuality != ContaminationLevel.Contaminated)
                this.WaterQuality = ContaminationLevel.Untreated;
        }
        
        protected virtual void CompTickDrain()
        {
            if (!this.parent.IsHashIntervalTick(10))
                return;
            
            // Get amount of water to push
            var totalPush = Mathf.Min(this.WaterStorage, 1f);
            if (totalPush <= 0.0) 
                return;
            
            // Empty tank by pushed amount
            DrawWater(this.PipeComp.pipeNet.BetterPushWater(totalPush));
        }

        /// <summary>
        /// handle auto generated water
        /// </summary>
        protected virtual void CompTickAutoGen()
        {
            if (!this.parent.IsHashIntervalTick(this.Props.Tickrate))
                return;

            if (!this.WorkingNow)
                return;

            if (this.Props.AutoOnRain)
            {
                // Fill with rain water
                var rainRate = this.parent.Map.weatherManager.RainRate;
                if (rainRate > 0.009999999776482582) 
                    this.AddWater(this.Props.AutoGenRate * rainRate);
            }
            else if (this.Props.Auto)
            {
                // Fill with gen water
                this.AddWater(this.Props.AutoGenRate);
            }
        }

        /// <summary>
        /// Fill per pipenet amount
        /// </summary>
        protected virtual void CompTickAcceptPumpedWater()
        {
            var pipeNet = this.PipeComp?.pipeNet;
            if (pipeNet == null)
                return;

            this.AddWater(ModOption.WaterPumpCapacity.Val == 0F ?
                this.AmountCanAccept : 
                pipeNet.WaterPumpedPerTick);
        }
    }
}