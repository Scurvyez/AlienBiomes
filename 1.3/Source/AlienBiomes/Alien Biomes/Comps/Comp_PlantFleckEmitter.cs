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
            List<Thing> pL = parent.Position.GetThingList(parent.Map); // list of pawns
            for (int i = 0; i < pawnsTouchingPlants.Count; ++i)
                // Loop to remove pawns from list of those ouching plant(s).
            {
                Pawn tP = pawnsTouchingPlants[i]; // pawns touching parent
                if (!tP.Spawned || tP.Position != parent.Position)
                    pawnsTouchingPlants.Remove(tP);
            }
            for (int i = 0; i < pL.Count; ++i)
                // Loop to add pawns to list of those touching plant(s).
            {
                if (pL[i] is Pawn p1 && !pawnsTouchingPlants.Contains(p1))
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
            SoundDef sDef = Props.soundOnEmission; // sound emitted in-game
            Vector3 vel = new(0.00f, 0.00f, 1.00f); // velocity

            for (int i = 0; i < Props.burstCount; ++i)
            {
                FleckCreationData fCD = FleckMaker.GetDataStatic(parent.DrawPos, parent.Map, Props.fleck, Props.scale.RandomInRange);
                fCD.rotationRate = Rand.RangeInclusive(-240, 240);
                fCD.instanceColor = new Color?(EmissionColor);
                fCD.velocitySpeed = Rand.Range(0.1f, 0.8f);
                fCD.velocity = vel * 0.5f;
                parent.Map.flecks.CreateFleck(fCD);

                //Log.Message("Fleck was created ");
                if (AlienBiomesSettings.AllowCompEffectSounds == true)
                {
                    SoundInfo sI = new TargetInfo(parent.Position, parent.Map);
                    sI.volumeFactor = AlienBiomesSettings.PlantSoundEffectVolume;
                    sDef.PlayOneShot(sI);
                }
            }
        }
    }
}
