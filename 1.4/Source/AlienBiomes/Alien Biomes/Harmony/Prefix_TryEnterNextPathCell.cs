using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace AlienBiomes
{
    [HarmonyPatch(typeof(Pawn_PathFollower), "TryEnterNextPathCell")]
    public class TryEnterNextPathCell_Patch
    {
        [HarmonyPostfix]
        public static void Prefix(Pawn ___pawn)
        {
            if (___pawn.IsColonist)
            {
                IntVec3 nextCell = ___pawn.pather.nextCell;
                MapComponent_PlantGetter plantGetter = ___pawn.Map?.GetComponent<MapComponent_PlantGetter>();

                if (plantGetter != null && plantGetter.ActiveLocationTriggers.TryGetValue(nextCell, out HashSet<Plant> plantsInCell))
                {
                    foreach (Plant plant in plantsInCell)
                    {
                        if (plant is Plant_Nastic nasticPlant)
                        {
                            nasticPlant.DrawVisuals();
                        }
                    }
                }
            }
        }
    }
}
