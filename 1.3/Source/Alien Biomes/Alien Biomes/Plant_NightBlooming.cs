using RimWorld;
using UnityEngine;
using Verse;

namespace Alien_Biomes
{
    [StaticConstructorOnStartup]
    public class Plant_NightBlooming : Plant
    {
        public static Graphic GraphicSowing = GraphicDatabase.Get<Graphic_Single>("Things/Plant/Plant_Sowing", ShaderDatabase.Cutout, Vector2.one, Color.white);
        public string nightfallGraphicPath;
        private Graphic graphicNightFallCache;
        public Graphic GraphicNightFall => graphicNightFallCache;
        // no loading GraphicNightFall every frame

        public override Graphic Graphic
        {
            get
            {
                if (LifeStage == PlantLifeStage.Sowing)
                {
                    return GraphicSowing;
                }

                if (!def.HasModExtension<NightBlooming_ModExtension>())
                {
                    return base.Graphic;
                }

                NightBlooming_ModExtension plantExt = def.GetModExtension<NightBlooming_ModExtension>();
                if ((GenLocalDate.DayPercent(this) * Rand.RangeSeeded(0.9f, 1.1f, Position.x)) >= 0.75 || (GenLocalDate.DayPercent(this) * Rand.RangeSeeded(0.9f, 1.1f, Position.x)) <= 0.20)
                    // This statement checks time of day on the player map (GenLocalDate.DayPercent), in sections (Rand.RangeSeeded)
                    // DayPercent max = 1.00 or 24 hours, so 0.75 = 18 hours
                    // ...so here, the check is for the hours of 1800 - 0448
                {
                    //Log.Message("Swap Texture Check " + GenLocalDate.DayPercent(this));
                    return graphicNightFallCache ??= GraphicDatabase.Get<Graphic_Single>(plantExt.nightfallGraphicPath, ShaderDatabase.TransparentPlant, Vector2.one, Color.white);
                }

                if (def.plant.leaflessGraphic != null && LeaflessNow && (!sown || !HarvestableNow))
                {
                    return def.plant.leaflessGraphic;
                }

                if (def.plant.immatureGraphic != null && !HarvestableNow)
                {
                    return def.plant.immatureGraphic;
                }

                return base.Graphic;
            }
        }
    }
}
