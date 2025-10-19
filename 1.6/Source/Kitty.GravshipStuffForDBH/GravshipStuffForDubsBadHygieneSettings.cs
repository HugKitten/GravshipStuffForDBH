using System;
using System.Collections.Generic;
using System.Linq;
using DubsBadHygiene;
using Verse;

namespace GravshipStuffForDubsBadHygiene.HarmonyPatches
{
    public class GravshipStuffForDubsBadHygieneSettings : ModSettings
    {
        public bool atmosphericIsTreated = true;
        public float atmosphericYieldPowered = 600F;
        public float atmosphericYieldUnpowered = 0F;
        public float atmosphericPowerConsumption = 500F;

        public float tankStorageCapPowered = 4000;
        public float tankStorageCapUnpowered = 12000;
        public float tankPowerConsumption = 200F;

        public bool patchContaminationEvent = true;
        public bool patchWaterTowers = true;

        public GravshipStuffForDubsBadHygieneSettings()
        {
            
        }

        public override void ExposeData()
        {   
            Scribe_Values.Look(ref atmosphericIsTreated, "AtmosphericIsTreated", true);
            Scribe_Values.Look(ref atmosphericYieldPowered, "AtmosphericYieldPowered", 600F);
            Scribe_Values.Look(ref atmosphericYieldUnpowered, "AtmosphericYieldUnpowered", 0F);
            Scribe_Values.Look(ref atmosphericPowerConsumption, "AtmosphericPowerConsumption", 500F);
            
            Scribe_Values.Look(ref tankStorageCapPowered, "TankStorageCapPowered", 12_000F);
            Scribe_Values.Look(ref tankStorageCapUnpowered, "TankStorageCapUnpowered", 4_000F);
            Scribe_Values.Look(ref tankPowerConsumption, "CompressedTankPowerConsumption", 200F);
            
            Scribe_Values.Look(ref patchContaminationEvent, "PatchContaminationEvent", true);
            Scribe_Values.Look(ref patchWaterTowers, "waterTowersToPatch", true);
        }
    }
}