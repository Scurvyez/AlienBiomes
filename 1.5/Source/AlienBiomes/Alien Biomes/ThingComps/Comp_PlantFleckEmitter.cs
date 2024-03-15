using RimWorld;
using System.Collections.Generic;
using System.Linq;
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
        private HashSet<Pawn> pawnsTouchingPlants = new();

        public override void CompTickLong()
        {
            if (!AlienBiomesSettings.ShowSpecialEffects) return;

            IEnumerable<Pawn> pawns = parent.Position.GetThingList(parent.Map).OfType<Pawn>();
            foreach (Pawn pawn in pawns)
            {
                if (pawnsTouchingPlants.Add(pawn))
                {
                    Emit();
                }
            }
            pawnsTouchingPlants.RemoveWhere(pawn => !pawn.Spawned || pawn.Position != parent.Position);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Emit()
        {
            SoundDef sDef = Props.soundOnEmission;
            Vector3 vel = new Vector3(0.00f, 0.00f, 0.5f) * Rand.Range(0.1f, 0.8f);

            for (int i = 0; i < Props.burstCount; ++i)
            {
                FleckCreationData fCD = FleckMaker.GetDataStatic(parent.DrawPos, parent.Map, Props.fleck, Props.scale.RandomInRange);
                fCD.rotationRate = Rand.RangeInclusive(-240, 240);
                fCD.instanceColor = new Color?(EmissionColor);
                fCD.velocity = vel;
                parent.Map.flecks.CreateFleck(fCD);

                if (AlienBiomesSettings.AllowCompEffectSounds)
                {
                    SoundInfo sI = new TargetInfo(parent.Position, parent.Map);
                    sI.volumeFactor = AlienBiomesSettings.PlantSoundEffectVolume;
                    sDef.PlayOneShot(sI);
                }
            }
        }
    }
}
