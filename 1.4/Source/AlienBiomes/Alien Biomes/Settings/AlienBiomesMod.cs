using UnityEngine;
using Verse;
using RimWorld;
using System;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using System.Runtime.InteropServices;
using System.IO;

namespace AlienBiomes
{
    public class AlienBiomesMod : Mod
    {
        AlienBiomesSettings settings;
        private Vector2 scrollPosition = Vector2.zero;
        public static AlienBiomesMod mod;

        public AlienBiomesMod(ModContentPack content) : base(content)
        {
            mod = this;
            settings = GetSettings<AlienBiomesSettings>();
            var harmony = new Harmony("com.alienbiomes");

            harmony.Patch(original: AccessTools.PropertyGetter(typeof(ShaderTypeDef), nameof(ShaderTypeDef.Shader)),
                prefix: new HarmonyMethod(typeof(AlienBiomesMod),
                nameof(ShaderFromAssetBundle)));

            harmony.PatchAll();
        }

        /// <summary>
		/// Load shader asset for AlienBiomes shader types
		/// </summary>
		public static void ShaderFromAssetBundle(ShaderTypeDef __instance, ref Shader ___shaderInt)
        {
            if (__instance is ABShaderTypeDef)
            {
                ___shaderInt = AlienBiomesContentDatabase.AlienBiomesBundle.LoadAsset<Shader>(__instance.shaderPath);
                if (___shaderInt is null)
                {
                    Log.Error($"[<color=#4494E3FF>AlienBiomes</color>] Failed to load Shader from path <text>\"{__instance.shaderPath}\"</text>");
                }
            }
        }

        public AssetBundle MainBundle
        {
            get
            {
                string text = "";
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    text = "StandaloneOSX";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    text = "StandaloneWindows64";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    text = "StandaloneLinux64";
                }
                string bundlePath = Path.Combine(base.Content.RootDir, "Materials\\Bundles\\" + text + "\\alienbiomesbundle");
                //Log.Message("[<color=#4494E3FF>AlienBiomes</color>] Bundle Path: " + bundlePath);

                // Load the bundle as a local variable
                AssetBundle bundle = AssetBundle.LoadFromFile(bundlePath);

                // Check if the bundle is not null
                if (bundle == null)
                {
                    Log.Message("[<color=#4494E3FF>AlienBiomes</color>] Failed to load bundle at path: " + bundlePath);
                }

                foreach (var allAssetName in bundle.GetAllAssetNames())
                {
                    //Log.Message($"[<color=#4494E3FF>AlienBiomes</color>] - [{allAssetName}]");
                }

                // Return the bundle
                return bundle;
            }
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            Listing_Standard list = new();
            Rect viewRect = new(inRect.x, inRect.y, inRect.width, inRect.height / 2 + 25f); // Adjust height here if more settings are added :)
            Rect vROffset = new(0f, 0f, inRect.width - 20, inRect.height);
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
            list.CheckboxLabeled("AlienBiomes_SettingUseAlienSand".Translate(), ref settings._useAlienSand, "AlienBiomes_SettingUseAlienSandDesc".Translate());
            list.CheckboxLabeled("AlienBiomes_SettingUseAlienGravel".Translate(), ref settings._useAlienGravel, "AlienBiomes_SettingUseAlienGravelDesc".Translate());
            list.CheckboxLabeled("AlienBiomes_SettingUseAlienWater".Translate(), ref settings._useAlienWater, "AlienBiomes_SettingUseAlienWaterDesc".Translate());
            list.Gap(12.0f);

            // GRAPHICS & PERFORMANCE SETTINGS
            list.Label("<color=cyan>Graphics</color> / <color=cyan>Performance</color>");
            list.Gap(3.00f);
            Texture2D partition2 = ContentFinder<Texture2D>.Get("UI/Settings/Partition", false);
            Rect parPos2 = vROffset.AtZero();
            parPos2.y = vROffset.yMin + 170f;
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
            parPos3.y = vROffset.yMin + 284f;
            parPos3.width = vROffset.width;
            parPos3.height = 12f;
            GUI.DrawTexture(parPos3, partition3, ScaleMode.StretchToFill, true);
            list.Gap(3.00f);
            list.CheckboxLabeled("AlienBiomes_SettingCompEffectSounds".Translate(), ref settings._allowCompEffectSounds, "AlienBiomes_SettingCompEffectSoundsDesc".Translate());
            list.Label(label: "AlienBiomes_PlantSoundEffectVolume".Translate((100f * settings._plantSoundEffectVolume).ToString("F0")), tooltip: "AlienBiomes_PlantSoundEffectVolumeDesc".Translate());
            settings._plantSoundEffectVolume = Mathf.Round(list.Slider(100f * settings._plantSoundEffectVolume, 0f, 100f)) / 100f;

            list.End();
            Widgets.EndScrollView();

            // Left hand side image Rect to display example map gen with settings turned on.
            // Right hand side image Rect to display example map gen with settings turned off.

            list.Label("                                        <color=green>Example Map Gen</color>                                                                            <color=red>Boring Example Map Gen</color>");

            // Right Rect (Bottom)
            Rect useVanillaTerrainImg = inRect.AtZero();
            useVanillaTerrainImg.x = inRect.center.x - (useVanillaTerrainImg.width / 2f - 300f);
            useVanillaTerrainImg.y = inRect.center.y - (useVanillaTerrainImg.height / 2f - 350f);

            Texture2D tex2 = ContentFinder<Texture2D>.Get("UI/Settings/UseVanillaTerrainRight", false);
            GUI.DrawTexture(useVanillaTerrainImg, tex2, ScaleMode.ScaleToFit, true);

            // Left Rect (Bottom)
            Rect previewImg = inRect.AtZero();
            previewImg.x = inRect.center.x - (previewImg.width / 2f + 300f);
            previewImg.y = inRect.center.y - (previewImg.height / 2f - 350f);

            if (settings._useAlienSand == false && settings._useAlienGravel == false && settings._useAlienWater == false) {
                Texture2D tex1 = ContentFinder<Texture2D>.Get("UI/Settings/UseVanillaTerrainLeft", false);
                GUI.DrawTexture(previewImg, tex1, ScaleMode.ScaleToFit, true);
            }
            else if (settings._useAlienSand == true && settings._useAlienGravel == false && settings._useAlienWater == false) {
                Texture2D tex1 = ContentFinder<Texture2D>.Get("UI/Settings/UseAlienSand", false);
                GUI.DrawTexture(previewImg, tex1, ScaleMode.ScaleToFit, true);
            }
            else if (settings._useAlienSand == false && settings._useAlienGravel == true && settings._useAlienWater == false) {
                Texture2D tex1 = ContentFinder<Texture2D>.Get("UI/Settings/UseAlienGravel", false);
                GUI.DrawTexture(previewImg, tex1, ScaleMode.ScaleToFit, true);
            }
            else if (settings._useAlienSand == false && settings._useAlienGravel == false && settings._useAlienWater == true) {
                Texture2D tex1 = ContentFinder<Texture2D>.Get("UI/Settings/UseAlienWater", false);
                GUI.DrawTexture(previewImg, tex1, ScaleMode.ScaleToFit, true);
            }
            else if (settings._useAlienSand == true && settings._useAlienGravel == true && settings._useAlienWater == false) {
                Texture2D tex1 = ContentFinder<Texture2D>.Get("UI/Settings/UseAlienGravelAndSand", false);
                GUI.DrawTexture(previewImg, tex1, ScaleMode.ScaleToFit, true);
            }
            else if (settings._useAlienSand == false && settings._useAlienGravel == true && settings._useAlienWater == true) {
                Texture2D tex1 = ContentFinder<Texture2D>.Get("UI/Settings/UseAlienGravelAndWater", false);
                GUI.DrawTexture(previewImg, tex1, ScaleMode.ScaleToFit, true);
            }
            else if (settings._useAlienSand == true && settings._useAlienGravel == false && settings._useAlienWater == true) {
                Texture2D tex1 = ContentFinder<Texture2D>.Get("UI/Settings/UseAlienSandAndWater", false);
                GUI.DrawTexture(previewImg, tex1, ScaleMode.ScaleToFit, true);
            }
            else if (settings._useAlienSand == true && settings._useAlienGravel == true && settings._useAlienWater == true) {
                Texture2D tex1 = ContentFinder<Texture2D>.Get("UI/Settings/UseNoVanillaTerrain", false);
                GUI.DrawTexture(previewImg, tex1, ScaleMode.ScaleToFit, true);
            }
        }

        public override string SettingsCategory()
        {
            return "AlienBiomes_ModName".Translate();
        }
    }
}
