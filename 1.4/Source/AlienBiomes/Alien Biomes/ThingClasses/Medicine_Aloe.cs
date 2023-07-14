using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace AlienBiomes
{
    /*
    public class Medicine_Aloe : Medicine
    {
        private static List<Hediff> tendableHediffsInTendPriorityOrder = new ();
        //private static List<Hediff> tmpHediffs = new ();

        public static new int GetMedicineCountToFullyHeal(Pawn pawn)
        {
            int num1 = 0;
            int num2 = pawn.health.hediffSet.hediffs.Count + 1;
            tendableHediffsInTendPriorityOrder.Clear();

            List<Hediff> hediffs = new ();
            for (int i = 0; i < hediffs.Count; ++i)
            {
                //if (hediffs[i].TendableNow() && hediffs[i].Label is "cut")tendableHediffsInTendPriorityOrder.Add(hediffs[i]);
                if (hediffs[i].TendableNow()) {
                    if (hediffs[i].Label is "scratch" || hediffs[i].Label is "burn") {
                        tendableHediffsInTendPriorityOrder.Add(hediffs[i]);
                    }
                }
            }

            TendUtility.SortByTendPriority(tendableHediffsInTendPriorityOrder);
            int num3 = 0;
            label_6:
            ++num1;

            if (num1 > num2)
            {
                Log.Error("Too many iterations.");
            }
            else
            {
                TendUtility.GetOptimalHediffsToTendWithSingleTreatment(pawn, true, hediffs, tendableHediffsInTendPriorityOrder);
                if (hediffs.Any())
                {
                    ++num3;
                    for (int i = 0; i < hediffs.Count; ++i)
                        tendableHediffsInTendPriorityOrder.Remove(hediffs[i]);
                    goto label_6;
                }
            }
            hediffs.Clear();
            tendableHediffsInTendPriorityOrder.Clear();
            return num3;
        }
    }
    */
}
