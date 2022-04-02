using HarmonyLib;
using Verse;

namespace AlienBiomes
{
    [HarmonyPatch(typeof(CompGlower), "ShouldBeLitNow", MethodType.Getter)]
    public static class CompGlower_Patch
    {
        /// <summary>
        /// Checks for an instance of CompGlower.
        /// If found, additional logic is applied to the method.
        /// </summary>
        public static void Postfix(CompGlower __instance, ref bool __result)
        {
            if (__instance is Comp_TimedGlower glower) __result = __result 
                    && AlienBiomesSettings.ShowPlantGlow && glower.AdditionalGlowerLogic();
        }
    }
}
