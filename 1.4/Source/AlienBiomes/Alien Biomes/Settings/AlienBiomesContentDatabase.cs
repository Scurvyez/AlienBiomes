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
        public static readonly Shader TransparentPlantShimmer = LoadShader("TransparentPlantShimmer");
        
        public static AssetBundle AlienBiomesBundle
        {
            get
            {
                if (bundleInt == null)
                {
                    bundleInt = AlienBiomesMod.mod.MainBundle;
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
                lookupShaders[shaderName] = AlienBiomesBundle.LoadAsset<Shader>(shaderName);
            }
            Shader shader = lookupShaders[shaderName];
            if (shader == null)
            {
                Log.Warning("[AB] Could not load shader " + shaderName);
                return ShaderDatabase.Cutout;
            }
            return shader;
        }
    }
}
