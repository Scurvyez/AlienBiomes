using System.Collections.Generic;
using Verse;
using UnityEngine;

namespace AlienBiomes
{
    public class Geyser_GraphicPerTerrain : DefModExtension
    {
        public List<TerrainDef> terrains;
        public Graphic[] newGraphics;
        public List<string> newGraphicPaths;
        bool initialized = false;
        
        public void Initialize()
        {
            var count = terrains.Count;
            newGraphics = new Graphic[count];

            for (int i = 0; i < count; i++)
            {
                if (newGraphicPaths[i] != null)
                    newGraphics[i] = GraphicDatabase.Get(typeof(Graphic_Single), newGraphicPaths[i], ShaderDatabase.Transparent, Vector2.one, Color.white, Color.white);
            }
            initialized = true;
        }

        public Graphic NewGeyserGraphic(TerrainDef geyser)
        {
            if (!initialized)
                Initialize();
            var geyserIndex = terrains.IndexOf(geyser);
            if (geyserIndex == -1 || newGraphics[geyserIndex] == null)
                return null;
            return newGraphics[geyserIndex];
        }
    }
}
