using Verse;

namespace AlienBiomes
{
    [StaticConstructorOnStartup]
    internal static class WorldGenStepConstructor
    {
        static WorldGenStepConstructor()
        {
            DefDatabase<WorldGenStepDef>.Add(new WorldGenStepDef
            {
                defName = "ABWorldGenStep",
                order = 999f,
                worldGenStep = new WorldGenStep_Late()
            });
        }
    }
}