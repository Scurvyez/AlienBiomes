using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace AlienBiomes
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            Harmony harmony = new (id: "rimworld.scurvyez.alienbiomes");
            
            harmony.Patch(original: AccessTools.PropertyGetter(typeof(CompGlower), "ShouldBeLitNow"),
                postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(ShouldBeLitNowPostfix)));
            
            harmony.Patch(original: AccessTools.PropertyGetter(typeof(Plant), "HarvestableNow"),
                postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(HarvestableNowPostFix)));
            
            //harmony.Patch(original: AccessTools.Method(typeof(MaterialPool), nameof(MaterialPool.MatFrom),
                    //[typeof(MaterialRequest)]),
                //postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(MatFromPostFix)));
            
            harmony.Patch(original: AccessTools.Method(typeof(PlantUtility), nameof(PlantUtility.CanEverPlantAt),
                    [typeof(ThingDef), typeof(IntVec3), typeof(Map), typeof(bool), typeof(bool)]),
                prefix: new HarmonyMethod(typeof(HarmonyPatches), nameof(CanEverPlantAtPrefix)));
            
            harmony.Patch(original: AccessTools.Method(typeof(Pawn_PathFollower), "TryEnterNextPathCell"),
                postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(TryEnterNextPathCellPostfix)));
            
            harmony.Patch(original: AccessTools.PropertyGetter(typeof(SectionLayer_TerrainScatter), "Visible"),
                prefix: new HarmonyMethod(typeof(HarmonyPatches), nameof(VisiblePrefix)));
        }
        
        public static void ShouldBeLitNowPostfix(CompGlower __instance, ref bool __result)
        {
            if (__instance is Comp_TimedGlower glower) __result = __result
                    && AlienBiomesSettings.ShowPlantGlow 
                    && glower.AdditionalGlowerLogic();
        }

        public static void HarvestableNowPostFix(Plant __instance, ref bool __result)
        {
            Comp_TimedHarvest comp = __instance.GetComp<Comp_TimedHarvest>();
            if (comp == null) return;
            __result = __result 
                       && comp.AdditionalPlantHarvestLogic();
        }
        
        // same as above...
        // we're not even using any of the custom shaders anymore... sadge
        // we may in the future so keep this!
        /*public static void MatFromPostFix(MaterialRequest req, ref Material __result)
        {
            if (__result != null
                && (__result.shader == AlienBiomesContentDatabase.TransparentPlantShimmer
                || __result.shader == AlienBiomesContentDatabase.TransparentPlantPulse
                || __result.shader == AlienBiomesContentDatabase.TransparentPlantFloating
                || __result.shader == AlienBiomesContentDatabase.TransparentPlantPlus))
            {
                WindManager.Notify_PlantMaterialCreated(__result);
            }
        }*/
        
        public static bool CanEverPlantAtPrefix(ref bool __result, ThingDef plantDef, 
            IntVec3 c, Map map, bool canWipePlantsExceptTree = false)
        {
            if (!c.InBounds(map)) return true;
            TerrainDef terrain = map.terrainGrid.TerrainAt(c);
            ModExt_PlantTerrainControl terrainExt = terrain.GetModExtension<ModExt_PlantTerrainControl>();
            ModExt_PlantTerrainControl modExtPlantTerrainExt = plantDef.GetModExtension<ModExt_PlantTerrainControl>();
            
            if (modExtPlantTerrainExt == null && terrain.HasTag(HarmonyPatchesUtil.WATER))
            {
                __result = false;
                return false;
            }
            
            foreach (Thing item in map.thingGrid.ThingsListAt(c))
            {
                if (item?.def.building == null 
                    || !plantDef.plant.sowTags.Contains(item.def.building.sowTag)) continue;
                __result = plantDef.plant.sowTags.Contains(item.def.building.sowTag);
                return plantDef.plant.sowTags.Contains(item.def.building.sowTag);
            }
            
            if (terrainExt != null && modExtPlantTerrainExt != null)
            {
                if (!modExtPlantTerrainExt.terrainTags.NullOrEmpty())
                {
                    if (terrainExt.terrainTags.NullOrEmpty())
                    {
                        __result = false;
                        return false;
                    }
                    
                    foreach (string terrainTag in terrainExt.terrainTags)
                    {
                        if (modExtPlantTerrainExt.terrainTags.Contains(terrainTag)) continue;
                        __result = false;
                        return false;
                    }
                }
                else if (terrain.HasTag(HarmonyPatchesUtil.WATER) || terrain.IsWater)
                {
                    __result = true;
                    return true;
                }
            }
            else if (terrainExt == null && terrain.HasTag(HarmonyPatchesUtil.WATER))
            {
                __result = false;
                return false;
            }
            
            if (modExtPlantTerrainExt == null || modExtPlantTerrainExt.terrainTags.NullOrEmpty() ||
                (terrainExt != null && !terrainExt.terrainTags.NullOrEmpty())) return true;
            __result = false;
            return false;
        }
        
        public static void TryEnterNextPathCellPostfix(Pawn ___pawn)
        {
            if (___pawn.Map == null || !___pawn.RaceProps.Humanlike) return;
            IntVec3 nextCell = ___pawn.pather.nextCell;
            MapComponent_PlantGetter plantGetter = ___pawn.Map
                .GetComponent<MapComponent_PlantGetter>();
            
            if (plantGetter == null) return;
            if (!plantGetter.ActiveLocationTriggers
                    .TryGetValue(nextCell, out HashSet<Plant_Nastic> plantsInCell)) 
                return;
            
            foreach (Plant_Nastic plant in plantsInCell)
            {
                ModExt_PlantNastic ext = plant.def
                    .GetModExtension<ModExt_PlantNastic>();
                
                if (ext == null) continue;
                if (ext.isTouchSensitive)
                {
                    if (ext.isVisuallyReactive &&
                        plant.Growth >= ext.visuallyReactiveThreshold)
                    {
                        plant.TouchSensitiveStartTime = GenTicks.TicksGame;
                        plant.TryDoNasticSfx(plant);
                        plant.TryDrawNasticFlecks();
                    }
                    
                    if (ext.isDamaging && !plant.GasExpelled &&
                        plant.Growth >= ext.explosionGrowthThreshold)
                    {
                        plant.TryDoNasticExplosion();
                        plant.GasExpelled = true;
                    }
                }
                if (ext.hediffToGive != null &&
                    plant.Growth >= ext.givesHediffGrowthThreshold &&
                    !___pawn.health.hediffSet.HasHediff(ext.hediffToGive))
                {
                    plant.TryGiveNasticHediff(___pawn);
                }
            }
        }
        
        public static bool VisiblePrefix(ref bool __result)
        {
            bool showTerrainDebris = AlienBiomesSettings.ShowTerrainDebris;
            __result = showTerrainDebris;
            return false;
        }
    }
}