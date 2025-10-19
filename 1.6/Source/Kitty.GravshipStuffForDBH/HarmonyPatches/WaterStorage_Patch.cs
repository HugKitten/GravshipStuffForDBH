using System;
using System.Collections.Generic;
using System.Reflection;
using DubsBadHygiene;
using HarmonyLib;

namespace GravshipStuffForDubsBadHygiene.HarmonyPatches
{
    [HarmonyPatchCategory("WaterStorage")]
    [HarmonyPatch]
    public class WaterStorage_Patch
    {
        private static readonly Type WaterStorageType =
            AccessTools.TypeByName("DubsBadHygiene.CompWaterStorage");
        private static readonly Type PlumbingNetType = AccessTools.TypeByName("DubsBadHygiene.PlumbingNet");
        private static readonly MethodInfo HasFilterMethod = AccessTools.PropertyGetter(PlumbingNetType, "HasFilter");
        private static readonly CodeInstruction EnablesContaminationIncidentCall = CodeInstruction.Call(typeof(PlumbingNetHelper),
            nameof(PlumbingNetHelper.IsTreated));
        
        static IEnumerable<MethodBase> TargetMethods()
        {
            var thisAssembly  = Assembly.GetExecutingAssembly();

            foreach (var type in AccessTools.AllTypes())
            {
                if (type.Assembly == thisAssembly)
                    continue;

                if (!WaterStorageType.IsAssignableFrom(type)) 
                    continue;
                
                foreach (var method in AccessTools.GetDeclaredMethods(type))
                    yield return method;
            }
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