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
        
        private const float HeaderTextGap = 3f;
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
                    nameof(ImpliedTerrainDefsPostfix)));
        }
        
        public static void ImpliedTerrainDefsPostfix(ref IEnumerable<TerrainDef> __result)
        {
            List<TerrainDef> modifiedList = [];
            foreach (TerrainDef def in __result)
            {
                if (!def.defName.Contains("_Smooth"))
                {
                    Plant_TerrainControl_ModExt plantTCExt = new()
                    {
                        terrainTags = ["Stony", "Rocky"]
                    };
                    
                    def.modExtensions ??= [];
                    def.modExtensions.Add(plantTCExt);
                    def.fertility = 0.3f;
                }
                modifiedList.Add(def);
            }
            __result = modifiedList;
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
            listLeft.Gap();
            
            listLeft.Label("General".Colorize(CategoryTextColor));
            listLeft.Gap(HeaderTextGap);
            
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
            listRight.Gap();
            
            listRight.Label("Graphics / Performance".Colorize(CategoryTextColor));
            listRight.Gap(HeaderTextGap);
            
            listRight.CheckboxLabeled("SZAB_SettingPlantGlow".Translate(), 
                ref _settings._showPlantGlow, "SZAB_SettingPlantGlowDesc".Translate());
            listRight.Gap();
            
            listRight.Label("Audio".Colorize(CategoryTextColor));
            listRight.Gap(HeaderTextGap);
            
            listRight.CheckboxLabeled("SZAB_SettingCompEffectSounds".Translate(), 
                ref _settings._allowCompEffectSounds, "SZAB_SettingCompEffectSoundsDesc".Translate());
            
            listRight.Label(label: "SZAB_SettingPlantSFXChance".Translate(
                    (100f * _settings._plantSFXChance).ToString("F0")), 
                tooltip: "SZAB_SettingPlantSFXChanceDesc".Translate());
            _settings._plantSFXChance = Mathf.Round(listRight.Slider(
                100f * _settings._plantSFXChance, 0f, 100f)) / 100f;
            
            listRight.End();
            Widgets.EndScrollView();
        }
    }
}