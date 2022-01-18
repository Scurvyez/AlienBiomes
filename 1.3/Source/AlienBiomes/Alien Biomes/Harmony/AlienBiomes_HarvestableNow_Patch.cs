using HarmonyLib;
using RimWorld;
using Verse;

namespace AlienBiomes
{
    [HarmonyPatch(typeof(Plant), "HarvestableNow", MethodType.Getter)]
    public static class AlienBiomes_HarvestableNow_Patch
    {
        [HarmonyPostfix]
        public static void HarvestableNowPostFix(Plant __instance, ref bool __result)
        {
            var comp = __instance.GetComp<Comp_TimedHarvest>();
            if (comp != null)
                __result = __result && comp.AdditionalPlantHarvestLogic();
        }
    }
}
