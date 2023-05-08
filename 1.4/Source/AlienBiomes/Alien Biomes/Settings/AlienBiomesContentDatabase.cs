using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    [StaticConstructorOnStartup]
    public static class AlienBiomesContentDatabase
    {
        private static AssetBundle bundleInt;
        private static Dictionary<string, Shader> lookupShaders;
        public static readonly Shader TransparentPlantShimmer = LoadShader(Path.Combine("Assets", "TransparentPlantShimmer.shader"));
        public static readonly Shader TransparentPlantPulse = LoadShader(Path.Combine("Assets", "TransparentPlantPulse.shader"));
        
        public static AssetBundle AlienBiomesBundle
        {
            get
            {
                if (bundleInt == null)
                {
                    bundleInt = AlienBiomesMod.mod.MainBundle;
                    //Log.Message("[<color=#4494E3FF>AlienBiomes</color>] bundleInt: " + bundleInt.name);
                }
                return bundleInt;
            }
        }

        public static Shader LoadShader(string shaderName)
        {
            if (lookupShaders == null)
            {
                lookupShaders = new Dictionary<string, Shader>();
            }
            if (!lookupShaders.ContainsKey(shaderName))
            {
                //Log.Message("[<color=#4494E3FF>AlienBiomes</color>] lookupShaders: " + lookupShaders.ToList().Count);
                lookupShaders[shaderName] = AlienBiomesBundle.LoadAsset<Shader>(shaderName);
            }
            Shader shader = lookupShaders[shaderName];
            if (shader == null)
            {
                Log.Warning("[<color=#4494E3FF>AlienBiomes</color>] Could not load shader: " + shaderName);
                return ShaderDatabase.DefaultShader;
            }
            if (shader != null)
            {
                Log.Message("[<color=#4494E3FF>AlienBiomes</color>] Loaded shader: " + shaderName);
            }
            return shader;
        }
    }
}
