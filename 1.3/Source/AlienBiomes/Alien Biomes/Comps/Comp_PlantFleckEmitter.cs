using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class Comp_PlantFleckEmitter : ThingComp
    {
        private CompProperties_PlantFleckEmitter Props => (CompProperties_PlantFleckEmitter)props;

        private Color EmissionColor => Color.Lerp(Props.colorA, Props.colorB, Rand.Value);

        public override void CompTickRare()
        {
            List<Pawn> pawnsTouchingPlants = new();

            if (parent.Spawned)
            {
                List<Thing> pawnList = parent.Position.GetThingList(parent.Map);
                for (int i = 0; i < pawnList.Count; ++i)
                {
                    if (pawnList[i] is Pawn p1 && !pawnsTouchingPlants.Contains(p1))
                    {
                        pawnsTouchingPlants.Add(p1);
                        Emit();
                    }
                }
                for (int ii = 0; ii < pawnsTouchingPlants.Count; ++ii)
                {
                    Pawn touchingPawn = pawnsTouchingPlants[ii];
                    if (!touchingPawn.Spawned || touchingPawn.Position != parent.Position)
                        pawnsTouchingPlants.Remove(touchingPawn);
                }
            }
        }
        
        private void Emit()
        {
            for (int iii = 0; iii < Props.burstCount; ++iii)
            {
                FleckCreationData dataStatic = FleckMaker.GetDataStatic(parent.DrawPos, parent.Map, Props.fleck, Props.scale.RandomInRange);
                dataStatic.rotationRate = Props.rotationRate.RandomInRange;
                dataStatic.instanceColor = new Color?(EmissionColor);
                dataStatic.velocityAngle = Props.velocityX.RandomInRange;
                dataStatic.velocitySpeed = Props.velocityY.RandomInRange;
                parent.Map.flecks.CreateFleck(dataStatic);
            }
        }
    }
}
