using UnityEngine;

namespace AlienBiomes
{
    public static class InternalShaderPropertyIDs
    {
        private static readonly string AlphaFactorName = "_AlphaFactor";
        private static readonly string DistortionTexName = "_DistortionTex";
        private static readonly string ShimmerTexName = "_ShimmerTex";
        private static readonly string ShimmerTilingName = "_ShimmerTiling";
        private static readonly string ShimmerScrollSpeedName = "_ShimmerScrollSpeed";
        private static readonly string ShimmerDistortionWeightName = "_ShimmerDistortionWeight";
        private static readonly string DistortionIntensityName = "_DistortionIntensity";
        private static readonly string DistortionScaleName = "_DistortionScale";
        private static readonly string DistortionScrollSpeedName = "_DistortionScrollSpeed";
        
        public static int AlphaFactor = Shader.PropertyToID(AlphaFactorName);
        public static int DistortionTex = Shader.PropertyToID(DistortionTexName);
        public static int ShimmerTex = Shader.PropertyToID(ShimmerTexName);
        public static int ShimmerTiling = Shader.PropertyToID(ShimmerTilingName);
        public static int ShimmerScrollSpeed = Shader.PropertyToID(ShimmerScrollSpeedName);
        public static int ShimmerDistortionWeight = Shader.PropertyToID(ShimmerDistortionWeightName);
        public static int DistortionIntensity = Shader.PropertyToID(DistortionIntensityName);
        public static int DistortionScale = Shader.PropertyToID(DistortionScaleName);
        public static int DistortionScrollSpeed = Shader.PropertyToID(DistortionScrollSpeedName);
    }
}