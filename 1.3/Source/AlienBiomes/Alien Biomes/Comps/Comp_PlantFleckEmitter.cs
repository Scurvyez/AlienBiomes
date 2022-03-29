using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace AlienBiomes
{
    public class Comp_PlantFleckEmitter : ThingComp
    {
        // tickerType MUST match CompTick, so "Normal"
        private CompProperties_PlantFleckEmitter Props => (CompProperties_PlantFleckEmitter)props;
        private Color EmissionColor => Color.Lerp(Props.colorA, Props.colorB, Rand.Value);
        // New variable color for the fleck itself.
        public List<Pawn> pawnsTouchingPlants = new();
        // A list of all pawns on the map when touching plants.

        public override void CompTickLong()
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
            for (int i = 0; i < pawnList.Count; ++i)
                // Loop to add pawns to list of those touching plant(s).
            {
                if (pawnList[i] is Pawn p1 && !pawnsTouchingPlants.Contains(p1))
                {
                    pawnsTouchingPlants.Add(p1);

                    if (AlienBiomesSettings.ShowSpecialEffects == true)
                    {
                        Emit();
                    }
                    // Call Emit and cast the built fleck. :)
                    //Log.Message("Fleck was emitted at " + parent.Position);
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Emit()
        {
            SoundDef soundDefUsed = Props.soundOnEmission;
            float throwAngle = 90f;
            Vector3 inheritVelocity = new(0.00f, 0.00f, 1.00f);

            for (int i = 0; i < Props.burstCount; ++i)
            {
                FleckCreationData dataStatic = FleckMaker.GetDataStatic(parent.DrawPos, parent.Map, Props.fleck, Props.scale.RandomInRange);
                dataStatic.rotationRate = Rand.RangeInclusive(-240, 240);
                dataStatic.instanceColor = new Color?(EmissionColor);
                dataStatic.velocityAngle = throwAngle + Rand.Range(-5, 5);
                dataStatic.velocitySpeed = Rand.Range(0.1f, 0.8f);
                dataStatic.velocity = inheritVelocity * 0.5f;
                parent.Map.flecks.CreateFleck(dataStatic);

                //Log.Message("Fleck was created ");
                if (AlienBiomesSettings.AllowCompEffectSounds == true)
                {
                    SoundInfo sI = new TargetInfo(parent.Position, parent.Map);
                    sI.volumeFactor = AlienBiomesSettings.PlantSoundEffectVolume;
                    soundDefUsed.PlayOneShot(sI);
                }
            }
        }
    }
}
