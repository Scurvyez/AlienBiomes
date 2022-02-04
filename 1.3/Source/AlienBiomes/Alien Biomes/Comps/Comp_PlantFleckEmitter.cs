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
        // New variable color for the fleck itself.
        public List<Pawn> pawnsTouchingPlants = new();
        // A list of all pawns on the map when touching plants.

        public override void CompTick()
        {
            List<Thing> pawnList = parent.Position.GetThingList(parent.Map);
            // List of parent position(s).
            for (int i = 0; i < pawnsTouchingPlants.Count; ++i)
                // Loop to remove pawns from list of those ouching plant(s).
            {
                Pawn touchingPawn = pawnsTouchingPlants[i];
                if (!touchingPawn.Spawned || touchingPawn.Position != parent.Position)
                    pawnsTouchingPlants.Remove(touchingPawn);
            }
            for (int ii = 0; ii < pawnList.Count; ++ii)
                // Loop to add pawns to list of those touching plant(s).
            {
                if (pawnList[ii] is Pawn p1 && !pawnsTouchingPlants.Contains(p1))
                {
                    pawnsTouchingPlants.Add(p1);
                    Emit();
                    // Call Emit and cast the fleck. :)
                    //Log.Message("Fleck was emitted at " + parent.Position);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Emit()
        {
            SoundDef soundDefUsed = Props.soundOnEmission;
            for (int iii = 0; iii < Props.burstCount; ++iii)
                // Fleck and sound creation loop.
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
