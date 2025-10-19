using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DubsBadHygiene;
using HarmonyLib;
using Mono.Cecil.Cil;
using Verse;

namespace GravshipStuffForDubsBadHygiene.HarmonyPatches
{
    public class Patcher
    {
        private readonly GravshipStuffForDubsBadHygieneSettings _settings;
        private readonly Harmony _harmony;

        public Patcher(ModContentPack content, GravshipStuffForDubsBadHygieneSettings settings)
        {
            this._settings = settings;
            this._harmony = new Harmony(content.PackageId);
        }
        
        public void PatchAll()
        {
            if (this._settings.patchContaminationEvent)
                this._harmony.CreateClassProcessor(typeof(TowerContaminationIncident_Patch)).Patch();

            if (this._settings.patchWaterTowers) 
                this._harmony.CreateClassProcessor(typeof(WaterStorage_Patch)).Patch();
        }
    }
}