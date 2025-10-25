using System.Linq;
using DubsBadHygiene;
using HarmonyLib;
using UnityEngine;

namespace GravshipStuffForDubsBadHygiene.HarmonyPatches
{
    // Patches WaterPumpedPerTick calculating 
    [HarmonyPatchCategory("PlumbingNetPatch")]
    [HarmonyPatch(typeof(PlumbingNet), "TickWater")]
    public class PlumbingNet_TickWWater_Patch
    {
        private static void PostFix(PlumbingNet __instance)
        {
            CompWaterInletCryo
            // Get max water that can be pumped
            var maxWaterPump = Mathf.Min(__instance.GroundWaterCapacitySum, __instance.PumpingCapacitySum);
            if (maxWaterPump <= 0.0)
            {
                __instance.WaterPumpedPerTick = 0F;
                return;
            }
            
            // Get total tanks to pump water to
            var awaitingTanks = Enumerable.Count(__instance.WaterTowers, ws => !ws.IsFull());
            if (awaitingTanks <= 0)
            {
                __instance.WaterPumpedPerTick = 0F;
                return;
            }

            // Get total water pump per tower
            var waterPumpedPerTowerPerDay = maxWaterPump / awaitingTanks;
            
            // Set total pumped water per tick
            __instance.WaterPumpedPerTick = waterPumpedPerTowerPerDay / 60_000F;
        }
    }
}