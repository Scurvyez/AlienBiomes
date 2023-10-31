using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class Comp_TestInvisAnimal : ThingComp
    {
        public CompProperties_TestInvisAnimal Props => (CompProperties_TestInvisAnimal)props;
        public float curAlpha;

        public override void CompTick()
        {
            base.CompTick();
            curAlpha = Mathf.Sin(Time.time * Props.fadeSpeed) * 0.5f + 0.5f;
        }

        public override void PostDraw()
        {
            base.PostDraw();
            if (parent is not Pawn parentPawn) return;

            if (parentPawn != null && parentPawn.def.defName == "Muffalo")
            {
                PawnKindDef pawnKind = parentPawn.kindDef;

                if (pawnKind != null && parentPawn.ageTracker != null && pawnKind.lifeStages != null &&
                    parentPawn.ageTracker.CurLifeStage != null && parentPawn.ageTracker.CurLifeStageIndex >= 0 &&
                    parentPawn.ageTracker.CurLifeStageIndex < pawnKind.lifeStages.Count &&
                    pawnKind.lifeStages[parentPawn.ageTracker.CurLifeStageIndex] != null &&
                    pawnKind.lifeStages[parentPawn.ageTracker.CurLifeStageIndex].bodyGraphicData != null &&
                    pawnKind.lifeStages[parentPawn.ageTracker.CurLifeStageIndex].bodyGraphicData.Graphic != null &&
                    pawnKind.lifeStages[parentPawn.ageTracker.CurLifeStageIndex].bodyGraphicData.Graphic.data != null &&
                    pawnKind.lifeStages[parentPawn.ageTracker.CurLifeStageIndex].bodyGraphicData.Graphic.data.shaderType != null)
                {
                    Material mat = pawnKind.lifeStages[parentPawn.ageTracker.CurLifeStageIndex].bodyGraphicData.Graphic.MatSingle;
                    if (mat != null)
                    {
                        mat.SetFloat("_ToggleAlpha", curAlpha);
                    }
                }
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            
        }
    }

    public class CompProperties_TestInvisAnimal : CompProperties
    {
        public float fadeSpeed = 0.02f;

        public CompProperties_TestInvisAnimal() => compClass = typeof(Comp_TestInvisAnimal);

    }
}
