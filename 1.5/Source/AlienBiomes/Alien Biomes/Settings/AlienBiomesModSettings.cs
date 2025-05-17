using Verse;

namespace AlienBiomes
{
    public class AlienBiomesSettings : ModSettings
    {
        private static AlienBiomesSettings _instance;
        
        public AlienBiomesSettings() => _instance = this;
        
        public static bool ShowPlantGlow => _instance._showPlantGlow;
        public static bool ShowTerrainDebris => _instance._showTerrainDebris;
        public static bool AllowCompEffectSounds => _instance._allowCompEffectSounds;
        public static float PlantSFXChance => _instance._plantSFXChance;
        public static bool AllowCrystallizing => _instance._allowCrystallizing;
        
        public bool _showPlantGlow = true;
        public bool _showTerrainDebris = true;
        public bool _allowCompEffectSounds = true;
        public float _plantSFXChance = 1f;
        public bool _allowCrystallizing = true;
        
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref _showPlantGlow, "allowPlantGlow", true);
            Scribe_Values.Look(ref _showTerrainDebris, "showTerrainDebris", true);
            Scribe_Values.Look(ref _allowCompEffectSounds, "allowCompEffectSounds", true);
            Scribe_Values.Look(ref _plantSFXChance, "plantSFXChance", 1f);
            Scribe_Values.Look(ref _allowCrystallizing, "allowCrystallizing", true);
        }
    }
}