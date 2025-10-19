using System;
using System.Collections.Generic;
using System.Reflection;
using DubsBadHygiene;
using HarmonyLib;
using RimWorld.Planet;

namespace GravshipStuffForDubsBadHygiene.HarmonyPatches
{
    [HarmonyPatchCategory("TowerContaminationIncident")]
    [HarmonyPatch]
    public static class TowerContaminationIncidentPatch
    {
        private static readonly Type TowerContaminationIncidentType =
            AccessTools.TypeByName("DubsBadHygiene.IncidentWorker_TowerContamination");
        private static readonly Type PlumbingNetType = AccessTools.TypeByName("DubsBadHygiene.PlumbingNet");
        private static readonly MethodInfo HasFilterMethod = AccessTools.PropertyGetter(PlumbingNetType, "HasFilter");
        private static readonly CodeInstruction EnablesContaminationIncidentCall = CodeInstruction.Call(typeof(PlumbingNetHelper),
            nameof(PlumbingNetHelper.EnablesContaminationIncident));
        
        static IEnumerable<MethodBase> TargetMethods()
        {
            foreach (var method in AccessTools.GetDeclaredMethods(TowerContaminationIncidentType))
                yield return method;
        }
        
        private static IEnumerable<CodeInstruction> Transpiler(
            IEnumerable<CodeInstruction> instructions )
        {
            foreach (var instruction in instructions)
            {
                if (instruction.Calls(HasFilterMethod))
                {
                    yield return EnablesContaminationIncidentCall;
                    continue;
                }
                
                yield return instruction;
            }
        }
    }
}