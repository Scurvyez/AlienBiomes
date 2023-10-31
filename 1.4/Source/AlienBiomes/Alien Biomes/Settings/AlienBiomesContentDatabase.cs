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
        private static Dictionary<string, Material> lookupMaterials;
        public static readonly Shader TransparentPlantShimmer = LoadShader(Path.Combine("Assets", "TransparentPlantShimmer.shader"));
        public static readonly Shader TransparentPlantPulse = LoadShader(Path.Combine("Assets", "TransparentPlantPulse.shader"));
        public static readonly Shader TransparentPlantFloating = LoadShader(Path.Combine("Assets", "TransparentPlantFloating.shader"));
        public static readonly Shader ImprovedFire = LoadShader(Path.Combine("Assets", "ImprovedFire.shader"));
        public static readonly Shader TransparentAlphaToggle = LoadShader(Path.Combine("Assets", "TransparentAlphaToggle.shader"));

        public static AssetBundle AlienBiomesBundle
        {
            get
            {
                if (bundleInt == null)
                {
                    bundleInt = AlienBiomesMod.mod.MainBundle;
                    //Log.Message("[<color=#4494E3FF>AlienBiomes</color>] <color=#e36c45FF>bundleInt:</color> " + bundleInt.name);
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
                Log.Warning("[<color=#4494E3FF>AlienBiomes</color>] <color=#e36c45FF>Could not load shader:</color> " + shaderName);
                return ShaderDatabase.DefaultShader;
            }
            if (shader != null)
            {
                Log.Message("[<color=#4494E3FF>AlienBiomes</color>] Loaded shader: " + shaderName);
            }
            return shader;
        }

        public static Material LoadMaterial(string materialName)
        {
            if (lookupMaterials == null)
            {
                lookupMaterials = new Dictionary<string, Material>();
            }
            if (!lookupMaterials.ContainsKey(materialName))
            {
                lookupMaterials[materialName] = AlienBiomesBundle.LoadAsset<Material>(materialName);
            }
            Material material = lookupMaterials[materialName];
            if (material == null)
            {
                Log.Warning("[<color=#4494E3FF>AlienBiomes</color>] <color=#e36c45FF>Could not load material:</color> " + materialName);
                return BaseContent.BadMat;
            }
            return material;
        }
    }
}
