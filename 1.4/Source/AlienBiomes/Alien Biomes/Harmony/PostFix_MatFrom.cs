using System;
using HarmonyLib;
using Verse;
using UnityEngine;

namespace AlienBiomes
{
    [HarmonyPatch(typeof(MaterialPool), "MatFrom", new Type[] { typeof(MaterialRequest) })]
    public static class MatFrom_Patch
    {
        [HarmonyPostfix]
        public static void MatFromPostFix(MaterialRequest req, ref Material __result)
        {
            if (__result != null && __result.shader == AlienBiomesContentDatabase.TransparentPlantShimmer)
            {
                //Log.Message("[<color=#4494E3FF>AlienBiomes</color>] MatFrom_Patch: Material shader = " + __result.shader.name);
                WindManager.Notify_PlantMaterialCreated(__result);
            }
        }
    }
}
