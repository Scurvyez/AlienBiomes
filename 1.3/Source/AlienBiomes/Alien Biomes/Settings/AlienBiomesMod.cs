using UnityEngine;
using Verse;
using RimWorld;
using System;
using System.Reflection;

namespace AlienBiomes
{
    public class AlienBiomesMod : Mod
    {
        AlienBiomesSettings settings;
        private Vector2 scrollPosition = Vector2.zero;

        public AlienBiomesMod(ModContentPack content): base(content)
        {
            settings = GetSettings<AlienBiomesSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            Listing_Standard list = new ();
            Rect viewRect = new(inRect.x, inRect.y, inRect.width, inRect.height / 2 + 25f);
            Rect vROffset = new(0f, 0f, inRect.width - 20, inRect.height); // Adjust height here if more settings are added :)
            Widgets.BeginScrollView(viewRect, ref scrollPosition, vROffset, true);

            list.Begin(vROffset);
            list.Gap(12.0f);

            // GENERAL SETTINGS
            list.Label("<color=cyan>General</color>");
            list.Gap(3.00f);
            Texture2D partition1 = ContentFinder<Texture2D>.Get("UI/Settings/Partition", false);
            Rect parPos1 = vROffset.AtZero();
            parPos1.y = vROffset.yMin + 32f; // from top of rect
            parPos1.width = vROffset.width;
            parPos1.height = 12f;
            GUI.DrawTexture(parPos1, partition1, ScaleMode.StretchToFill, true);
            list.Gap(3.00f);
            list.CheckboxLabeled("AlienBiomes_SettingCrystallizing".Translate(), ref settings._allowCrystallizing, "AlienBiomes_SettingCrystallizingDesc".Translate());
            list.CheckboxLabeled("AlienBiomes_SettingReplaceSand".Translate(), ref settings._allowReplacingOfSand, "AlienBiomes_SettingReplaceSandDesc".Translate());
            list.CheckboxLabeled("AlienBiomes_SettingReplaceGravel".Translate(), ref settings._allowReplacingOfGravel, "AlienBiomes_SettingReplaceGravelDesc".Translate());
            list.Gap(12.0f);

            // GRAPHICS & PERFORMANCE SETTINGS
            list.Label("<color=cyan>Graphics</color> / <color=cyan>Performance</color>");
            list.Gap(3.00f);
            Texture2D partition2 = ContentFinder<Texture2D>.Get("UI/Settings/Partition", false);
            Rect parPos2 = vROffset.AtZero();
            parPos2.y = vROffset.yMin + 145f;
            parPos2.width = vROffset.width;
            parPos2.height = 12f;
            GUI.DrawTexture(parPos2, partition2, ScaleMode.StretchToFill, true);
            list.Gap(3.00f);
            list.CheckboxLabeled("AlienBiomes_SettingPlantGlow".Translate(), ref settings._showPlantGlow, "AlienBiomes_SettingPlantGlowDesc".Translate());
            list.CheckboxLabeled("AlienBiomes_SettingEffectorOverlay".Translate(), ref settings._showEffecterOverlay, "AlienBiomes_SettingEffectorOverlayDesc".Translate());
            list.CheckboxLabeled("AlienBiomes_SettingSpecialEffects".Translate(), ref settings._showSpecialEffects, "AlienBiomes_SettingSpecialEffectsDesc".Translate());
            list.Gap(12.0f);

            // AUDIO SETTINGS
            list.Label("<color=cyan>Audio</color>");
            list.Gap(3.00f);
            Texture2D partition3 = ContentFinder<Texture2D>.Get("UI/Settings/Partition", false);
            Rect parPos3 = vROffset.AtZero();
            parPos3.y = vROffset.yMin + 259f;
            parPos3.width = vROffset.width;
            parPos3.height = 12f;
            GUI.DrawTexture(parPos3, partition3, ScaleMode.StretchToFill, true);
            list.Gap(3.00f);
            list.CheckboxLabeled("AlienBiomes_SettingCompEffectSounds".Translate(), ref settings._allowCompEffectSounds, "AlienBiomes_SettingCompEffectSoundsDesc".Translate());
            list.Label(label: "AlienBiomes_PlantSoundEffectVolume".Translate((100f * settings._plantSoundEffectVolume).ToString("F0")), tooltip: "AlienBiomes_PlantSoundEffectVolumeDesc".Translate());
            settings._plantSoundEffectVolume = Mathf.Round(list.Slider(100f * settings._plantSoundEffectVolume, 0f, 100f)) / 100f;

            list.End();
            Widgets.EndScrollView();

            // Preview picture #1
            Texture2D tex1 = ContentFinder<Texture2D>.Get("UI/Settings/Texture3", false);
            Rect pos1 = inRect.AtZero();
            pos1.x = inRect.center.x - (pos1.width / 2f + 300f);
            pos1.y = inRect.center.y - (pos1.height / 2f - 350f);
            GUI.DrawTexture(pos1, tex1, ScaleMode.ScaleToFit, true);

            // Preview picture #2
            Texture2D tex2 = ContentFinder<Texture2D>.Get("UI/Settings/Texture2", false);
            Rect pos2 = inRect.AtZero();
            pos2.x = inRect.center.x - (pos2.width / 2f - 300f);
            pos2.y = inRect.center.y - (pos2.height / 2f - 350f);
            GUI.DrawTexture(pos2, tex2, ScaleMode.ScaleToFit, true);

            /*var oldBool = settings._allowCrystallizing;
            if (oldBool != true)
            {
                Texture2D tex1 = ContentFinder<Texture2D>.Get("UI/Settings/Texture3", false);
                Rect pos1 = inRect.AtZero();
                pos1.x = inRect.center.x - (pos1.width / 2f + 300f);
                pos1.y = inRect.center.y - (pos1.height / 2f - 350f);
                GUI.DrawTexture(pos1, tex1, ScaleMode.ScaleToFit, true);
            }*/
        }

        public override string SettingsCategory()
        {
            return "AlienBiomes_ModName".Translate();
        }
    }
}
