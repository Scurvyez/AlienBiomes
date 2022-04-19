using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class AlienBiomesSettings : ModSettings
    {
        private static AlienBiomesSettings _instance;

        public static bool ShowPlantGlow
        {
            get
            {
                return _instance._showPlantGlow;
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

        public static bool UseVanillaSand
        {
            get
            {
                return _instance._useVanillaSand;
            }
        }

        public static bool UseVanillaGravel
        {
            get
            {
                return _instance._useVanillaGravel;
            }
        }

        public static bool UseVanillaWater
        {
            get
            {
                return _instance._useVanillaWater;
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
        
        public bool _showPlantGlow = false;
        public bool _showEffecterOverlay = false;
        public bool _showSpecialEffects = false;
        public bool _allowCrystallizing = false;
        public bool _useVanillaSand = false;
        public bool _useVanillaGravel = false;
        public bool _useVanillaWater = false;
        public bool _allowCompEffectSounds = false;
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
            Scribe_Values.Look(ref _allowCrystallizing, "allowCrystallizing", true);
            Scribe_Values.Look(ref _useVanillaSand, "useVanillaSand", false);
            Scribe_Values.Look(ref _useVanillaGravel, "useVanillaGravel", false);
            Scribe_Values.Look(ref _useVanillaWater, "useVanillaWater", false);
            Scribe_Values.Look(ref _allowCompEffectSounds, "allowCompEffectSounds", true);
            Scribe_Values.Look(ref _plantSoundEffectVolume, "plantSoundEffectVolume", 1.00f);
        }
    }
}
