using HarmonyLib;
using RimWorld;

namespace AlienBiomes
{
    [HarmonyPatch(typeof(Plant), "HarvestableNow", MethodType.Getter)]
    public static class AlienBiomes_HarvestableNow_Patch
    {
        [HarmonyPostfix]
        public static void HarvestableNowPostFix(Plant __instance, ref bool __result)
        {
            //Log.Message("Hello from the patch..");
            var comp = __instance.GetComp<Comp_TimedHarvest>();
            if (comp != null)
            {
                //Log.Message("Hello from the comp not being null in the patch!");
                __result = __result && comp.AdditionalPlantHarvestLogic();
            }
        }
    }
}
