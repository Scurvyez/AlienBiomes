using System;
using Verse;

namespace AlienBiomes
{
    [StaticConstructorOnStartup]
    public static class AlienBiomesMain
    {
        public static bool DebugStartOutputLogging = false;
        
        static AlienBiomesMain()
        {
            ABLog.Message($"{DateTime.Now.Date.ToShortDateString()} "
                          + "[1.6 Alpha-Build | Nothing to report.]");
        }
    }
}