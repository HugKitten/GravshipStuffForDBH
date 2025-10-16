using System.Text;
using DubsBadHygiene;
using RimWorld;
using Verse;

namespace GravshipStuffForDubsBadHygiene
{
    public class CompAtmosphericWaterStation : CompWaterPumpingStation
    {
        public ExternalWaterInlet compWaterInlet;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);

            this.compWaterInlet = new ExternalWaterInlet()
            {
                parent = this.parent,
                Props =
                {
                    Capacity = this.Props.Capacity,
                    Deep = false,
                    Piped = false,
                    Radius = 0F
                },
                GetCapacity = this.GetCapacity
            };
            
            this.PipeComp = this.parent.GetComp<CompPipe>();
            this.parent.AllComps.Add(this.compWaterInlet);
            
            var inlet = this.compWaterInlet;
            if (inlet == null) 
                return;
            
            inlet.PostSpawnSetup(respawningAfterLoad);
            inlet.UpdateCap();
        }
        
        protected virtual float GetCapacity(float f)
        {
            return this.WorkingNow ? f : 0F;
        }
        
        public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
        {
            base.PostDeSpawn(map, mode);
            this.parent.AllComps.Remove(this.compWaterInlet);
        }
        
        public override string CompInspectStringExtra()
        {
            if (this.ParentHolder is MinifiedThing)
                return base.CompInspectStringExtra();
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine((string) "AtmosphericYield".Translate((NamedArgument) this.Capacity.ToString("0.0")));
            if (this.PipeComp.pipeNet.WaterTowers.Any<CompWaterStorage>())
                stringBuilder.AppendLine((string) "TotalWaterStorage".Translate((NamedArgument) this.PipeComp.pipeNet.WaterStorage.ToString("0")));
            else
                stringBuilder.AppendLine((string) "NoWaterTowers".Translate());
            stringBuilder.AppendLine((string) "PipedPumpCapacity".Translate((NamedArgument) this.PipeComp.pipeNet.PumpingCapacitySum.ToString("0"), (NamedArgument) this.PipeComp.pipeNet.GroundWaterCapacitySum.ToString("0"), (NamedArgument) this.PipeComp.pipeNet.WaterCap.ToStringPercent("0.0")));
            return stringBuilder.ToString().TrimEndNewlines();
        }
    }
}