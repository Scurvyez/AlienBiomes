﻿using HarmonyLib;
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
            if (!___pawn.IsColonist || ___pawn.Map == null)
                return;

            IntVec3 nextCell = ___pawn.pather.nextCell;
            MapComponent_PlantGetter plantGetter = ___pawn.Map.GetComponent<MapComponent_PlantGetter>();

            if (plantGetter == null)
                return;

            if (plantGetter.ActiveLocationTriggers.TryGetValue(nextCell, out HashSet<Plant_Nastic> plantsInCell))
            {
                foreach (Plant_Nastic plant in plantsInCell)
                {
                    Plant_Nastic_ModExtension plantExt = plant.def.GetModExtension<Plant_Nastic_ModExtension>();
                    if (plantExt == null)
                        continue;

                    if (plantExt.emitFlecks)
                    {
                        plant.DrawEffects();
                    }

                    if (plantExt.isTouchSensitive)
                    {
                        plant.touchSensitiveSwitch = true;
                    }

                }
            }

            /*
            else
            {
                // Set touchSensitiveSwitch to false if the pawn is not about to move into any cells
                SetTouchSensitiveSwitchFalse(___pawn.Map.GetComponent<MapComponent_PlantGetter>());
            }
            */
        }

        /*
        private static void SetTouchSensitiveSwitchFalse(MapComponent_PlantGetter plantGetter)
        {
            foreach (HashSet<Plant_Nastic> plantsInCell in plantGetter.ActiveLocationTriggers.Values)
            {
                foreach (Plant_Nastic plant in plantsInCell)
                {
                    plant.touchSensitiveSwitch = false;
                }
            }
        }
        */
    }
}
