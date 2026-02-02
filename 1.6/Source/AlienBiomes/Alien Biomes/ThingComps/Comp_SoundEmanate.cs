using UnityEngine;
using Verse;
using Verse.Sound;

namespace AlienBiomes
{
    public class Comp_SoundEmanate : ThingComp
    {
        public CompProperties_SoundEmanate Props => (CompProperties_SoundEmanate)props;
        
        private SoundDef _cachedSound;
        private ModExt_PlantVisuallyReactive _ext = null;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            _cachedSound = Props.triggeredSound;
            _ext = parent.def.GetModExtension<ModExt_PlantVisuallyReactive>();
        }
        
        public void TryPlayTriggeredSound()
        {
            if (Props.triggeredSound == null 
                || !AlienBiomesSettings.AllowCompEffectSounds
                || !Rand.Chance(AlienBiomesSettings.PlantSFXChance)) return;

            if (parent is Plant_VisuallyReactive plant)
            {
                if (_ext == null) return;
                if (!Mathf.Approximately(plant.CurrentScale, _ext.minDrawScale))
                {
                    _cachedSound.PlayOneShot(new TargetInfo(parent.Position, parent.Map));
                }
            }
            else
            {
                _cachedSound.PlayOneShot(new TargetInfo(parent.Position, parent.Map));
            }
        }
    }
}