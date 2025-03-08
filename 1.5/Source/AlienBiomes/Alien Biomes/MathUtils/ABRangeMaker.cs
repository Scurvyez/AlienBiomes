using Verse;

namespace AlienBiomes
{
    public static class ABRangeMaker
    {
        public static IntRange GetRangeWithMidpointValue(int midpoint, int offset)
        {
            int lowerBound = midpoint - offset;
            int upperBound = midpoint + offset;
            return new IntRange(lowerBound, upperBound);
        }
    }
}