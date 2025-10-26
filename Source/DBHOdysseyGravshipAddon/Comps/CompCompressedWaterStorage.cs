using Verse;

namespace DBHOdysseyGravshipAddon;

/// <summary>
/// Powered water storage that uses mod settings
/// Inherit from CompPoweredWaterStorage instead in your own mod.
/// </summary>
public sealed class CompCompressedWaterStorage : CompPoweredWaterStorage
{
    private readonly DBHOdysseyGravshipAddonSettings _settings = LoadedModManager.GetMod<DBHOdysseyGravshipAddonMod>()
        .GetSettings<DBHOdysseyGravshipAddonSettings>();

    public override float WaterStorageCap =>
        this.WorkingNow ? _settings.tankStorageCapPowered : _settings.tankStorageCapUnpowered;

    public override void CompTick()
    {
        base.CompTick();
        if (this.parent.IsHashIntervalTick(250))
            UpdatePowerOutput();
    }

    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        base.PostSpawnSetup(respawningAfterLoad);
        UpdatePowerOutput();
    }

    public override string GetDescriptionPart() => 
        "GSSFDBH_TankDescription".Translate(_settings.tankStorageCapPowered, _settings.tankStorageCapUnpowered);
    
    private void UpdatePowerOutput()
    {
        var powerComp = this.powerComp;
        if (powerComp == null)
            return;
        powerComp.PowerOutput = -_settings.tankPowerConsumption;
    }
}