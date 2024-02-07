using HarmonyLib;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    /*
    [StaticConstructorOnStartup]
    [HarmonyPatch(typeof(Current), "Notify_LoadedSceneChanged")]
    public class CurrentNotify_LoadedSceneChanged_Postfix
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            GameObject cameraObject;

            if (GenScene.InPlayScene)
            {
                cameraObject = Find.Camera.gameObject;
                cameraObject.AddComponent<FullScreenEffects>();
            }
        }
    }
    */
}
