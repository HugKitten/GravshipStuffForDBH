using System.Linq;
using DubsBadHygiene;
using HarmonyLib;
using UnityEngine;

namespace GravshipStuffForDubsBadHygiene.HarmonyPatches
{
    // Patches WaterPumpedPerTick calculating 
    [HarmonyPatchCategory("PlumbingNetPatch")]
    [HarmonyPatch(typeof(PlumbingNet), "TickWater")]
    public class PlumbingNet_Patch
    {
        private static void PostFix(PlumbingNet __instance)
        {
            var num1 = (float) __instance.WaterTowers.Count(WaterStorageHelper.IsAccepting);
            var num2 = Mathf.Min(__instance.GroundWaterCapacitySum, __instance.PumpingCapacitySum);
            if (num1 <= 0.0 || num2 <= 0.0)
                return;
            __instance.WaterPumpedPerTick = num2 / num1 / 60000f;
        }
        
        // Too much of a headache ;-;
        /*
        private static readonly Type PlumbingNetType = typeof(PlumbingNet);
        private static readonly Type WaterStorageHelperType = typeof(WaterStorageHelper);
        private static readonly Type WaterStoragePropsType = typeof(CompProperties_WaterStorage);
        private static readonly MethodInfo WaterStoragePropsMethod = AccessTools.Method(WaterStoragePropsType, nameof(CompWaterStorage.Props));

        private static IEnumerable<CodeInstruction> Transpiler(
            IEnumerable<CodeInstruction> instructions )
        {
            var codeMatcher = new CodeMatcher(instructions);
            codeMatcher.MatchStartForward(CodeMatch.Calls(WaterStoragePropsMethod))
                .RemoveInstruction()
                .Insert(new CodeInstruction( OpCodes.Dup ));

            codeMatcher.MatchEndForward(CodeMatch.LoadsField(AccessTools.Field(WaterStoragePropsType,
                    nameof(CompProperties_WaterStorage.WaterStorageCap))))
                .RemoveInstruction()
                .Insert(CodeInstruction.Call(WaterStorageHelperType, nameof(WaterStorageHelper.GetWaterStorageCap)));
            
            codeMatcher.MatchEndForward(CodeMatch.LoadsField(AccessTools.Field(WaterStoragePropsType,
                    nameof(CompProperties_WaterStorage.AutoGenRate))))
                .RemoveInstruction()
                .Insert(CodeInstruction.Call(WaterStorageHelperType, nameof(WaterStorageHelper.GetAutoGenRate)));
            
            codeMatcher.MatchEndForward(CodeMatch.LoadsField(AccessTools.Field(WaterStoragePropsType,
                    nameof(CompProperties_WaterStorage.Tickrate))))
                .RemoveInstruction()
                .Insert(CodeInstruction.Call(WaterStorageHelperType, nameof(WaterStorageHelper.GetTickRate)));
            
            codeMatcher.MatchEndForward(CodeMatch.LoadsField(AccessTools.Field(WaterStoragePropsType,
                    nameof(CompProperties_WaterStorage.AutoGenRate))))
                .RemoveInstruction()
                .Insert(CodeInstruction.Call(WaterStorageHelperType, nameof(WaterStorageHelper.GetAutoGenRate)));
            
            codeMatcher.MatchEndForward(CodeMatch.LoadsField(AccessTools.Field(WaterStoragePropsType,
                    nameof(CompProperties_WaterStorage.AutoOnRain))))
                .RemoveInstruction()
                .Insert(CodeInstruction.Call(WaterStorageHelperType, nameof(WaterStorageHelper.GetAutoOnRain)));

            return codeMatcher.Instructions();
        }
        */
    }
}