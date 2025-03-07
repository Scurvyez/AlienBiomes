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
                    new[] { typeof(MaterialRequest) }),
                postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(MatFromPostFix)));

            harmony.Patch(original: AccessTools.Method(typeof(GenStep_Terrain), "TerrainFrom"),
                postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(ReplaceTerrainPostfix)));

            harmony.Patch(original: AccessTools.Method(typeof(PlantUtility), nameof(PlantUtility.CanEverPlantAt), 
                    new [] { typeof(ThingDef), typeof(IntVec3), typeof(Map), typeof(bool) }),
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
            if (!biome.HasModExtension<BiomeControls>()) return;
            BiomeControls ext = biome.GetModExtension<BiomeControls>();
            
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
            if (!map.Biome.HasModExtension<BiomeControls>()) return;
            BiomeControls ext = map.Biome.GetModExtension<BiomeControls>();

            if (ext.newGravel != null && __result == TerrainDefOf.Gravel)
            {
                __result = ext.newGravel;
            }
            
            if (ext.newSand != null && __result == TerrainDefOf.Sand)
            {
                __result = ext.newSand;
            }

            if (ext.newShallowWater != null && __result == TerrainDefOf.WaterShallow)
            {
                __result = ext.newShallowWater;
            }
            else if (ext.newWaterMovingShallow != null && __result == TerrainDefOf.WaterMovingShallow)
            {
                __result = ext.newWaterMovingShallow;
            }
            else if (ext.newWaterOceanShallow != null && __result == TerrainDefOf.WaterOceanShallow)
            {
                __result = ext.newWaterOceanShallow;
            }
            else if (ext.newWaterDeep != null && __result == TerrainDefOf.WaterDeep)
            {
                __result = ext.newWaterDeep;
            }
            else if (ext.newWaterOceanDeep != null && __result == TerrainDefOf.WaterOceanDeep)
            {
                __result = ext.newWaterOceanDeep;
            }
            else if (ext.newWaterMovingChestDeep != null && __result == TerrainDefOf.WaterMovingChestDeep)
            {
                __result = ext.newWaterMovingChestDeep;
            }
        }

        public static bool CanEverPlantAtPrefix(ref bool __result, ThingDef plantDef, IntVec3 c, Map map, bool canWipePlantsExceptTree = false)
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
                if (item?.def.building == null || !plantDef.plant.sowTags.Contains(item.def.building.sowTag)) continue;
                __result = plantDef.plant.sowTags.Contains(item.def.building.sowTag);
                return plantDef.plant.sowTags.Contains(item.def.building.sowTag);
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
                        if (plantExt.terrainTags.Contains(terrainTag)) continue;
                        __result = false;
                        return false;
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

            if (plantExt == null || plantExt.terrainTags.NullOrEmpty() ||
                (terrainExt != null && !terrainExt.terrainTags.NullOrEmpty())) return true;
            __result = false;
            return false;
        }

        public static void TryEnterNextPathCellPostfix(Pawn ___pawn)
        {
            if (___pawn.Map == null || !___pawn.RaceProps.Humanlike)
                return;

            IntVec3 nextCell = ___pawn.pather.nextCell;
            MapComponent_PlantGetter plantGetter = ___pawn.Map
                .GetComponent<MapComponent_PlantGetter>();

            if (plantGetter == null)
                return;

            if (!plantGetter.ActiveLocationTriggers
                    .TryGetValue(nextCell, out HashSet<Plant_Nastic> plantsInCell))
                return;
            
            foreach (Plant_Nastic plant in plantsInCell)
            {
                PlantNastic_ModExtension plantExt = plant.def
                    .GetModExtension<PlantNastic_ModExtension>();
                
                if (plantExt == null)
                    continue;

                if (plantExt.emitFlecks)
                {
                    plant.DrawEffects();
                }

                if (plantExt.isTouchSensitive)
                {
                    if (plantExt.isVisuallyReactive)
                    {
                        plant.TouchSensitiveStartTime = GenTicks.TicksGame;
                    }
                    else if (plantExt.isDamaging && !plant.GasExpelled)
                    {
                        plant.DoExplosion();
                        plant.GasExpelled = true;
                    }
                }
                if (plantExt.givesHediff)
                {
                    plant.GiveHediff(___pawn);
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
        
        public static void NaturalRockTypesInPostfix(ref IEnumerable<ThingDef> __result, int tile, World __instance)
        {
            Tile tile2 = __instance.grid[tile];
            
            if (tile2 == null) return;
            BiomeDef biome = tile2.biome;
            List<ThingDef> list = __result.ToList();
            
            Rand.PushState(tile);
            int num = Rand.RangeInclusive(2, 3);
            Biome_Rocks_ModExtension modExt = biome.GetModExtension<Biome_Rocks_ModExtension>();
            
            if (modExt == null)
            {
                Rand.PopState();
                return;
            }

            if (modExt.allowedRockTypes != null)
            {
                List<ThingDef> allowedRocks = new List<ThingDef>();
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