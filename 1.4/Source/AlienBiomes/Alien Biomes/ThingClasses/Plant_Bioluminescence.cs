using RimWorld;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class Plant_Bioluminescence : Plant
    {
        private MaterialPropertyBlock propertyBlock;
        private Color randomColor;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);

            RandomColor();
            propertyBlock = new MaterialPropertyBlock();
            if (!respawningAfterLoad && propertyBlock != null && def.graphicData.shaderType.Shader == ShaderDatabase.MoteGlowDistorted)
            {
                propertyBlock.SetColor("_Color", randomColor);
            }
        }

        private Color RandomColor()
        {
            float r = Random.Range(0.7f, 1f);
            float g = Random.Range(0.7f, 1f);
            float b = Random.Range(0.7f, 1f);
            float a = Random.Range(0.2f, 0.9f);

            randomColor = new(r, g, b, a);
            return randomColor;
        }
    }
}
