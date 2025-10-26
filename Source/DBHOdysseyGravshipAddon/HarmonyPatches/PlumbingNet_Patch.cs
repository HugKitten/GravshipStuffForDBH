using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DubsBadHygiene;
using HarmonyLib;
using Verse;

namespace DBHOdysseyGravshipAddon.HarmonyPatches
{
    // Patches WaterPumpedPerTick calculating 
    [HarmonyPatchCategory("PlumbingNetPatch")]
    public class PlumbingNet_TickWater_Patch
    {
        private static int CountAccepting(IEnumerable<CompWaterStorage> waterStorages,
            Func<CompWaterStorage, bool> predicate) =>
            waterStorages.Count(IsAccepting);

        private static bool IsAccepting(CompWaterStorage storage) =>
            storage is CompPoweredWaterStorage pws 
                ? pws.AmountCanAccept >= 0F
                : storage.WaterStorage < storage.GetWaterStorageCap();

        private static IEnumerable<MethodBase> TargetMethods() =>
            [AccessTools.Method(typeof(PlumbingNet), "TickWater")];

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) =>
            new CodeMatcher(instructions)
                .MatchStartForward(CodeMatch.Calls(() => GenCollection.Count(null, (Predicate<CompWaterStorage>)null)))
                .ThrowIfInvalid("Could not patch IncidentWorker_TowerContamination.TickWater")
                .SetOperandAndAdvance(
                    AccessTools.Method(typeof(PlumbingNet_TickWater_Patch), nameof(CountAccepting))
                ).InstructionEnumeration();
    }
}