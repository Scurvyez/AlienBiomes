using System;
using Verse;

namespace AlienBiomes
{
    [StaticConstructorOnStartup]
    public static class AlienBiomesMain
    {
        static AlienBiomesMain()
        {
            ABLog.Message($"{DateTime.Now.Date.ToShortDateString()} "
                          + "[1.6 Alpha-Build | Nothing to report.]");
        }
    }
}