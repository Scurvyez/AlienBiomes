using HarmonyLib;
using RimWorld;
using Verse;

namespace AlienBiomes
{
    [HarmonyPatch(typeof(Plant), "HarvestableNow", MethodType.Getter)]
    public static class AlienBiomes_HarvestableNow_Patch
    {
        public static void HarvestableNowPostFix(Comp_TimedHarvest __instance, ref bool __result)
        {
            var comp = (Comp_TimedHarvest)__instance.GetComp<Comp_TimedHarvest>();
            Log.Message("boom" + comp.GetType().FullName);
            if (comp != null)
            {
                __result = __result && comp.AdditionalPlantHarvestLogic();
            }
        }
    }
}
