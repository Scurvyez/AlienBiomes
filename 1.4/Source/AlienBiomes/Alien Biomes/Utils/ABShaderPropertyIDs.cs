using UnityEngine;

namespace AlienBiomes
{
    public static class ABShaderPropertyIDs
    {
        private static readonly string ShimmerSpeedName = "_ShimmerSpeed";
        private static readonly string PulseSpeedName = "_PulseSpeed";
        private static readonly string PulseLengthName = "_PulseLength";
        private static readonly string PulseOriginName = "_PulseOrigin";
        private static readonly string DistortionAmountName = "_DistortionAmount";

        public static int ShimmerSpeed = Shader.PropertyToID(ShimmerSpeedName);
        public static int PulseSpeed = Shader.PropertyToID(PulseSpeedName);
        public static int PulseLength = Shader.PropertyToID(PulseLengthName);
        public static int PulseOrigin = Shader.PropertyToID(PulseOriginName);
        public static int DistortionAmount = Shader.PropertyToID(DistortionAmountName);
    }
}
