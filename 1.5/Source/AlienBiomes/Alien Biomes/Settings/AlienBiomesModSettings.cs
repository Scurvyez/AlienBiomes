using Verse;

namespace AlienBiomes
{
    public class AlienBiomesSettings : ModSettings
    {
        private static AlienBiomesSettings _instance;

        public static bool ShowPlantGlow
        {
            get {
                return _instance._showPlantGlow;
            }
        }

        public static bool ShowEffecterOverlay
        {
            get {
                return _instance._showEffecterOverlay;
            }
        }

        public static bool ShowSpecialEffects
        {
            get {
                return _instance._showSpecialEffects;
            }
        }

        public static bool EnableScreenPosEffects
        {
            get
            {
                return _instance._enableScreenPosEffects;
            }
        }

        /*
        public static bool AllowCrystallizing
        {
            get {
                return _instance._allowCrystallizing;
            }
        }
        */

        public static bool UseAlienSand
        {
            get {
                return _instance._useAlienSand;
            }
        }

        public static bool UseAlienGravel
        {
            get {
                return _instance._useAlienGravel;
            }
        }

        public static bool UseAlienWater
        {
            get {
                return _instance._useAlienWater;
            }
        }

        public static bool AllowCompEffectSounds
        {
            get {
                return _instance._allowCompEffectSounds;
            }
        }

        public static float PlantSoundEffectVolume
        {
            get {
                return _instance._plantSoundEffectVolume;
            }
        }
        
        public bool _showPlantGlow = true;
        public bool _showEffecterOverlay = true;
        public bool _showSpecialEffects = true;
        public bool _enableScreenPosEffects = true;
        //public bool _allowCrystallizing = true;
        public bool _useAlienSand = true;
        public bool _useAlienGravel = true;
        public bool _useAlienWater = true;
        public bool _allowCompEffectSounds = true;
        public float _plantSoundEffectVolume = 1.00f;

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
            //Scribe_Values.Look(ref _allowCrystallizing, "allowCrystallizing", true);
            Scribe_Values.Look(ref _useAlienSand, "useAlienSand", true);
            Scribe_Values.Look(ref _useAlienGravel, "useAlienGravel", true);
            Scribe_Values.Look(ref _useAlienWater, "useAlienWater", true);
            Scribe_Values.Look(ref _allowCompEffectSounds, "allowCompEffectSounds", true);
            Scribe_Values.Look(ref _plantSoundEffectVolume, "plantSoundEffectVolume", 1.00f);
        }
    }
}
