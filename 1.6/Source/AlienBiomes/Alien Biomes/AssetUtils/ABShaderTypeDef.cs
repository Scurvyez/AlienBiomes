using System;
using JetBrains.Annotations;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    [UsedImplicitly]
    public class ABShaderTypeDef : ShaderTypeDef
    {
        public string actualShaderName = "";
        
        public override void PostLoad()
        {
            if (shaderPath.NullOrEmpty()) return;

            LongEventHandler.ExecuteWhenFinished(() =>
            {
                try
                {
                    Shader found = ContentFinder<Shader>.TryFindAssetInModBundles(actualShaderName);

                    if (found == null)
                    {
                        var bundles = modContentPack?.assetBundles?.loadedAssetBundles;
                        if (bundles != null)
                        {
                            for (int b = 0; b < bundles.Count; b++)
                            {
                                var bundle = bundles[b];
                                if (bundle == null) continue;

                                var shaders = bundle.LoadAllAssets<Shader>();
                                if (shaders == null) continue;

                                for (int i = 0; i < shaders.Length; i++)
                                {
                                    var s = shaders[i];
                                    if (s != null && s.name == actualShaderName)
                                    {
                                        found = s;
                                        break;
                                    }
                                }

                                if (found != null) break;
                            }
                        }
                    }

                    if (found == null)
                    {
                        ABLog.Warning($"[AlienBiomes] Could not find shader by name '{actualShaderName}' in mod bundles.");
                        return;
                    }

                    ShaderDatabase.lookup[shaderPath] = found;
                    ShaderDatabase.lookup[found.name] = found;

                    ABLog.Message($"Shader registered. lookup['{shaderPath}'] -> '{found.name}'");
                }
                catch (Exception e)
                {
                    ABLog.Error($"Failed to register shader '{shaderPath}': {e}");
                }
            });
        }
    }
}