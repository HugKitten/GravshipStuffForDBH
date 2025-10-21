using UnityEngine;
using Verse;

namespace GravshipStuffForDubsBadHygiene
{
    public class GravshipStuffForDubsBadHygieneMod : Mod
    {
        //private Vector2 scrollPosition = Vector2.zero;
        
        public GravshipStuffForDubsBadHygieneSettings Settings { get; }
        
        public GravshipStuffForDubsBadHygieneMod(ModContentPack content) 
            : base(content)
        {
            Settings = this.GetSettings<GravshipStuffForDubsBadHygieneSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            var tankStorageCapPoweredBuffer = Settings.atmosphericYieldPowered.ToString("0.0F");
            var tankStorageCapUnpoweredBuffer = Settings.atmosphericYieldUnpowered.ToString("0.0F");
            var atmosphericPowerConsumptionBuffer = Settings.atmosphericPowerConsumption.ToString("0.0F");
            
            var atmosphericYieldPoweredBuffer = Settings.atmosphericYieldPowered.ToString("0.0F");
            var atmosphericYieldUnpoweredBuffer = Settings.atmosphericYieldUnpowered.ToString("0.0F");
            var tankPowerConsumptionBuffer = Settings.tankPowerConsumption.ToString("0.0F");
            
            var listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            // Water station
            listingStandard.Label("GSSFDBH_Atmospheric".Translate());
            listingStandard.CheckboxLabeled("GSSFDBH_AtmosphericIsTreated".Translate(), ref Settings.atmosphericIsTreated);
            listingStandard.TextFieldNumericLabeled("GSSFDBH_AtmosphericYieldPowered".Translate(), ref Settings.atmosphericYieldPowered, ref atmosphericYieldPoweredBuffer, 0F, 10_000F);
            listingStandard.TextFieldNumericLabeled("GSSFDBH_AtmosphericYieldUnpowered".Translate(), ref Settings.atmosphericYieldUnpowered, ref atmosphericYieldUnpoweredBuffer, 0F, 10_000F);
            listingStandard.TextFieldNumericLabeled("GSSFDBH_AtmosphericPowerConsumption".Translate(), ref Settings.atmosphericPowerConsumption, ref atmosphericPowerConsumptionBuffer, 0F,10_000F);
            
            // Compressed tank
            listingStandard.Label("GSSFDBH_Tank".Translate());
            listingStandard.TextFieldNumericLabeled("GSSFDBH_TankCapacityPowered".Translate(), ref Settings.tankStorageCapPowered, ref tankStorageCapPoweredBuffer, 0F,100_000F);
            listingStandard.TextFieldNumericLabeled("GSSFDBH_TankCapacityUnpowered".Translate(), ref Settings.tankStorageCapUnpowered, ref tankStorageCapUnpoweredBuffer, 0F, 100_000F);
            listingStandard.TextFieldNumericLabeled("GSSFDBH_TankPowerConsumption".Translate(), ref Settings.tankPowerConsumption, ref tankPowerConsumptionBuffer, 0F, 10_000F);
            
            // Patches
            listingStandard.Label("GSSFDBH_Patch".Translate());
            listingStandard.CheckboxLabeled("GSSFDBH_PatchEvent".Translate(), ref Settings.patchContaminationEvent);
            listingStandard.CheckboxLabeled("GSSFDBH_PatchTowers".Translate(), ref Settings.patchWaterTowers);

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

        public override string SettingsCategory() => "Gravship Stuff for DBH";
    }
}