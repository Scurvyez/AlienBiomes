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
                // spawn new steam geyser
                Thing thing = ThingMaker.MakeThing(steamGeyserOne);
                GenPlace.TryPlaceThing(thing, __instance.Position, map, ThingPlaceMode.Direct);
            }
        }
    }
}
