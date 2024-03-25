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
            if (biome.HasModExtension<BiomeControls>())
            {
                BiomeControls ext = biome.GetModExtension<BiomeControls>();
                if (ext.newBeachSand != null && __result == TerrainDefOf.Sand)
                {
                    __result = ext.newBeachSand;
                }
            }
        }
    }
}
