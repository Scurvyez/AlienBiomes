using HarmonyLib;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace AlienBiomes
{
    /*
    [HarmonyPatch(typeof(Pawn_PathFollower), "TryEnterNextPathCell")]
    public class TryEnterNextPathCell_Patch
    {
        [HarmonyPostfix]
        public static void Prefix(Pawn ___pawn)
        {
            if (___pawn.Map == null || !___pawn.RaceProps.Humanlike)
                return;

            IntVec3 nextCell = ___pawn.pather.nextCell;
            MapComponent_PlantGetter plantGetter = ___pawn.Map.GetComponent<MapComponent_PlantGetter>();

            if (plantGetter == null)
                return;

            if (plantGetter.ActiveLocationTriggers.TryGetValue(nextCell, out HashSet<Plant_Nastic> plantsInCell))
            {
                foreach (Plant_Nastic plant in plantsInCell)
                {
                    PlantNastic_ModExtension plantExt = plant.def.GetModExtension<PlantNastic_ModExtension>();
                    if (plantExt == null)
                        continue;

                    if (plantExt.emitFlecks)
                    {
                        plant.DrawEffects();
                    }

                    else if (plantExt.isTouchSensitive)
                    {
                        if (plantExt.isVisuallyReactive)
                        {
                            plant.TouchSensitiveStartTime = GenTicks.TicksGame;
                        }
                        else if (plantExt.isDamaging && !plant.GasExpelled)
                        {
                            plant.ExpelGas();
                            plant.GasExpelled = true;
                        }
                    }
                    else if (plantExt.givesHediff)
                    {
                        plant.GiveHediff(___pawn);
                    }
                }
            }
        }
    }
    */
}
