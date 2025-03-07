using System.Text;
using Verse;

namespace AlienBiomes
{
    public class Comp_NasticInfo : ThingComp
    {
        public CompProperties_NasticInfo Props => (CompProperties_NasticInfo)props;
        private PlantNastic_ModExtension plantExt;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            plantExt = parent.def.GetModExtension<PlantNastic_ModExtension>();
        }

        public override string CompInspectStringExtra()
        {
            StringBuilder stringBuilder = new ();

            if (plantExt != null)
            {
                if (plantExt.explosionDamageDef != null)
                {
                    string effectRadiusFormatted = $"{plantExt.explosionDamageEffectRadius:F2}";
                    stringBuilder.AppendLine("SZ_PlantNasticHarmfulInfo"
                        .Translate(effectRadiusFormatted, plantExt.explosionDamageDef.label));
                }
                else if (plantExt.givesHediff)
                {
                    string effectRadiusFormatted = $"{plantExt.effectRadius:F2}";
                    stringBuilder.AppendLine("SZ_PlantNasticHediffGiverInfo"
                        .Translate(effectRadiusFormatted, plantExt.hediffToGive.label));
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