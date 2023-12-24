using System.Linq;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    /*
    [StaticConstructorOnStartup]
    public class MapComponent_PlantEffects : MapComponent
    {
        FullScreenEffects fullScreenEffects = FullScreenEffects.instance;
        private ThingWithComps specificPlant;
        private Vector3 trackedPosition;

        public static readonly Texture2D PCFOTex = ContentFinder<Texture2D>.Get("Things/Plants/Special/PyroclasticChaliceFungus/GlistenEffect1");

        public MapComponent_PlantEffects(Map map) : base(map) { }

        public override void MapGenerated()
        {
            base.MapGenerated();
            if (map.Biome == AlienBiomes_BiomeDefOf.SZ_RadiantPlains)
            {
                fullScreenEffects.screenPosEffectsMat.SetTexture("_PCFOTex", PCFOTex);

                specificPlant = map.listerThings.ThingsOfDef(AlienBiomes_ThingDefOf.SZ_PyroclasticChaliceFungus).OfType<ThingWithComps>().FirstOrDefault();
                trackedPosition = specificPlant.Position.ToVector3();

                Log.Message("specificPlant: " + specificPlant);
                Log.Message("trackedPosition: " + trackedPosition);
            }
        }

        public override void MapComponentTick()
        {
            base.MapComponentTick();

            if (map.Biome == AlienBiomes_BiomeDefOf.SZ_RadiantPlains)
            {
                if (AlienBiomesSettings.EnableScreenPosEffects)
                {
                    fullScreenEffects.screenPosEffectsMat.SetVector("_TrackedPosition", trackedPosition);
                    fullScreenEffects.screenPosEffectsMat.SetFloat("_PCFOTexDrawSize", 1f);
                }
            }
        }
    }
    */
}
