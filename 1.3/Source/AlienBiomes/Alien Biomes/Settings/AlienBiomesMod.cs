using UnityEngine;
using Verse;
using System;

namespace AlienBiomes
{
    public class AlienBiomesMod : Mod
    {
        AlienBiomesSettings settings;

        public AlienBiomesMod(ModContentPack content): base(content)
        {
            settings = GetSettings<AlienBiomesSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            Listing_Standard list = new ();

            list.Begin(inRect);
            list.Gap(12.0f);

            list.Label("<color=cyan>General</color>");
            list.GapLine(2.00f);
            list.CheckboxLabeled("AlienBiomes_SettingCrystallizing".Translate(), ref settings._allowCrystallizing, "AlienBiomes_SettingCrystallizingDesc".Translate());
            list.CheckboxLabeled("AlienBiomes_SettingReplaceSand".Translate(), ref settings._allowReplacingOfSand, "AlienBiomes_SettingReplaceSandDesc".Translate());
            list.CheckboxLabeled("AlienBiomes_SettingReplaceGravel".Translate(), ref settings._allowReplacingOfGravel, "AlienBiomes_SettingReplaceGravelDesc".Translate());
            list.Gap(12.0f);

            list.Label("<color=cyan>Graphics</color> / <color=cyan>Performance</color>");
            list.GapLine(2.00f);
            list.CheckboxLabeled("AlienBiomes_SettingPlantGlow".Translate(), ref settings._allowPlantGlow, "AlienBiomes_SettingPlantGlowDesc".Translate());
            list.CheckboxLabeled("AlienBiomes_SettingEffecterOverlay".Translate(), ref settings._showEffecterOverlay, "AlienBiomes_SettingEffecterOverlayDesc".Translate());
            list.CheckboxLabeled("AlienBiomes_SettingSpecialEffects".Translate(), ref settings._showSpecialEffects, "AlienBiomes_SettingSpecialEffectsDesc".Translate());
            list.Gap(12.0f);

            list.Label("<color=cyan>Sounds</color>");
            list.GapLine(2.00f);
            list.CheckboxLabeled("AlienBiomes_SettingCompEffectSounds".Translate(), ref settings._allowCompEffectSounds, "AlienBiomes_SettingCompEffectSoundsDesc".Translate());
            list.Label(label: "AlienBiomes_PlantSoundEffectVolume".Translate((100f * settings._plantSoundEffectVolume).ToString("F0")), tooltip: "AlienBiomes_PlantSoundEffectVolumeDesc".Translate());
            settings._plantSoundEffectVolume = Mathf.Round(list.Slider(100f * settings._plantSoundEffectVolume, 0f, 100f)) / 100f;

            list.End();
        }

        public override string SettingsCategory()
        {
            return "AlienBiomes_ModName".Translate();
        }
    }
}
