using UnityEngine;
using Verse;

namespace DBHOdysseyGravshipAddon
{
    public class DBHOdysseyGravshipAddonMod : Mod
    {
        //private Vector2 scrollPosition = Vector2.zero;
        
        public DBHOdysseyGravshipAddonSettings Settings { get; }
        
        public DBHOdysseyGravshipAddonMod(ModContentPack content) 
            : base(content)
        {
            Settings = this.GetSettings<DBHOdysseyGravshipAddonSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            var atmosphericYieldPoweredBuffer = Settings.atmosphericYieldPowered.ToString("0.0");
            var atmosphericYieldUnpoweredBuffer = Settings.atmosphericYieldUnpowered.ToString("0.0");
            var atmosphericPowerConsumptionBuffer = Settings.atmosphericPowerConsumption.ToString("0.0");
            
            var tankStorageCapPoweredBuffer = Settings.tankStorageCapPowered.ToString("0.0");
            var tankStorageCapUnpoweredBuffer = Settings.tankStorageCapUnpowered.ToString("0.0");
            var tankPowerConsumptionBuffer = Settings.tankPowerConsumption.ToString("0.0");
            
            var listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            
            // Water station
            listingStandard.Label("GSSFDBH_Atmospheric".Translate());
            listingStandard.CheckboxLabeled("GSSFDBH_AtmosphericIsTreated".Translate(), ref Settings.atmosphericIsTreated);
            listingStandard.TextFieldNumericLabeled("GSSFDBH_AtmosphericYieldPowered".Translate(), ref Settings.atmosphericYieldPowered, ref atmosphericYieldPoweredBuffer, 0F, 10_000F);
            listingStandard.TextFieldNumericLabeled("GSSFDBH_AtmosphericYieldUnpowered".Translate(), ref Settings.atmosphericYieldUnpowered, ref atmosphericYieldUnpoweredBuffer, 0F, 10_000F);
            listingStandard.TextFieldNumericLabeled("GSSFDBH_AtmosphericPowerConsumption".Translate(), ref Settings.atmosphericPowerConsumption, ref atmosphericPowerConsumptionBuffer, 0F,10_000F);
            
            listingStandard.Gap();
            
            // Compressed tank
            listingStandard.Label("GSSFDBH_Tank".Translate());
            listingStandard.TextFieldNumericLabeled("GSSFDBH_TankCapacityPowered".Translate(), ref Settings.tankStorageCapPowered, ref tankStorageCapPoweredBuffer, 0F,100_000F);
            listingStandard.TextFieldNumericLabeled("GSSFDBH_TankCapacityUnpowered".Translate(), ref Settings.tankStorageCapUnpowered, ref tankStorageCapUnpoweredBuffer, 0F, 100_000F);
            listingStandard.TextFieldNumericLabeled("GSSFDBH_TankPowerConsumption".Translate(), ref Settings.tankPowerConsumption, ref tankPowerConsumptionBuffer, 0F, 10_000F);
            
            listingStandard.GapLine();
            
            // Patches
            listingStandard.Label("GSSFDBH_Patch".Translate());
            listingStandard.CheckboxLabeled("GSSFDBH_PatchEvent".Translate(), ref Settings.patchContaminationEvent);
            listingStandard.CheckboxLabeled("GSSFDBH_PatchTowers".Translate(), ref Settings.patchWaterTowers);
            listingStandard.CheckboxLabeled("GSSFDBH_PatchPlumbingNet".Translate(), ref Settings.patchPlumbingNet);
            
            listingStandard.Gap();
            
            var reset = listingStandard.ButtonText("GSSFDBH_Reset".Translate());
            if (reset)
            {
                Settings.ResetToDefault();
            }
            
            /*
            listingStandard.Label("Patch water storage to see the atmospheric water station as a treated water source.");
            Widgets.BeginScrollView(inRect, ref scrollPosition, new Rect(0, 0, inRect.width, 10));
            foreach (var type in Patcher.ClassesToPatch)
            {
                var name = type.FullName ?? type.Name;
                var isEnabled = Settings.wellsToPatch.TryGetValue(name, out var value) && value;
                PatchCard.Draw(inRect, this.Settings, name);
                Settings.wellsToPatch.Add(name, true);
            }
            */
            
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory() => "Dubs' Bad Hygiene - Odyssey Gravship Addon";
    }
}