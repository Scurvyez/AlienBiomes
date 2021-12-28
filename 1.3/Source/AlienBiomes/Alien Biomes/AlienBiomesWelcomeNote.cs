using Verse;

namespace AlienBiomes
{
    [StaticConstructorOnStartup]
    public static class AlienBiomesWelcomeNote
    {
        static AlienBiomesWelcomeNote()
        {
            Log.Message("[AlienBiomes] Welcome, enjoy the ride!");
        }
    }
}
