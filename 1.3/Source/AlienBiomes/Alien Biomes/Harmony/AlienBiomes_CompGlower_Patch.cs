using HarmonyLib;
using Verse;

namespace AlienBiomes
{
    [HarmonyPatch(typeof(CompGlower), "ShouldBeLitNow", MethodType.Getter)]
    public static class AlienBiomes_CompGlower_Patch
    {
        public static void Postfix(CompGlower __instance, ref bool __result)
        {
            if (__instance is Comp_TimedGlower glower) __result = __result && glower.AdditionalGlowerLogic();
        }
    }
}
