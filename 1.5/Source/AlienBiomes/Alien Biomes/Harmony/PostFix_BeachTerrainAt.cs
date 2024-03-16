using HarmonyLib;
using RimWorld;
using Verse;

namespace AlienBiomes
{
    [HarmonyPatch(typeof(BeachMaker), "BeachTerrainAt")]
    public static class BeachTerrainAt_Patch
    {
        /// <summary>
        /// Checks to see if a biome has vanilla sand on beach areas.
        /// If so, changes the current maps' beach sand to something else.
        /// </summary>
        [HarmonyPostfix]
        public static void ReplaceBeachTerrain(BiomeDef biome, ref TerrainDef __result)
        {
            if (__result == TerrainDefOf.Sand)
            {
                if (biome == ABDefOf.SZ_RadiantPlains && AlienBiomesSettings.UseAlienSand == true)
                {
                    __result = ABDefOf.SZ_SoothingSand;
                }
                else if (biome == ABDefOf.SZ_DeliriousDunes && AlienBiomesSettings.UseAlienSand == true)
                {
                    __result = ABDefOf.SZ_DeliriousSmolderingSand;
                }
            }
        }
    }
}
