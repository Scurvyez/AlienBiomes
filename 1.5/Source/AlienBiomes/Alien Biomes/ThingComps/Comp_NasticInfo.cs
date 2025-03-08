using System.Text;
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
        
        public override string CompInspectStringExtra()
        {
            StringBuilder stringBuilder = new ();
            
            if (_ext != null)
            {
                if (_ext.explosionDamageDef != null)
                {
                    string effectRadiusFormatted = $"{_ext.explosionDamageEffectRadius:F2}";
                    stringBuilder.AppendLine("SZ_PlantNasticHarmfulInfo"
                        .Translate(effectRadiusFormatted, _ext.explosionDamageDef.label));
                }
                else if (_ext.givesHediff)
                {
                    string effectRadiusFormatted = $"{_ext.effectRadius:F2}";
                    stringBuilder.AppendLine("SZ_PlantNasticHediffGiverInfo"
                        .Translate(effectRadiusFormatted, _ext.hediffToGive.label));
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