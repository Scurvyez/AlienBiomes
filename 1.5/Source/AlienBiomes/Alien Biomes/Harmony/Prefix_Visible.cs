using HarmonyLib;
using Verse;

namespace AlienBiomes
{
    [HarmonyPatch(typeof(SectionLayer_TerrainScatter), "Visible", MethodType.Getter)]
    public static class SectionLayerTerrainScatterVisible_Patch
    {
        [HarmonyPrefix]
	    public static bool Prefix(ref bool __result)
	    {
            bool showTerrainDebris = AlienBiomesSettings.ShowTerrainDebris;
            __result = showTerrainDebris;
		    return false;
	    }
    }
}
