using System.Collections.Generic;
using System.Reflection;
using DubsBadHygiene;
using HarmonyLib;

namespace GravshipStuffForDubsBadHygiene.HarmonyPatches
{
    [HarmonyPatch]
    public class WaterStorage_Patch
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            var thisAssembly  = Assembly.GetExecutingAssembly();
            
            var waterStorageType = typeof(CompWaterStorage);
            foreach (var type in AccessTools.AllTypes())
            {
                if (type.Assembly == thisAssembly)
                    continue;

                if (!waterStorageType.IsAssignableFrom(type)) 
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
                if (instruction.Calls(AccessTools.PropertyGetter(typeof(PlumbingNet), nameof(PlumbingNet.HasFilter))))
                {
                    yield return CodeInstruction.Call(typeof(PlumbingNetHelper), nameof(PlumbingNetHelper.IsTreated));
                    continue;
                }
                
                yield return instruction;
            }
        }
    }
}