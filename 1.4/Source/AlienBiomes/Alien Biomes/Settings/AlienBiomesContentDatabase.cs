using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    [StaticConstructorOnStartup]
    public static class AlienBiomesContentDatabase
    {
        private static AssetBundle bundleInt;
        private static Dictionary<string, Shader> lookupShaders;
        public static readonly Shader TransparentPlantShimmer = LoadShader("Assets/TransparentPlantShimmer.shader");
        
        public static AssetBundle AlienBiomesBundle
        {
            get
            {
                if (bundleInt == null)
                {
                    bundleInt = AlienBiomesMod.mod.MainBundle;
                    Log.Message("[<color=#4494E3FF>AlienBiomes</color>] bundleInt: " + bundleInt.name);
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
                Log.Message("[<color=#4494E3FF>AlienBiomes</color>] lookupShaders: " + lookupShaders.ToList().Count);
                lookupShaders[shaderName] = AlienBiomesBundle.LoadAsset<Shader>(shaderName);
            }
            Shader shader = lookupShaders[shaderName];
            if (shader == null)
            {
                Log.Warning("[<color=#4494E3FF>AlienBiomes</color>] Could not load shader " + shaderName);
                return ShaderDatabase.Cutout;
            }
            return shader;
        }
    }
}
