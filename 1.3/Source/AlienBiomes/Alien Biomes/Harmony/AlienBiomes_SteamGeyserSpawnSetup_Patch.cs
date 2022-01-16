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

            var steamGeyserOne = ThingDef.Named("SZ_SteamGeyserRadiantSoil");
            var steamGeyserTwo = ThingDef.Named("SZ_SteamGeyserRadiantRichSoil");
            var vanillaGeyserPos = __instance.Position;

            // Checks to see if the given terrain at the vanilla geysers' pos is "radiant soil".
            if (map.terrainGrid.TerrainAt(vanillaGeyserPos) == AlienBiomes_TerrainDefOf.SZ_RadiantSoil)
            {
                if (!(__instance is Building_SteamGeyserRadiantSoil))
                {
                    if (!__instance.Destroyed)
                    {
                        // Destroy old geyser.
                        __instance.Destroy(DestroyMode.Vanish);
                        // Spawn new geyser, in same cell.
                        Thing newGeyser = ThingMaker.MakeThing(steamGeyserOne);
                        GenPlace.TryPlaceThing(newGeyser, vanillaGeyserPos, map, ThingPlaceMode.Direct);
                    }
                }
            }
        }
    }
}
