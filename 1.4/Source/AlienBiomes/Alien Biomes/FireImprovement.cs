using HarmonyLib;
using RimWorld;
using System.Reflection;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    /*
    [StaticConstructorOnStartup]
    public static class FireImprovement
    {
        private static readonly FieldInfo FireGraphicField = AccessTools.Field(typeof(CompFireOverlay), "FireGraphic");
        private static readonly Texture2D FireGraphicMask = ContentFinder<Texture2D>.Get("Other/Ripples", true);
        private static MaterialPropertyBlock MPB;

        static FireImprovement()
        {
            Graphic improvedFireGraphic = GraphicDatabase.Get<Graphic_Flicker>("Things/Special/Fire", AlienBiomesContentDatabase.ImprovedFire, Vector2.one, Color.white);

            MPB = new MaterialPropertyBlock();
            float randomShimmerSpeed = Random.Range(3f, 6f);
            float randomShimmerLength = Random.Range(-15f, -7.5f);
            float randomHorizontalDistortionAmount = Random.Range(5f, 10f);
            float randomVerticalDistortionAmount = Random.Range(2f, 5.5f);
            float randomDistortionSpeed = Random.Range(2f, 6f);

            MPB.SetTexture("_MaskTex", FireGraphicMask);
            MPB.SetFloat("_ShimmerSpeed", randomShimmerSpeed);
            MPB.SetFloat("_ShimmerLength", randomShimmerLength);
            MPB.SetFloat("_HorizontalDistortionAmount", randomHorizontalDistortionAmount);
            MPB.SetFloat("_VerticalDistortionAmount", randomVerticalDistortionAmount);
            MPB.SetFloat("_DistortionSpeed", randomDistortionSpeed);

            FireGraphicField.SetValue(null, improvedFireGraphic);
        }
    }
    */
}
