using System.Collections.Generic;
using System.Reflection;
using DubsBadHygiene;
using HarmonyLib;

namespace GravshipStuffForDubsBadHygiene.HarmonyPatches
{
    [HarmonyPatch]
    public class TowerContaminationIncident_Patch
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            var towerContaminationIncidentType =
                AccessTools.TypeByName("DubsBadHygiene.IncidentWorker_TowerContamination");
            foreach (var method in AccessTools.GetDeclaredMethods(towerContaminationIncidentType))
                yield return method;
        }
        
        private static IEnumerable<CodeInstruction> Transpiler(
            IEnumerable<CodeInstruction> instructions )
        {
            foreach (var instruction in instructions)
            {
                if (instruction.Calls(AccessTools.PropertyGetter(typeof(PlumbingNet), nameof(PlumbingNet.HasFilter))))
                {
                    yield return CodeInstruction.Call(typeof(PlumbingNetHelper), nameof(PlumbingNetHelper.EnablesContaminationIncident));
                    continue;
                }
                
                yield return instruction;
            }
        }
    }
}