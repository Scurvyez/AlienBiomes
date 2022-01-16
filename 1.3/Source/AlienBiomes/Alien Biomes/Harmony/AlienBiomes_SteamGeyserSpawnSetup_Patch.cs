using HarmonyLib;
using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace AlienBiomes
{
    // This patch replaces vanilla steam geyser textures on map gen.

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
                    // Allows for geysers to be destroyed.
                    Thing.allowDestroyNonDestroyable = true;
                    if (!__instance.Destroyed)
                    {
                        // Destroy old geyser.
                        __instance.Destroy(DestroyMode.Vanish);
                        // Spawn new geyser, in same cell.
                        Thing newGeyserOne = ThingMaker.MakeThing(steamGeyserOne);
                        GenPlace.TryPlaceThing(newGeyserOne, vanillaGeyserPos, map, ThingPlaceMode.Direct);
                    }
                    // Disallows geyser destroying.
                    Thing.allowDestroyNonDestroyable = false;
                }
            }

            else if (map.terrainGrid.TerrainAt(vanillaGeyserPos) == AlienBiomes_TerrainDefOf.SZ_RadiantRichSoil)
            {
                if (!(__instance is Building_SteamGeyserRadiantRichSoil))
                {
                    Thing.allowDestroyNonDestroyable = true;
                    if (!__instance.Destroyed)
                    {
                        __instance.Destroy(DestroyMode.Vanish);
                        Thing newGeyserTwo = ThingMaker.MakeThing(steamGeyserTwo);
                        GenPlace.TryPlaceThing(newGeyserTwo, vanillaGeyserPos, map, ThingPlaceMode.Direct);
                    }
                    Thing.allowDestroyNonDestroyable = false;
                }
            }
        }
    }
}
