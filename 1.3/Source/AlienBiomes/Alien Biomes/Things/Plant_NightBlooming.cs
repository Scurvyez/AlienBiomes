using RimWorld;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    [StaticConstructorOnStartup]
    public class Plant_NightBlooming : Plant
    {
        public static Graphic GraphicSowing = GraphicDatabase.Get<Graphic_Single>("Things/Plant/Plant_Sowing", ShaderDatabase.Cutout, Vector2.one, Color.white);
        //public string nightfallGraphicPath;
        private Graphic graphicNightFallCache;
        public Graphic GraphicNightFall => graphicNightFallCache;
        // No loading GraphicNightFall every frame.

        /// <summary>
        /// Returns a given graphic based on status.
        /// Statuses = sowing, set time of day range, leafless, and immature.
        /// </summary>
        public override Graphic Graphic
        {
            get
            {
                if (LifeStage == PlantLifeStage.Sowing) return GraphicSowing;
                // Same as vanilla.

                if (!def.HasModExtension<NightBlooming_ModExtension>()) return base.Graphic;

                NightBlooming_ModExtension plantExt = def.GetModExtension<NightBlooming_ModExtension>();
                if ((GenLocalDate.DayPercent(this) * Rand.RangeSeeded(0.9f, 1.1f, Position.x)) >= plantExt.nightBloomStart 
                    || (GenLocalDate.DayPercent(this) * Rand.RangeSeeded(0.9f, 1.1f, Position.x)) <= plantExt.nightBloomEnd)
                    // This statement checks time of day on the player map (GenLocalDate.DayPercent), in sections (Rand.RangeSeeded)
                    // DayPercent max = 1.00 or 24 hours, so 0.75 = 18 hours
                    // ...so for this example, the check starts at 1800.
                {
                    return graphicNightFallCache ??= GraphicDatabase.Get<Graphic_Single>
                        (plantExt.nightfallGraphicPath, ShaderDatabase.TransparentPlant, Vector2.one, Color.white);
                }

                if (def.plant.leaflessGraphic != null && LeaflessNow && (!sown || !HarvestableNow)) {
                    return def.plant.leaflessGraphic;
                    // Same as vanilla.
                }

                if (def.plant.immatureGraphic != null && !HarvestableNow) {
                    return def.plant.immatureGraphic;
                    // Same as vanilla.
                }

                return base.Graphic;
            }
        }
    }
}
