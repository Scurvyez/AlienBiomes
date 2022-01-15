using HarmonyLib;
using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace AlienBiomes
{
    [HarmonyPatch(typeof(Building_SteamGeyser), "SpawnSetup")]
    public static class AlienBiomes_SteamGeyserSpawnSetup_Patch
    {
        [HarmonyPostfix]
        public static void SpawnSetupUpdateGeysers(Map map, bool respawningAfterLoad, Building_SteamGeyser __instance)
        {

            var steamGeyserOne = ThingDef.Named("SZ_SteamGeyserEnlightenedSoil");
            var steamGeyserTwo = ThingDef.Named("SZ_SteamGeyserEnlightenedRichSoil");

            // checks to see if the given terrain at the vanilla geysers' pos is "enlightened soil"
            if (map.terrainGrid.TerrainAt(__instance.Position) == AlienBiomes_TerrainDefOf.SZ_EnlightenedSoil)
            {
                if (!__instance.Destroyed)
                {
                    // destroy old geyser
                    __instance.Destroy(DestroyMode.Vanish);
                    // spawn new geyser
                    Thing newGeyser = ThingMaker.MakeThing(steamGeyserOne);
                    GenPlace.TryPlaceThing(newGeyser, __instance.Position, map, ThingPlaceMode.Direct);
                }
            }
        }
    }
}
