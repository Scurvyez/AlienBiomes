using System.Text;
using Verse;

namespace AlienBiomes
{
    public class Comp_HediffGiver : ThingComp
    {
        public CompProperties_HediffGiver Props => (CompProperties_HediffGiver)props;

        public bool Triggered;
        
        private int _triggerCounter;
        
        public override void CompTickInterval(int delta)
        {
            base.CompTickInterval(delta);
            if (Triggered)
            {
                _triggerCounter++;
                if (_triggerCounter > Props.triggerReleaseCooldown)
                {
                    Triggered = false;
                }
            }
            else
            {
                _triggerCounter = 0;
            }
        }
        
        public override string CompInspectStringExtra()
        {
            StringBuilder stringBuilder = new ();

            if (Props.hediffToGive != null)
            {
                string givesHeRadFormatted = $"{Props.triggerRadius:F2}";
                stringBuilder.AppendLine("SZAB_PlantNasticHediffGiverInfo"
                    .Translate(givesHeRadFormatted, Props.hediffToGive.label));
                    
                string heGrowThrFormatted = $"{Props.triggerGrowthThreshold * 100f}";
                stringBuilder.AppendLine("SZAB_PlantNasticHarmfulInfo_HeThreshold"
                    .Translate(heGrowThrFormatted, Props.chanceToGive * 100f));
            }

            string baseInspectString = base.CompInspectStringExtra();
            if (!string.IsNullOrEmpty(baseInspectString))
            {
                stringBuilder.AppendLine(baseInspectString);
            }
            return stringBuilder.ToString().TrimEndNewlines();
        }
        
        public void TryGiveHediff(Pawn pawn)
        {
            if (!Rand.Chance(Props.chanceToGive)) return;
            if (pawn.NonHumanlikeOrWildMan() || pawn.IsColonyMech) return;
            
            if (Props.hediffToGive == InternalDefOf.SZ_Crystallize && AlienBiomesSettings.AllowCrystallizing)
            {
                // give to a specific part maybe?
                pawn.health.AddHediff(Props.hediffToGive);
                HealthUtility.AdjustSeverity(pawn, Props.hediffToGive, 0.01f);
                
                if (!pawn.IsColonist) return;
                Find.LetterStack.ReceiveLetter("SZAB_LetterLabelCrystallizing".Translate(), 
                    "SZAB_LetterCrystallizing".Translate(pawn), InternalDefOf.SZ_PawnCrystallizingLetter);
            }
        }
    }
}