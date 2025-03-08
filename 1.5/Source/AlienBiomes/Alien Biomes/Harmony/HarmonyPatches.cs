using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;

namespace AlienBiomes
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        private const string WATER = "Water";
        
        static HarmonyPatches()
        {
            Harmony harmony = new (id: "rimworld.scurvyez.alienbiomes");
            
            harmony.Patch(original: AccessTools.PropertyGetter(typeof(ShaderTypeDef), nameof(ShaderTypeDef.Shader)),
                prefix: new HarmonyMethod(typeof(HarmonyPatches), nameof(ShaderFromAssetBundle)));
            
            harmony.Patch(original: AccessTools.Method(typeof(BeachMaker), nameof(BeachMaker.BeachTerrainAt)),
                postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(ReplaceBeachTerrainPostfix)));
            
            harmony.Patch(original: AccessTools.PropertyGetter(typeof(CompGlower), "ShouldBeLitNow"),
                postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(ShouldBeLitNowPostfix)));
            
            harmony.Patch(original: AccessTools.PropertyGetter(typeof(Plant), "HarvestableNow"),
                postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(HarvestableNowPostFix)));
            
            harmony.Patch(original: AccessTools.Method(typeof(MaterialPool), nameof(MaterialPool.MatFrom),
                    [typeof(MaterialRequest)]),
                postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(MatFromPostFix)));
            
            harmony.Patch(original: AccessTools.Method(typeof(GenStep_Terrain), "TerrainFrom"),
                postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(ReplaceTerrainPostfix)));
            
            harmony.Patch(original: AccessTools.Method(typeof(PlantUtility), nameof(PlantUtility.CanEverPlantAt),
                    [typeof(ThingDef), typeof(IntVec3), typeof(Map), typeof(bool)]),
                prefix: new HarmonyMethod(typeof(HarmonyPatches), nameof(CanEverPlantAtPrefix)));
            
            harmony.Patch(original: AccessTools.Method(typeof(Pawn_PathFollower), "TryEnterNextPathCell"),
                postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(TryEnterNextPathCellPostfix)));
            
            harmony.Patch(original: AccessTools.PropertyGetter(typeof(SectionLayer_TerrainScatter), "Visible"),
                prefix: new HarmonyMethod(typeof(HarmonyPatches), nameof(VisiblePrefix)));
            
            harmony.Patch(original: AccessTools.Method(typeof(RiverMaker), "ValidatePassage"),
                prefix: new HarmonyMethod(typeof(HarmonyPatches), nameof(ValidatePassagePrefix)));
            
            harmony.Patch(original: AccessTools.Method(typeof(World), "NaturalRockTypesIn"),
                postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(NaturalRockTypesInPostfix)));
            
            harmony.Patch(original: AccessTools.Method(typeof(Designator_PlantsCut), "AffectsThing"),
                postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(AffectsThingPostfix)));
            
            //harmony.Patch(original: AccessTools.Method(typeof(Pawn), "DoKillSideEffects"),
            //postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(DoKillSideEffectsPostfix)));
        }
        
        /*
        public static void DoKillSideEffectsPostfix(Pawn __instance, DamageInfo? dinfo, Hediff exactCulprit, bool spawned)
        {
            if (dinfo?.Instigator != null && dinfo.Value.Instigator is Pawn pawn)
            {
                ThoughtDef thought = ThoughtDefOf.AteCorpse;
                pawn.needs.mood.thoughts.memories.TryGainMemory(thought);
                Log.Message($"<color=#4494E3FF>{pawn.Name}</color> has been given the <color=#ff8c66>{thought}</color> thought.");
            }
        }
        */
        
        public static void ShaderFromAssetBundle(ShaderTypeDef __instance, ref Shader ___shaderInt)
        {
            if (__instance is not ABShaderTypeDef) return;
            ___shaderInt = AlienBiomesContentDatabase.AlienBiomesBundle
                .LoadAsset<Shader>(__instance.shaderPath);
            
            if (___shaderInt is null)
            {
                ABLog.Message($"Failed to load Shader from path {__instance.shaderPath}");
            }
        }

        public static void ReplaceBeachTerrainPostfix(BiomeDef biome, ref TerrainDef __result)
        {
            if (!biome.HasModExtension<Biome_Generation_ModExt>()) return;
            Biome_Generation_ModExt ext = biome.GetModExtension<Biome_Generation_ModExt>();
            
            if (ext.newBeachSand != null && __result == TerrainDefOf.Sand)
            {
                __result = ext.newBeachSand;
            }
        }

        public static void ShouldBeLitNowPostfix(CompGlower __instance, ref bool __result)
        {
            if (__instance is Comp_TimedGlower glower) __result = __result
                    && AlienBiomesSettings.ShowPlantGlow && glower.AdditionalGlowerLogic();
        }

        public static void HarvestableNowPostFix(Plant __instance, ref bool __result)
        {
            Comp_TimedHarvest comp = __instance.GetComp<Comp_TimedHarvest>();
            
            if (comp == null) return;
            __result = __result && comp.AdditionalPlantHarvestLogic();
        }
        
        public static void MatFromPostFix(MaterialRequest req, ref Material __result)
        {
            if (__result != null
                && (__result.shader == AlienBiomesContentDatabase.TransparentPlantShimmer
                || __result.shader == AlienBiomesContentDatabase.TransparentPlantPulse
                || __result.shader == AlienBiomesContentDatabase.TransparentPlantFloating
                || __result.shader == AlienBiomesContentDatabase.TransparentPlantPlus))
            {
                WindManager.Notify_PlantMaterialCreated(__result);
            }
        }
        
        public static void ReplaceTerrainPostfix(Map map, ref TerrainDef __result)
        {
            if (!map.Biome.HasModExtension<Biome_Generation_ModExt>()) return;
            Biome_Generation_ModExt ext = map.Biome.GetModExtension<Biome_Generation_ModExt>();
            
            var replacements = new Dictionary<TerrainDef, TerrainDef>
            {
                { TerrainDefOf.Gravel, ext.newGravel },
                { TerrainDefOf.Sand, ext.newSand },
                { TerrainDefOf.WaterShallow, ext.newShallowWater },
                { TerrainDefOf.WaterMovingShallow, ext.newWaterMovingShallow },
                { TerrainDefOf.WaterOceanShallow, ext.newWaterOceanShallow },
                { TerrainDefOf.WaterDeep, ext.newWaterDeep },
                { TerrainDefOf.WaterOceanDeep, ext.newWaterOceanDeep },
                { TerrainDefOf.WaterMovingChestDeep, ext.newWaterMovingChestDeep }
            };
            
            if (replacements.TryGetValue(__result, out TerrainDef newTerrain) && newTerrain != null)
            {
                __result = newTerrain;
            }
        }
        
        public static bool CanEverPlantAtPrefix(ref bool __result, ThingDef plantDef, 
            IntVec3 c, Map map, bool canWipePlantsExceptTree = false)
        {
            if (!c.InBounds(map)) return true;
            TerrainDef terrain = map.terrainGrid.TerrainAt(c);
            Plant_TerrainControl_ModExt terrainExt = terrain.GetModExtension<Plant_TerrainControl_ModExt>();
            Plant_TerrainControl_ModExt plantTerrainExt = plantDef.GetModExtension<Plant_TerrainControl_ModExt>();
            
            if (plantTerrainExt == null && terrain.HasTag(WATER))
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
            
            if (terrainExt != null && plantTerrainExt != null)
            {
                if (!plantTerrainExt.terrainTags.NullOrEmpty())
                {
                    if (terrainExt.terrainTags.NullOrEmpty())
                    {
                        __result = false;
                        return false;
                    }
                    
                    foreach (string terrainTag in terrainExt.terrainTags)
                    {
                        if (plantTerrainExt.terrainTags.Contains(terrainTag)) continue;
                        __result = false;
                        return false;
                    }
                }
                else if (terrain.HasTag(WATER) || terrain.IsWater)
                {
                    __result = true;
                    return true;
                }
            }
            else if (terrainExt == null && terrain.HasTag(WATER))
            {
                __result = false;
                return false;
            }
            
            if (plantTerrainExt == null || plantTerrainExt.terrainTags.NullOrEmpty() ||
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
                Plant_Nastic_ModExt ext = plant.def
                    .GetModExtension<Plant_Nastic_ModExt>();
                
                if (ext == null) continue;
                if (ext.emitFlecks)
                {
                    plant.DrawNasticFlecks();
                }
                
                if (ext.isTouchSensitive)
                {
                    if (ext.isVisuallyReactive)
                    {
                        plant.TouchSensitiveStartTime = GenTicks.TicksGame;
                    }
                    else if (ext.isDamaging && !plant.GasExpelled)
                    {
                        plant.DoNasticExplosion();
                        plant.GasExpelled = true;
                    }
                }
                if (ext.givesHediff)
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
        
        private static bool ValidatePassagePrefix(Map map)
        {
            return map.Biome != ABDefOf.SZ_RadiantPlains
                   && map.Biome != ABDefOf.SZ_CrystallineFlats
                   && map.Biome != ABDefOf.SZ_DeliriousDunes;
        }
        
        public static void NaturalRockTypesInPostfix(
            ref IEnumerable<ThingDef> __result, int tile, World __instance)
        {
            Tile otherTile = __instance.grid[tile];
            
            if (otherTile == null) return;
            BiomeDef biome = otherTile.biome;
            List<ThingDef> list = __result.ToList();
            
            Rand.PushState(tile);
            int num = Rand.RangeInclusive(2, 3);
            Biome_Rocks_ModExt modExt = biome.GetModExtension<Biome_Rocks_ModExt>();
            
            if (modExt == null)
            {
                Rand.PopState();
                return;
            }
            
            if (modExt.allowedRockTypes != null)
            {
                List<ThingDef> allowedRocks = [];
                foreach (ThingDef rockDef in modExt.allowedRockTypes)
                {
                    if (rockDef != null && !list.Contains(rockDef))
                    {
                        allowedRocks.Add(rockDef);
                    }
                }
                
                if (allowedRocks.Any())
                {
                    while (list.Count + allowedRocks.Count > num && list.Count > 0)
                    {
                        list.Remove(list.RandomElement());
                    }
                    list.AddRange(allowedRocks);
                }
            }
            
            if (modExt.disallowedRockTypes != null)
            {
                list.RemoveAll(rockDef => modExt.disallowedRockTypes.Contains(rockDef));
            }
            
            Rand.PopState();
            __result = list;
        }
        
        public static void AffectsThingPostfix(ref bool __result, Thing t)
        {
            if (t is Plant_Bioluminescence) __result = false;
        }
    }
}