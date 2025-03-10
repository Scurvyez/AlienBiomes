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
        public static float PlantSoundEffectVolume => _instance._plantSoundEffectVolume;
        public static bool AllowCrystallizing => _instance._allowCrystallizing;
        
        public bool _showPlantGlow = true;
        public bool _showTerrainDebris = true;
        public bool _allowCompEffectSounds = true;
        public float _plantSoundEffectVolume = 1.00f;
        public bool _allowCrystallizing = true;
        
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref _showPlantGlow, "allowPlantGlow", true);
            Scribe_Values.Look(ref _showTerrainDebris, "showTerrainDebris", true);
            Scribe_Values.Look(ref _allowCompEffectSounds, "allowCompEffectSounds", true);
            Scribe_Values.Look(ref _plantSoundEffectVolume, "plantSoundEffectVolume", 1.00f);
            Scribe_Values.Look(ref _allowCrystallizing, "allowCrystallizing", true);
        }
    }
}