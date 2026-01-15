using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    [StaticConstructorOnStartup]
    public static class AssetCompendium
    {
        private const string BiolumWaterOverlayTexPath = "AlienBiomes/Other/BiolumWater1";
        private static readonly Texture2D ShimmerTex;
        
        private static readonly Dictionary<int, Material> BiolumMatByBiome = new();
        private static readonly Material BaseBiolumMat;

        static AssetCompendium()
        {
            ShimmerTex = ContentFinder<Texture2D>.Get("AlienBiomes/Other/Noise_036");
            
            Shader shader = InternalDefOf.AB_MoteGlowDistortedVertex.Shader;
            Material pooled = MaterialPool.MatFrom(BiolumWaterOverlayTexPath, shader);
            BaseBiolumMat = new Material(pooled);

            if (BaseBiolumMat.mainTexture is Texture2D tex)
            {
                tex.wrapMode = TextureWrapMode.Repeat;
                tex.filterMode = FilterMode.Bilinear;
            }
            
            ShimmerTex.wrapMode = TextureWrapMode.Repeat;
            
            ApplyBiolumMaterialTuning(BaseBiolumMat);
        }

        public static Material GetBiolumMaterialForBiome(BiomeDef biome)
        {
            int key = biome?.shortHash ?? 0;
            if (BiolumMatByBiome.TryGetValue(key, out Material mat) && mat != null)
                return mat;

            mat = new Material(BaseBiolumMat);

            Color tint = biome?.GetModExtension<ModExt_BiomeBiolumColor>()?.biolumColor
                         ?? Color.white;

            mat.color = tint;
            BiolumMatByBiome[key] = mat;
            
            return mat;
        }

        private static void ApplyBiolumMaterialTuning(Material mat)
        {
            if (mat == null) return;

            mat.SetFloat(InternalShaderPropertyIDs.AlphaFactor, 1.5f);
            mat.SetTexture(InternalShaderPropertyIDs.DistortionTex, TexGame.RippleTex);
            mat.SetTexture(InternalShaderPropertyIDs.ShimmerTex, ShimmerTex);
            mat.SetVector(InternalShaderPropertyIDs.ShimmerTiling, new Vector2(1f, 1f));
            mat.SetVector(InternalShaderPropertyIDs.ShimmerScrollSpeed, new Vector4(0.03f, 0.03f, 0f, 0f));
            mat.SetFloat(InternalShaderPropertyIDs.ShimmerDistortionWeight, 0.75f);
            mat.SetFloat(InternalShaderPropertyIDs.DistortionIntensity, 0.05f);
            mat.SetFloat(InternalShaderPropertyIDs.DistortionScale, 0.1f);
            mat.SetVector(InternalShaderPropertyIDs.DistortionScrollSpeed, new Vector4(0.02f, 0.02f, 0f, 0f));
        }
    }
}