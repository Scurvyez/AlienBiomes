using System.Text;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class Comp_NasticInfo : ThingComp
    {
        public CompProperties_NasticInfo Props => (CompProperties_NasticInfo)props;
        private Plant_Nastic_ModExt _ext;
        
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            _ext = parent.def.GetModExtension<Plant_Nastic_ModExt>();
        }

        public override void PostDrawExtraSelectionOverlays()
        {
            if (_ext == null) return;
            GenDraw.DrawRadiusRing(parent.Position, _ext.effectRadius, _ext.hediffEffectRadiusColor);
        }
        
        public override string CompInspectStringExtra()
        {
            StringBuilder stringBuilder = new ();
            
            if (_ext != null)
            {
                if (_ext.explosionDamageDef != null)
                {
                    string exDmgEffRadFormatted = $"{_ext.explosionDamageEffectRadius:F2}";
                    stringBuilder.AppendLine("SZAB_PlantNasticHarmfulInfo"
                        .Translate(exDmgEffRadFormatted, _ext.explosionDamageDef.label));
                    
                    string exGrowThrFormatted = $"{_ext.explosionGrowthThreshold * 100f}";
                    stringBuilder.AppendLine("SZAB_PlantNasticHarmfulInfo_ExThreshold"
                        .Translate(exGrowThrFormatted));
                }
                else if (_ext.hediffToGive != null)
                {
                    string givesHeRadFormatted = $"{_ext.givesHediffRadius:F2}";
                    stringBuilder.AppendLine("SZAB_PlantNasticHediffGiverInfo"
                        .Translate(givesHeRadFormatted, _ext.hediffToGive.label));
                    
                    string heGrowThrFormatted = $"{_ext.givesHediffGrowthThreshold * 100f}";
                    stringBuilder.AppendLine("SZAB_PlantNasticHarmfulInfo_HeThreshold"
                        .Translate(heGrowThrFormatted, _ext.hediffChance * 100f));
                }
            }
            
            string baseInspectString = base.CompInspectStringExtra();
            if (!string.IsNullOrEmpty(baseInspectString))
            {
                stringBuilder.AppendLine(baseInspectString);
            }
            return stringBuilder.ToString().TrimEndNewlines();
        }
    }
}