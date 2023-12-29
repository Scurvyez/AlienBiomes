using System;
using UnityEngine;

namespace AlienBiomes
{
    /// <summary>
    /// Just remember: 0~1 in, 0~1 out is how you make good, reusable, nestable easing functions 👍
    /// </summary>
    public static class ABEasingFunctions
    {
        public static float Linear(float t) => t;
        public static float EaseInQuad(float t) => t * t;
        public static float EaseOutQuad(float t) => 1f - (1f - t) * (1f - t);
        public static float EaseInOutQuad(float t) => t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;
        public static float InSine(float t) => (float)-Math.Cos(t * Math.PI / 2);
        public static float OutSine(float t) => (float)Math.Sin(t * Math.PI / 2);
        public static float InOutSine(float t) => (float)(Math.Cos(t * Math.PI) - 1) / -2;
        public static float InBounce(float t) => 1 - OutBounce(1 - t);
        public static float OutBounce(float t)
        {
            float div = 2.75f;
            float mult = 7.5625f;

            if (t < 1 / div)
            {
                return mult * t * t;
            }
            else if (t < 2 / div)
            {
                t -= 1.5f / div;
                return mult * t * t + 0.75f;
            }
            else if (t < 2.5 / div)
            {
                t -= 2.25f / div;
                return mult * t * t + 0.9375f;
            }
            else
            {
                t -= 2.625f / div;
                return mult * t * t + 0.984375f;
            }
        }
        public static float InOutBounce(float t)
        {
            if (t < 0.5) return InBounce(t * 2) / 2;
            return 1 - InBounce((1 - t) * 2) / 2;
        }
    }
}
