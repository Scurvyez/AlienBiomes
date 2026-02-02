using System.Text;
using Verse;

namespace AlienBiomes
{
    public class Comp_Explosive : ThingComp
    {
        public CompProperties_Explosive Props => (CompProperties_Explosive)props;
        
        public bool Exploded;
        
        private int _explosionCounter;

        public override void CompTickInterval(int delta)
        {
            base.CompTickInterval(delta);
            if (Exploded)
            {
                _explosionCounter++;
                if (_explosionCounter > Props.triggerReleaseCooldown)
                {
                    Exploded = false;
                }
            }
            else
            {
                _explosionCounter = 0;
            }
        }

        public override string CompInspectStringExtra()
        {
            StringBuilder stringBuilder = new ();

            if (Props?.triggeredDamageDef != null)
            {
                string exDmgEffRadFormatted = $"{Props.triggerRadius:F2}";
                stringBuilder.AppendLine("SZAB_PlantNasticHarmfulInfo"
                    .Translate(exDmgEffRadFormatted, Props.triggeredDamageDef.label));
                    
                string exGrowThrFormatted = $"{Props.triggerGrowthThreshold * 100f}";
                stringBuilder.AppendLine("SZAB_PlantNasticHarmfulInfo_ExThreshold"
                    .Translate(exGrowThrFormatted));
            }

            string baseInspectString = base.CompInspectStringExtra();
            if (!string.IsNullOrEmpty(baseInspectString))
            {
                stringBuilder.AppendLine(baseInspectString);
            }
            return stringBuilder.ToString().TrimEndNewlines();
        }

        public void TryDoExplosion()
        {
            GenExplosion.DoExplosion(parent.Position, parent.Map, Props.triggerRadius, 
                Props.triggeredDamageDef, instigator: null, damAmount: Props.triggeredDamageAmount.RandomInRange, 
                postExplosionSpawnThingCount: 0, screenShakeFactor: 0.02f);
        }
    }
}