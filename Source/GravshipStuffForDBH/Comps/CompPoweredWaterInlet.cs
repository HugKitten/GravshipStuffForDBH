using RimWorld;

namespace GravshipStuffForDubsBadHygiene;

/// <summary>
/// Water inlet that requires power/fuel (And all the features from CompTreatedWaterInlet)
/// Feel free to extend this class if you want to include such a feature in your mod
/// Dubs if you see this, please consider adding native support (Although I'll understand if you dont)
/// </summary>
public class CompPoweredWaterInlet : CompTreatedWaterInlet
{
    protected CompPowerTrader PowerComp  { get; private set; }
    protected CompRefuelable FuelComp  { get; private  set; }
    protected CompBreakdownable BreakdownableComp { get; private  set; }

    /// <summary>
    /// If machine is in working state
    /// </summary>
    public virtual bool WorkingNow =>
        FlickUtility.WantsToBeOn(this.parent) && this.PowerComp is not { PowerOn: false } &&
        this.FuelComp is not { HasFuel: false } && this.BreakdownableComp is not { BrokenDown: true };
    
    /// <summary>
    /// Get current incoming water
    /// </summary>
    public override float GetGroundWaterCapacity =>
        this.WorkingNow ?
            Props.CapacityPowered : 
            Props.CapacityUnpowered;

    public new CompProperties_PoweredWaterInlet Props => (CompProperties_PoweredWaterInlet)base.Props;

    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        base.PostSpawnSetup(respawningAfterLoad);
            
        // Get comps
        this.PowerComp = this.parent.GetComp<CompPowerTrader>();
        this.FuelComp = this.parent.GetComp<CompRefuelable>();
        this.BreakdownableComp = this.parent.GetComp<CompBreakdownable>();
            
        // Defaults pollution
        this.Pollution = 0F;
        this.PolutionLog = "";
    }
        
    /// <summary>
    /// Handle update cap on changes
    /// </summary>
    public override void ReceiveCompSignal(string signal)
    {
        base.ReceiveCompSignal(signal);
        if (signal != "PowerTurnedOn" && signal != "PowerTurnedOff" && signal != "FlickedOn" && signal != "FlickedOff" && signal != "Refueled" && signal != "RanOutOfFuel" && signal != "Breakdown")
            return;
        
        UpdateCap();
    }
}