using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    [StaticConstructorOnStartup]
    public static class AlienBiomesContentDatabase
    {
        public static readonly Shader TransparentPlantShimmer = 
            LoadShader(Path.Combine("Assets", "TransparentPlantShimmer.shader"));
        public static readonly Shader TransparentPlantPulse = 
            LoadShader(Path.Combine("Assets", "TransparentPlantPulse.shader"));
        public static readonly Shader TransparentPlantFloating = 
            LoadShader(Path.Combine("Assets", "TransparentPlantFloating.shader"));
        public static readonly Shader TransparentPlantPlus = 
            LoadShader(Path.Combine("Assets", "TransparentPlantPlus.shader"));
        
        private static AssetBundle _bundleInt;
        private static Dictionary<string, Shader> _lookupShaders;
        
        public static AssetBundle AlienBiomesBundle
        {
            get
            {
                if (_bundleInt != null) return _bundleInt;
                _bundleInt = AlienBiomesMod.ABMod.MainBundle;
                //ABLog.Message("bundleInt: " + _bundleInt.name);
                return _bundleInt;
            }
        }

        private static Shader LoadShader(string shaderName)
        {
            _lookupShaders ??= new Dictionary<string, Shader>();
            if (!_lookupShaders.ContainsKey(shaderName))
            {
                //ABLog.Message("lookupShaders: " + _lookupShaders.ToList().Count);
                _lookupShaders[shaderName] = AlienBiomesBundle.LoadAsset<Shader>(shaderName);
            }
            
            Shader shader = _lookupShaders[shaderName];
            if (shader == null)
            {
                ABLog.Warning("Could not load shader: " + shaderName);
                return ShaderDatabase.DefaultShader;
            }
            
            if (shader != null)
            {
                //ABLog.Message("Loaded shader: " + shaderName);
            }
            return shader;
        }
    }
}