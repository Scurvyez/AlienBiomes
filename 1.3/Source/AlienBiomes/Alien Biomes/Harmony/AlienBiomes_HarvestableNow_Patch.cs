using HarmonyLib;
using RimWorld;

namespace AlienBiomes
{
    [HarmonyPatch(typeof(Plant), "HarvestableNow", MethodType.Getter)]
    public static class AlienBiomes_HarvestableNow_Patch
    {
        public static void HarvestableNowPostFix(Comp_TimedHarvest __instance, ref bool __result)
        {
            var comp = __instance.GetComp<Comp_TimedHarvest>();
            if (comp != null)
            {
                if (__instance is Comp_TimedHarvest harvest) __result = __result && harvest.AdditionalPlantHarvestLogic();
            }
        }
    }
}
