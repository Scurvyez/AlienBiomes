using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public static class RangeUtils
    {
        public static IntRange GetRangeWithMidpointValue(int midpoint, int offset)
        {
            int lowerBound = midpoint - offset;
            int upperBound = midpoint + offset;
            return new IntRange(lowerBound, upperBound);
        }
        
        public static float RangeWeight(float value, float min, float max)
        {
            if (value <= min || value >= max)
                return 0f;

            float mid = (min + max) * 0.5f;
            float half = (max - min) * 0.5f;
            float dist = Mathf.Abs(value - mid);

            // 1 at the center, 0 at the edges
            return 1f - (dist / half);
        }
    }
}