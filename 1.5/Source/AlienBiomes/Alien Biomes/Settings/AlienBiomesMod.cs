using System;
using UnityEngine;
using Verse;
using RimWorld;
using HarmonyLib;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections.Generic;

namespace AlienBiomes
{
    public class AlienBiomesMod : Mod
    {
        public static AlienBiomesMod ABMod;
        
        private readonly AlienBiomesSettings _settings;
        private Vector2 _scrollPos = Vector2.zero;
        
        public AlienBiomesMod(ModContentPack content) : base(content)
        {
            ABMod = this;
            _settings = GetSettings<AlienBiomesSettings>();
            
            Harmony harmony = new (id: "rimworld.scurvyez.alienbiomes-preSCOS");
            
            // implied defs are generated before SCOS runs
            // so this patch needs to be run before then, hence why it's here
            harmony.Patch(original: AccessTools.Method(typeof(TerrainDefGenerator_Stone), 
                    nameof(TerrainDefGenerator_Stone.ImpliedTerrainDefs)),
                postfix: new HarmonyMethod(typeof(AlienBiomesMod), 
                    nameof(AddBiomesDefModExtensionsPostfix)));
        }
        
        public static IEnumerable<TerrainDef> AddBiomesDefModExtensionsPostfix(IEnumerable<TerrainDef> __result)
        {
            foreach (TerrainDef terrainDef in __result)
            {
                Plant_TerrainControl_ModExt plantTerrainControlModExt = new();
                plantTerrainControlModExt.terrainTags.Add("Stony");
                plantTerrainControlModExt.terrainTags.Add("Rocky");
                terrainDef.modExtensions ??= [];
                terrainDef.modExtensions.Add(plantTerrainControlModExt);
                terrainDef.fertility = 0.35f;
                yield return terrainDef;
            }
        }
        
        public AssetBundle MainBundle
        {
            get
            {
                string text = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "StandaloneOSX"
                    : RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "StandaloneWindows64"
                    : RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "StandaloneLinux64"
                    : throw new PlatformNotSupportedException("Unsupported Platform");
                
                string bundlePath = Path.Combine(Content.RootDir, 
                    @"Materials\\Bundles\\" + text + "\\alienbiomesbundle");
                //ABLog.Message("Bundle Path: " + bundlePath);
                
                AssetBundle bundle = AssetBundle.LoadFromFile(bundlePath);
                if (bundle == null)
                {
                    ABLog.Message("Failed to load bundle at path: " + bundlePath);
                }
                
                // foreach (string allAssetName in bundle.GetAllAssetNames())
                // {
                //     ABLog.Message($" - {allAssetName}");
                // }
                return bundle;
            }
        }
        
        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            Listing_Standard list = new();
            Rect viewRect = new(inRect.x, inRect.y, inRect.width, inRect.height / 2);
            Rect vROffset = new(0f, 0f, inRect.width - 20, inRect.height - 225); // Adjust last value here for more height :)
            Widgets.BeginScrollView(viewRect, ref _scrollPos, vROffset, true);

            list.Begin(vROffset);
            list.Gap();

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
            
            list.CheckboxLabeled("AlienBiomes_SettingCrystallizing".Translate(),
                ref _settings._allowCrystallizing, "AlienBiomes_SettingCrystallizingDesc".Translate());
            list.CheckboxLabeled("AlienBiomes_SettingUseAlienSand".Translate(), 
                ref _settings._useAlienSand, "AlienBiomes_SettingUseAlienSandDesc".Translate());
            list.CheckboxLabeled("AlienBiomes_SettingUseAlienGravel".Translate(), 
                ref _settings._useAlienGravel, "AlienBiomes_SettingUseAlienGravelDesc".Translate());
            list.CheckboxLabeled("AlienBiomes_SettingUseAlienWater".Translate(), 
                ref _settings._useAlienWater, "AlienBiomes_SettingUseAlienWaterDesc".Translate());
            list.CheckboxLabeled("AlienBiomes_SettingShowTerrainDebris".Translate(), 
                ref _settings._showTerrainDebris, "AlienBiomes_SettingShowTerrainDebrisDesc".Translate());
            list.Gap();
            
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

            list.CheckboxLabeled("AlienBiomes_SettingPlantGlow".Translate(), 
                ref _settings._showPlantGlow, "AlienBiomes_SettingPlantGlowDesc".Translate());
            list.CheckboxLabeled("AlienBiomes_SettingEffectorOverlay".Translate(), 
                ref _settings._showEffecterOverlay, "AlienBiomes_SettingEffectorOverlayDesc".Translate());
            list.CheckboxLabeled("AlienBiomes_EnableScreenPositionEffects".Translate(), 
                ref _settings._enableScreenPosEffects, "AlienBiomes_EnableScreenPositionEffectsDesc".Translate());
            list.CheckboxLabeled("AlienBiomes_SettingSpecialEffects".Translate(),
                ref _settings._showSpecialEffects, "AlienBiomes_SettingSpecialEffectsDesc".Translate());
            list.Gap();

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

            list.CheckboxLabeled("AlienBiomes_SettingCompEffectSounds".Translate(), 
                ref _settings._allowCompEffectSounds, "AlienBiomes_SettingCompEffectSoundsDesc".Translate());
            list.Label(label: "AlienBiomes_PlantSoundEffectVolume".Translate(
                    (100f * _settings._plantSoundEffectVolume).ToString("F0")), 
                tooltip: "AlienBiomes_PlantSoundEffectVolumeDesc".Translate());
            _settings._plantSoundEffectVolume = Mathf.Round(list.Slider(
                100f * _settings._plantSoundEffectVolume, 0f, 100f)) / 100f;

            list.End();
            Widgets.EndScrollView();

            /*
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
            */
        }

        public override string SettingsCategory()
        {
            return "AlienBiomes_ModName".Translate();
        }
    }
}