using HarmonyLib;
using RimWorld;
using Verse;
using System.Collections.Generic;

namespace AlienBiomes
{
    /*
    [HarmonyPatch(typeof(TerrainDefGenerator_Stone), "ImpliedTerrainDefs")]
    public static class ImpliedTerrainDefs_Patch
    {
        /// <summary>
        /// Adds the BiomePlantControl modExtension to all generated stone TerrainDef's. 
        /// Also gives the TerrainDef's some fertility (water terrain as well). This allows plants to grow on these terrains.
        /// </summary>
        [HarmonyPostfix]
        public static IEnumerable<TerrainDef> AddBiomesCoreDefModExtensions(IEnumerable<TerrainDef> __result)
        {
            foreach (var terrainDef in __result)
            {
                BiomePlantControl plantControl = new ();
                plantControl.terrainTags.Add("Stony");
                plantControl.terrainTags.Add("Rocky");
                if (terrainDef.modExtensions == null)
                {
                    terrainDef.modExtensions = new List<DefModExtension>();
                }

                terrainDef.modExtensions.Add(plantControl);
                terrainDef.fertility = 0.35f;

                // check to make sure tags are being applied to each terrain correctly
                //string tags = string.Join(", ", plantControl.terrainTags);
                //Log.Message($"<color=#4494E3FF>AlienBiomesDebug: </color>{terrainDef.label} - {tags}");

                yield return terrainDef;
            }
        }
    }
    */
}
