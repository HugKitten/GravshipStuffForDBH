using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using DubsBadHygiene;
using HarmonyLib;

namespace GravshipStuffForDubsBadHygiene.HarmonyPatches
{
    /// <summary>
    /// Patch to prevent contamination incident on pipetnets with only treated water source
    /// </summary>
    [HarmonyPatchCategory("TowerContaminationIncident")]
    public static class TowerContaminationIncidentPatch
    {
        private static IEnumerable<PlumbingNet> WhereTreated(this IEnumerable<PlumbingNet> source,
            Func<PlumbingNet, bool> predicate) =>
            source.Where(predicate).Where(EnablesContaminationIncident);

        private static bool EnablesContaminationIncident(PlumbingNet plumbingNet) =>
            !plumbingNet.WaterWells.All(w => w is CompTreatedWaterInlet { EnablesContaminationIncident: false });

        private static IEnumerable<MethodBase> TargetMethods() =>
        [
            AccessTools.Method(AccessTools.TypeByName("DubsBadHygiene.IncidentWorker_TowerContamination"), "pango")
        ];

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) =>
            new CodeMatcher(instructions)
                .MatchStartForward(CodeMatch.Calls(() => Enumerable.Where(null, (Func<PlumbingNet, bool>)null)))
                .ThrowIfInvalid("Could not patch IncidentWorker_TowerContamination.pango")
                // only change the operand (the MethodInfo) — keep opcode and labels intact
                .SetOperandAndAdvance(
                    AccessTools.Method(typeof(TowerContaminationIncidentPatch), nameof(WhereTreated))
                ).InstructionEnumeration();
    }
}