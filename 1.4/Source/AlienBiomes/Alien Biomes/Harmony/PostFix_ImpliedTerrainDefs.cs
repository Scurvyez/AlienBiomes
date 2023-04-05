using HarmonyLib;
using RimWorld;
using Verse;
using System.Collections.Generic;
using BiomesCore.DefModExtensions;

namespace AlienBiomes
{
    [HarmonyPatch(typeof(TerrainDefGenerator_Stone), "ImpliedTerrainDefs")]
    public static class ImpliedTerrainDefs_Patch
    {
        /// <summary>
        /// Adds the BiomesCore.DefModExtensions.Biomes_PlantControl modExtension
        /// to all generated stone TerrainDef's. Also give the TerrainDef's some fertility.
        /// This allows plants to grow on these terrains.
        /// </summary>
        [HarmonyPostfix]
        public static IEnumerable<TerrainDef> AddBiomesCoreDefModExtensions(IEnumerable<TerrainDef> __result)
        {
            foreach (var terrainDef in __result)
            {
                Biomes_PlantControl plantControl = new ();
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
}
