using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DubsBadHygiene;
using HarmonyLib;
using Verse;

namespace GravshipStuffForDubsBadHygiene.HarmonyPatches
{
    public class Patcher
    {
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> HasFilterToEnablesContaminationIncidentTranspiler(
            IEnumerable<CodeInstruction> instructions )
        {
            foreach (var instruction in instructions)
            {
                if (instruction.Calls(AccessTools.PropertyGetter(nameof(PlumbingNet.HasFilter))))
                {
                    yield return CodeInstruction.Call(nameof(PlumbingNetHelper.EnablesContaminationIncident));
                    continue;
                }
                
                yield return instruction;
            }
        }
        
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> HasFilterToIsTreatedTranspiler(
            IEnumerable<CodeInstruction> instructions )
        {
            foreach (var instruction in instructions)
            {
                if (instruction.Calls(AccessTools.PropertyGetter(nameof(PlumbingNet.HasFilter))))
                {
                    yield return CodeInstruction.Call(nameof(PlumbingNetHelper.IsTreated));
                    continue;
                }
                
                yield return instruction;
            }
        }
        
        private readonly Harmony _harmony;
        private readonly Assembly _gravshipStuff;
        private readonly Assembly _dubs;
        private readonly HarmonyMethod _contaminationIncidentTranspiler;
        private readonly HarmonyMethod _wellFixTranspiler;

        public Patcher(ModContentPack content)
        {
            this._harmony = new Harmony(content.PackageId);
            this._gravshipStuff = Assembly.GetExecutingAssembly();
            this._dubs = Assembly.GetAssembly(typeof(DubsBadHygieneMod));
            this._contaminationIncidentTranspiler = new HarmonyMethod(AccessTools.Method(nameof(HasFilterToEnablesContaminationIncidentTranspiler)));
            this._wellFixTranspiler = new HarmonyMethod(AccessTools.Method(nameof(HasFilterToIsTreatedTranspiler)));
        }
        
        public void PatchAll()
        {
            var settings = LoadedModManager.GetMod<GravshipStuffForDubsBadHygieneMod>()
                .GetSettings<GravshipStuffForDubsBadHygieneSettings>();
            
            if (settings.patchContaminationEvent)
                PatchContaminationIncident();

            if (settings.patchWaterTowers) 
                PatchWaterTowers();
        }

        private void PatchContaminationIncident()
        {
            foreach (var processor in AccessTools.GetDeclaredMethods(AccessTools.TypeByName("DubsBadHygiene.IncidentWorker_TowerContamination"))
                         .Select(method => _harmony.CreateProcessor(method)))
            {
                processor.AddTranspiler(_contaminationIncidentTranspiler);
                processor.Patch();
            }
        }

        private void PatchWaterTowers()
        {
            var waterStorageType = typeof(CompWaterStorage);
            foreach (var type in AccessTools.AllTypes())
            {
                if (type.Assembly == this._gravshipStuff)
                    continue;

                if (waterStorageType.IsAssignableFrom(type))
                {
                    foreach (var processor in AccessTools.GetDeclaredMethods(type)
                                 .Select(method => _harmony.CreateProcessor(method)))
                    {
                        processor.AddTranspiler(_wellFixTranspiler);
                        processor.Patch();
                    }
                }
            }
        }
    }
}