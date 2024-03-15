using HarmonyLib;
using RimWorld;
using Verse;
using System;

namespace AlienBiomes
{
    [HarmonyPatch(typeof(PlantUtility), "CanEverPlantAt", new Type[] { typeof(ThingDef), typeof(IntVec3), typeof(Map), typeof(bool) })]
    public static class PlantUtilityCanEverPlantAt_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(ref bool __result, ThingDef plantDef, IntVec3 c, Map map, bool canWipePlantsExceptTree = false)
        {
            if (!c.InBounds(map))
            {
                return true;
            }

            TerrainDef terrain = map.terrainGrid.TerrainAt(c);
            BiomePlantControl terrainExt = terrain.GetModExtension<BiomePlantControl>();
            BiomePlantControl plantExt = plantDef.GetModExtension<BiomePlantControl>();

            if (plantExt == null && terrain.HasTag("Water"))
            {
                __result = false;
                return false;
            }

            foreach (Thing item in map.thingGrid.ThingsListAt(c))
            {
                if (item?.def.building != null && plantDef.plant.sowTags.Contains(item.def.building.sowTag))
                {
                    __result = plantDef.plant.sowTags.Contains(item.def.building.sowTag);
                    return plantDef.plant.sowTags.Contains(item.def.building.sowTag);
                }
            }

            if (terrainExt != null && plantExt != null)
            {
                if (!plantExt.terrainTags.NullOrEmpty())
                {
                    if (terrainExt.terrainTags.NullOrEmpty())
                    {
                        __result = false;
                        return false;
                    }
                    foreach (string terrainTag in terrainExt.terrainTags)
                    {
                        if (!plantExt.terrainTags.Contains(terrainTag))
                        {
                            __result = false;
                            return false;
                        }
                    }
                }
                else if (terrain.HasTag("Water") || terrain.IsWater)
                {
                    __result = true;
                    return true;
                }
            }
            else if (terrainExt == null && terrain.HasTag("Water"))
            {
                __result = false;
                return false;
            }

            if (plantExt != null && !plantExt.terrainTags.NullOrEmpty() && (terrainExt == null || terrainExt.terrainTags.NullOrEmpty()))
            {
                __result = false;
                return false;
            }

            return true;
        }
    }
}
