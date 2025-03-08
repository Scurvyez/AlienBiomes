using Verse;

namespace AlienBiomes
{
    public class AlienBiomesSettings : ModSettings
    {
        private static AlienBiomesSettings _instance;

        public static bool ShowPlantGlow => _instance._showPlantGlow;
        public static bool ShowEffecterOverlay => _instance._showEffecterOverlay;
        public static bool ShowSpecialEffects => _instance._showSpecialEffects;
        public static bool EnableScreenPosEffects => _instance._enableScreenPosEffects;
        public static bool UseAlienSand => _instance._useAlienSand;
        public static bool UseAlienGravel => _instance._useAlienGravel;
        public static bool UseAlienWater => _instance._useAlienWater;
        public static bool ShowTerrainDebris => _instance._showTerrainDebris;
        public static bool AllowCompEffectSounds => _instance._allowCompEffectSounds;
        public static float PlantSoundEffectVolume => _instance._plantSoundEffectVolume;
        public static bool AllowCrystallizing => _instance._allowCrystallizing;
        
        public bool _showPlantGlow = true;
        public bool _showEffecterOverlay = true;
        public bool _showSpecialEffects = true;
        public bool _enableScreenPosEffects = true;
        public bool _useAlienSand = true;
        public bool _useAlienGravel = true;
        public bool _useAlienWater = true;
        public bool _showTerrainDebris = true;
        public bool _allowCompEffectSounds = true;
        public float _plantSoundEffectVolume = 1.00f;
        public bool _allowCrystallizing = true;

        public AlienBiomesSettings()
        {
            _instance = this;
        }
        
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref _showPlantGlow, "allowPlantGlow", true);
            Scribe_Values.Look(ref _showEffecterOverlay, "allowEffecterOverlay", true);
            Scribe_Values.Look(ref _showSpecialEffects, "showSpecialEffects", true);
            Scribe_Values.Look(ref _enableScreenPosEffects, "enableScreenPosEffects", true);
            Scribe_Values.Look(ref _useAlienSand, "useAlienSand", true);
            Scribe_Values.Look(ref _useAlienGravel, "useAlienGravel", true);
            Scribe_Values.Look(ref _useAlienWater, "useAlienWater", true);
            Scribe_Values.Look(ref _showTerrainDebris, "showTerrainDebris", true);
            Scribe_Values.Look(ref _allowCompEffectSounds, "allowCompEffectSounds", true);
            Scribe_Values.Look(ref _plantSoundEffectVolume, "plantSoundEffectVolume", 1.00f);
            Scribe_Values.Look(ref _allowCrystallizing, "allowCrystallizing", true);
        }
    }
}