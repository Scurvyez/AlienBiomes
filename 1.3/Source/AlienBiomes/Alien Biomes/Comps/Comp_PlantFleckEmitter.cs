using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace AlienBiomes
{
    public class Comp_PlantFleckEmitter : ThingComp
    {
        private CompProperties_PlantFleckEmitter Props => (CompProperties_PlantFleckEmitter)props;

        private Color EmissionColor => Color.Lerp(Props.colorA, Props.colorB, Rand.Value);
        public List<Pawn> pawnsTouchingPlants = new();

        public override void CompTick()
        {
            List<Thing> pawnList = parent.Position.GetThingList(parent.Map);
            for (int i = 0; i < pawnsTouchingPlants.Count; ++i)
            {
                Pawn touchingPawn = pawnsTouchingPlants[i];
                if (!touchingPawn.Spawned || touchingPawn.Position != parent.Position)
                    pawnsTouchingPlants.Remove(touchingPawn);
            }
            for (int ii = 0; ii < pawnList.Count; ++ii)
            {
                if (pawnList[ii] is Pawn p1 && !pawnsTouchingPlants.Contains(p1))
                {
                    pawnsTouchingPlants.Add(p1);
                    Emit();
                    //Log.Message("Fleck was emitted at " + parent.Position);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Emit()
        {
            SoundDef soundDefUsed = Props.soundOnEmission;
            for (int iii = 0; iii < Props.burstCount; ++iii)
            {
                FleckCreationData dataStatic = FleckMaker.GetDataStatic(parent.DrawPos, parent.Map, Props.fleck, Props.scale.RandomInRange);
                dataStatic.rotationRate = Props.rotationRate.RandomInRange;
                dataStatic.instanceColor = new Color?(EmissionColor);
                dataStatic.velocityAngle = Props.velocityX.RandomInRange;
                dataStatic.velocitySpeed = Props.velocityY.RandomInRange;
                parent.Map.flecks.CreateFleck(dataStatic);
                //Log.Message("Fleck was created ");
                soundDefUsed.PlayOneShot(new TargetInfo(parent.Position, null));
            }
        }
    }
}
