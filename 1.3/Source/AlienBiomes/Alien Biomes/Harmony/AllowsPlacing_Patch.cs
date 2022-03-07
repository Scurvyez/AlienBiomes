using HarmonyLib;
using RimWorld;
using System;
using Verse;

namespace AlienBiomes
{
    [HarmonyPatch(typeof(PlaceWorker_OnSteamGeyser), "AllowsPlacing")]
    public static class AllowsPlacing_Patch
    {
        [HarmonyPostfix]
        public static AcceptanceReport Postfix(AcceptanceReport __result, Map map, IntVec3 loc)
        {
            if (!__result)
            {
                var geyser = map.thingGrid.ThingAt(loc, AlienBiomes_NaturalBuildingDefOf.SZ_SteamGeyserRadiantSoil);
                if (geyser != null && geyser.Position == loc)
                    return true;
            }
            return __result;
        }
    }
}
