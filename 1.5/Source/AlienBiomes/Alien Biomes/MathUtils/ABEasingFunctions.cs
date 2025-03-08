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
    }
}