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

        private const float _newListingGap = 12f;
        private const float _newSectionGap = 6f;
        private const float _headerTextGap = 3f;
        
        private readonly AlienBiomesSettings _settings;
        private float _halfWidth;
        private Vector2 _leftScrollPos = Vector2.zero;
        private Vector2 _rightScrollPos = Vector2.zero;
        
        private static Color CategoryTextColor => ABLog.MessageMsgCol;
        public override string SettingsCategory() => "SZAB_ModName".Translate();
        
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
        
        public static IEnumerable<TerrainDef> AddBiomesDefModExtensionsPostfix(
            IEnumerable<TerrainDef> __result)
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
            _halfWidth = (inRect.width - 30f) / 2;
            LeftSideScrollViewHandler(new Rect(inRect.x, inRect.y, _halfWidth, inRect.height));
            RightSideScrollViewHandler(new Rect(inRect.x + _halfWidth + 20, inRect.y, _halfWidth, inRect.height));
        }
        
        private void LeftSideScrollViewHandler(Rect inRect)
        {
            Listing_Standard listLeft = new();
            Rect viewRectLeft = new(inRect.x, inRect.y, inRect.width, inRect.height);
            Rect vROffsetLeft = new(0, 0, inRect.width - 20, inRect.height);
            
            Widgets.BeginScrollView(viewRectLeft, ref _leftScrollPos, vROffsetLeft);
            listLeft.Begin(vROffsetLeft);
            listLeft.Gap(_newListingGap);
            
            listLeft.Label("General".Colorize(CategoryTextColor));
            listLeft.Gap(_headerTextGap);
            
            listLeft.CheckboxLabeled("SZAB_SettingCrystallizing".Translate(),
                ref _settings._allowCrystallizing, "SZAB_SettingCrystallizingDesc".Translate());
            listLeft.CheckboxLabeled("SZAB_SettingShowTerrainDebris".Translate(), 
                ref _settings._showTerrainDebris, "SZAB_SettingShowTerrainDebrisDesc".Translate());
            
            listLeft.End();
            Widgets.EndScrollView();
        }
        
        private void RightSideScrollViewHandler(Rect inRect)
        {
            Listing_Standard listRight = new ();
            Rect viewRectRight = new(inRect.x, inRect.y, inRect.width, inRect.height);
            Rect vROffsetRight = new(0, 0, inRect.width - 20, inRect.height);
            
            Widgets.BeginScrollView(viewRectRight, ref _rightScrollPos, vROffsetRight);
            listRight.Begin(vROffsetRight);
            listRight.Gap(_newListingGap);
            
            listRight.Label("Graphics / Performance".Colorize(CategoryTextColor));
            listRight.Gap(_headerTextGap);
            
            listRight.CheckboxLabeled("SZAB_SettingPlantGlow".Translate(), 
                ref _settings._showPlantGlow, "SZAB_SettingPlantGlowDesc".Translate());
            listRight.Gap(_newListingGap);
            
            listRight.Label("Audio".Colorize(CategoryTextColor));
            listRight.Gap(_headerTextGap);
            
            listRight.CheckboxLabeled("SZAB_SettingCompEffectSounds".Translate(), 
                ref _settings._allowCompEffectSounds, "SZAB_SettingCompEffectSoundsDesc".Translate());
            listRight.Label(label: "SZAB_PlantSoundEffectVolume".Translate(
                    (100f * _settings._plantSoundEffectVolume).ToString("F0")), 
                tooltip: "SZAB_PlantSoundEffectVolumeDesc".Translate());
            _settings._plantSoundEffectVolume = Mathf.Round(listRight.Slider(
                100f * _settings._plantSoundEffectVolume, 0f, 100f)) / 100f;
            
            listRight.End();
            Widgets.EndScrollView();
        }
    }
}