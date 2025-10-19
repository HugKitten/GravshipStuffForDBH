using UnityEngine;
using Verse;

namespace GravshipStuffForDubsBadHygiene.HarmonyPatches
{
    public class GravshipStuffForDubsBadHygieneMod : Mod
    {
        //private Vector2 scrollPosition = Vector2.zero;
        
        public GravshipStuffForDubsBadHygieneSettings Settings { get; }
        public Patcher Patcher { get; }
        
        public GravshipStuffForDubsBadHygieneMod(ModContentPack content) 
            : base(content)
        {
            Settings = this.GetSettings<GravshipStuffForDubsBadHygieneSettings>();
            Patcher = new Patcher(content);

            Patcher.PatchAll();
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
            listingStandard.Label("Atmospheric".Translate());
            listingStandard.CheckboxLabeled("AtmosphericIsTreated".Translate(), ref Settings.atmosphericIsTreated);
            listingStandard.TextFieldNumericLabeled("AtmosphericYieldPowered".Translate(), ref Settings.atmosphericYieldPowered, ref atmosphericYieldPoweredBuffer, 0F, 10_000F);
            listingStandard.TextFieldNumericLabeled("AtmosphericYieldUnpowered".Translate(), ref Settings.atmosphericYieldUnpowered, ref atmosphericYieldUnpoweredBuffer, 0F, 10_000F);
            listingStandard.TextFieldNumericLabeled("AtmosphericPowerConsumption".Translate(), ref Settings.atmosphericPowerConsumption, ref atmosphericPowerConsumptionBuffer, 0F,10_000F);
            
            // Compressed tank
            listingStandard.Label("Tank".Translate());
            listingStandard.TextFieldNumericLabeled("TankCapacityPowered".Translate(), ref Settings.tankStorageCapPowered, ref tankStorageCapPoweredBuffer, 0F,100_000F);
            listingStandard.TextFieldNumericLabeled("TankCapacityUnpowered".Translate(), ref Settings.tankStorageCapUnpowered, ref tankStorageCapUnpoweredBuffer, 0F, 100_000F);
            listingStandard.TextFieldNumericLabeled("TankPowerConsumption".Translate(), ref Settings.tankPowerConsumption, ref tankPowerConsumptionBuffer, 0F, 10_000F);
            
            // Patches
            listingStandard.Label("Patch".Translate());
            listingStandard.CheckboxLabeled("PatchEvent".Translate(), ref Settings.patchContaminationEvent);
            listingStandard.CheckboxLabeled("PatchTowers".Translate(), ref Settings.patchWaterTowers);

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
    }
}