using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DubsBadHygiene;
using HarmonyLib;
using Verse;

namespace GravshipStuffForDubsBadHygiene.HarmonyPatches
{
    /// <summary>
    /// Patches water storage to show proper inspect data and use the correct storage cap
    /// </summary>
    [HarmonyPatchCategory("WaterStorage")]
    [HarmonyPatch]
    public class WaterStorage_Patch
    {
        private static readonly Assembly ThisAssembly = Assembly.GetExecutingAssembly();
        
        private static float GetWaterStorageCap(CompProperties_WaterStorage props, CompWaterStorage storage) => storage is CompPoweredWaterStorage pws ?
            pws.WaterStorageCap :
            props.WaterStorageCap;
        
        static IEnumerable<MethodBase> TargetMethods() =>
            AccessTools.AllTypes()
                .Where(t => t.Assembly != ThisAssembly)
                .Where(t => typeof(CompWaterStorage).IsAssignableFrom(t))
                .SelectMany(t => t.GetMethods(BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.Instance |
                                              BindingFlags.NonPublic | BindingFlags.Public |
                                              BindingFlags.SetProperty | BindingFlags.DeclaredOnly)).Cast<MethodBase>();

        private static IEnumerable<CodeInstruction> Transpiler(MethodBase methodBase, 
            IEnumerable<CodeInstruction> instructions ) =>
            new CodeMatcher(instructions)
                .MatchStartForward(
                    CodeMatch.Calls(AccessTools.PropertyGetter(typeof(PlumbingNet), nameof(PlumbingNet.HasFilter))))
                .Repeat(cm => cm.SetInstructionAndAdvance(CodeInstruction.Call(() => default(PlumbingNet).IsTreated())))
                .Start()
                .MatchStartForward(CodeMatch.LoadsField(AccessTools.Field(typeof(CompProperties_WaterStorage),
                    nameof(CompProperties_WaterStorage.WaterStorageCap))))
                .Repeat(cm =>
                {
                    cm.SetInstructionAndAdvance(CodeInstruction.LoadArgument(0));
                    cm.InsertAndAdvance(CodeInstruction.Call(typeof(WaterStorage_Patch), nameof(GetWaterStorageCap)));
                })
                .InstructionEnumeration();
    }
}