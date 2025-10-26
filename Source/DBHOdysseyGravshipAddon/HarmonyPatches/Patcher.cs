using HarmonyLib;
using Verse;

namespace DBHOdysseyGravshipAddon.HarmonyPatches
{
    [StaticConstructorOnStartup]
    public static class Patcher
    {
        static Patcher()
        {
            var mod = LoadedModManager.GetMod<DBHOdysseyGravshipAddonMod>();
            var settings = mod.GetSettings<DBHOdysseyGravshipAddonSettings>();
            var harmony = new Harmony(mod.Content.PackageId);
            
            if (settings.patchContaminationEvent)
                harmony.PatchCategory("TowerContaminationIncident");

            if (settings.patchWaterTowers) 
                harmony.PatchCategory("WaterStorage");
            
            if (settings.patchPlumbingNet)
                harmony.PatchCategory("PlumbingNetPatch");
        }
    }
} 