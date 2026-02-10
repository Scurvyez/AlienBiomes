using HarmonyLib;
using RimWorld;
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
                postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(CompGlower_ShouldBeLitNowPostfix)));
            
            harmony.Patch(original: AccessTools.PropertyGetter(typeof(Plant), "HarvestableNow"),
                postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(Plant_HarvestableNowPostFix)));
            
            harmony.Patch(original: AccessTools.Method(typeof(Pawn_PathFollower), "TryEnterNextPathCell"),
                postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(Pawn_PathFollower_TryEnterNextPathCellPostfix)));
            
            harmony.Patch(original: AccessTools.PropertyGetter(typeof(SectionLayer_TerrainScatter), "Visible"),
                prefix: new HarmonyMethod(typeof(HarmonyPatches), nameof(SectionLayer_TerrainScatter_VisiblePrefix)));
            
            /*harmony.Patch(original: AccessTools.Method(typeof(MaterialPool), nameof(MaterialPool.MatFrom),
                    [typeof(MaterialRequest)]),
                postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(MatFromPostFix)));*/
        }
        
        public static void CompGlower_ShouldBeLitNowPostfix(CompGlower __instance, ref bool __result)
        {
            if (__instance is Comp_TimedGlower glower) __result = __result
                    && AlienBiomesSettings.ShowPlantGlow 
                    && glower.AdditionalGlowerLogic();
        }

        public static void Plant_HarvestableNowPostFix(Plant __instance, ref bool __result)
        {
            Comp_TimedHarvest comp = __instance.GetComp<Comp_TimedHarvest>();
            if (comp == null) return;
            __result = __result && comp.AdditionalPlantHarvestLogic();
        }
        
        public static void Pawn_PathFollower_TryEnterNextPathCellPostfix(Pawn ___pawn)
        {
            if (___pawn.Map == null || !___pawn.RaceProps.Humanlike) return;
            IntVec3 nextCell = ___pawn.pather.nextCell;
            
            HarmonyPatchesUtil.TryTriggerVisuallyReactivePlants(___pawn, nextCell);
            HarmonyPatchesUtil.TryTriggerExplosivePlants(___pawn, nextCell);
            HarmonyPatchesUtil.TryTriggerHediffGiverPlants(___pawn, nextCell);
        }
        
        public static bool SectionLayer_TerrainScatter_VisiblePrefix(ref bool __result)
        {
            bool showTerrainDebris = AlienBiomesSettings.ShowTerrainDebris;
            __result = showTerrainDebris;
            return false;
        }
        
        // not even using any of the custom plant shaders atm... sadge
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
    }
}