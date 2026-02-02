using System.Collections.Generic;
using Verse;

namespace AlienBiomes
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatchesUtil
    {
        public const string WATER = "Water";

        public static void TryTriggerVisuallyReactivePlants(Pawn pawn, IntVec3 cell)
        {
            var plantGetterTouchSensitive = pawn.Map.GetComponent<MapComponent_PlantGetter_VisuallyReactive>();
            
            if (plantGetterTouchSensitive == null) return;
            if (!plantGetterTouchSensitive.ActiveLocationTriggers
                .TryGetValue(cell, out HashSet<Plant_VisuallyReactive> plantsInCell)
                || plantsInCell == null
                || plantsInCell.Count == 0)
            {
                return;
            }
            
            foreach (Plant_VisuallyReactive plant in plantsInCell)
            {
                plant.TouchSensitiveStartTime = GenTicks.TicksGame;

                var soundEmanateComp = plant.TryGetComp<Comp_SoundEmanate>();
                soundEmanateComp?.TryPlayTriggeredSound();
            }
        }
        
        public static void TryTriggerHediffGiverPlants(Pawn pawn, IntVec3 cell)
        {
            var plantGetterExplosive = pawn?.Map?.GetComponent<MapComponent_PlantGetter_HediffGiver>();
            
            if (plantGetterExplosive == null) return;
            if (!plantGetterExplosive.ActiveLocationTriggers
                    .TryGetValue(cell, out HashSet<Plant_Improved> plantsInCell)
                || plantsInCell == null
                || plantsInCell.Count == 0)
            {
                return;
            }
            
            foreach (Plant_Improved plant in plantsInCell)
            {
                var comp = plant.TryGetComp<Comp_HediffGiver>();
                var props = (CompProperties_HediffGiver)comp?.props;

                if (props?.hediffToGive == null) continue;

                if (comp.Triggered
                    || !Rand.Chance(props.chanceToGive)
                    || !(plant.Growth >= props.triggerGrowthThreshold)
                    || pawn.health.hediffSet.HasHediff(props.hediffToGive)) continue;
                
                comp.TryGiveHediff(pawn);
            }
        }
        
        public static void TryTriggerExplosivePlants(Pawn pawn, IntVec3 cell)
        {
            var plantGetterExplosive = pawn?.Map?.GetComponent<MapComponent_PlantGetter_Explosive>();
            
            if (plantGetterExplosive == null) return;
            if (!plantGetterExplosive.ActiveLocationTriggers
                    .TryGetValue(cell, out HashSet<Plant_Improved> plantsInCell)
                || plantsInCell == null
                || plantsInCell.Count == 0)
            {
                return;
            }
            
            foreach (Plant_Improved plant in plantsInCell)
            {
                var comp = plant.TryGetComp<Comp_Explosive>();
                var props = (CompProperties_Explosive)comp?.props;
                
                if (props?.triggeredDamageDef == null) continue;
                
                if (comp.Exploded 
                    || !(plant.Growth >= props.triggerGrowthThreshold)) continue;
                
                comp.TryDoExplosion();
                comp.Exploded = true;
            }
        }
    }
}