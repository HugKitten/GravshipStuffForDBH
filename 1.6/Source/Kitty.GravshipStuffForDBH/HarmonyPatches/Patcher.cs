using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DubsBadHygiene;
using HarmonyLib;
using Mono.Cecil.Cil;
using Verse;

namespace GravshipStuffForDubsBadHygiene.HarmonyPatches
{
    [StaticConstructorOnStartup]
    public static class Patcher
    {
        static Patcher()
        {
            var mod = LoadedModManager.GetMod<GravshipStuffForDubsBadHygieneMod>();
            var settings = mod.GetSettings<GravshipStuffForDubsBadHygieneSettings>();
            var harmony = new Harmony(mod.Content.PackageId);
            
            if (settings.patchContaminationEvent)
                harmony.PatchCategory("TowerContaminationIncident");

            if (settings.patchWaterTowers) 
                harmony.PatchCategory("WaterStorage");
        }
    }
} 