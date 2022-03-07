using HarmonyLib;
using RimWorld;
using Verse;

namespace AlienBiomes
{
    [HarmonyPatch(typeof(Building_SteamGeyser), "SpawnSetup")]
    public static class SteamGeyserSpawnSetup_Patch
    {
        /// <summary>
        /// Checks the map for vanilla steam geysers.
        /// If any are present on radiant or rich radiant soil, they are replaced.
        /// </summary>
        [HarmonyPostfix]
        public static void SpawnSetupUpdateGeysers(Map map, bool respawningAfterLoad, Building_SteamGeyser __instance)
        {
            var steamGeyserOne = AlienBiomes_NaturalBuildingDefOf.SZ_SteamGeyserRadiantSoil;
            var steamGeyserTwo = AlienBiomes_NaturalBuildingDefOf.SZ_SteamGeyserRadiantRichSoil;
            var terrainOne = AlienBiomes_TerrainDefOf.SZ_RadiantSoil;
            var terrainTwo = AlienBiomes_TerrainDefOf.SZ_RadiantRichSoil;
            var vanillaGeyserPos = __instance.Position;

            // Checks to see if the given terrain at the vanilla geysers' pos is "radiant soil".
            if (map.terrainGrid.TerrainAt(vanillaGeyserPos) == terrainOne)
            {
                if ((__instance is Building_SteamGeyser))
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

            else if (map.terrainGrid.TerrainAt(vanillaGeyserPos) == terrainTwo)
            {
                if ((__instance is Building_SteamGeyser))
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
