using Verse;

namespace AlienBiomes
{
    public class AlienBiomesSettings : ModSettings
    {
        private static AlienBiomesSettings _instance;

        public static bool AllowPlantGlow
        {
            get
            {
                return _instance._allowPlantGlow;
            }
        }

        public static bool ShowEffecterOverlay
        {
            get
            {
                return _instance._showEffecterOverlay;
            }
        }

        public static bool ShowSpecialEffects
        {
            get
            {
                return _instance._showSpecialEffects;
            }
        }

        public static bool AllowCrystallizing
        {
            get
            {
                return _instance._allowCrystallizing;
            }
        }

        public static bool AllowReplacingOfSand
        {
            get
            {
                return _instance._allowReplacingOfSand;
            }
        }

        public static bool AllowReplacingOfGravel
        {
            get
            {
                return _instance._allowReplacingOfGravel;
            }
        }

        public static bool AllowCompEffectSounds
        {
            get
            {
                return _instance._allowCompEffectSounds;
            }
        }

        public static float PlantSoundEffectVolume
        {
            get
            {
                return _instance._plantSoundEffectVolume;
            }
        }
        
        public bool _allowPlantGlow = false;
        public bool _showEffecterOverlay = false;
        public bool _showSpecialEffects = false;
        public bool _allowCrystallizing = false;
        public bool _allowReplacingOfSand = false;
        public bool _allowReplacingOfGravel = false;
        public bool _allowCompEffectSounds = false;
        public float _plantSoundEffectVolume = 1.00f;

        public AlienBiomesSettings()
        {
            _instance = this;
        }
        
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref _allowPlantGlow, "allowPlantGlow", true);
            Scribe_Values.Look(ref _showEffecterOverlay, "allowEffecterOverlay", true);
            Scribe_Values.Look(ref _showSpecialEffects, "showSpecialEffects", true);
            Scribe_Values.Look(ref _allowCrystallizing, "allowCrystallizing", true);
            Scribe_Values.Look(ref _allowReplacingOfSand, "allowReplacingOfSand", true);
            Scribe_Values.Look(ref _allowReplacingOfGravel, "allowReplacingOfSand", true);
            Scribe_Values.Look(ref _allowCompEffectSounds, "allowCompEffectSounds", true);
            Scribe_Values.Look(ref _plantSoundEffectVolume, "plantSoundEffectVolume", 1.00f);
        }
    }
}
