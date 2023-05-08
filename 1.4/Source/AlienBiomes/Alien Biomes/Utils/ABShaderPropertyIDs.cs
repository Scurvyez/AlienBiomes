using UnityEngine;

namespace AlienBiomes
{
    public static class ABShaderPropertyIDs
    {
        private static readonly string HashOffsetName = "_HashOffset";
        private static readonly string PulseSpeedName = "_PulseSpeed";
        private static readonly string PulseLengthName = "_PulseLength";

        public static int HashOffset = Shader.PropertyToID(HashOffsetName);
        public static int PulseSpeed = Shader.PropertyToID(PulseSpeedName);
        public static int PulseLength = Shader.PropertyToID(PulseLengthName);
    }
}
