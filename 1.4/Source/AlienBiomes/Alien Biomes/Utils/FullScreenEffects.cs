using UnityEngine;

namespace AlienBiomes
{
    public class FullScreenEffects : MonoBehaviour
    {
        public static FullScreenEffects instance;
        public Material screenPosEffectsMat;

        public void Start()
        {
            instance = this;
            screenPosEffectsMat = new Material(AlienBiomesContentDatabase.ScreenPosEffects);
        }

        public void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            
            if (AlienBiomesSettings.EnableScreenPosEffects)
            {
                Graphics.Blit(source, destination, screenPosEffectsMat);
            }
            else
            {
                Graphics.Blit(source, destination);
            }
        }
    }
}
