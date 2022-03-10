using System.Collections.Generic;
using Verse;
using UnityEngine;
using RimWorld;

namespace AlienBiomes
{
    public class Geyser_GraphicPerTerrain : DefModExtension
    {
        public List<TerrainDef> terrains;
        public Graphic[] graphics;
        public List<string> graphicPaths;
        bool initialized = false;
        
        public void Initialize()
        {
            //Log.Warning("INITIALIZED");
            var count = terrains.Count;
            graphics = new Graphic[count];

            for (int i = 0; i < count; i++)
            {
                if (graphicPaths[i] != null)
                    graphics[i] = GraphicDatabase.Get(typeof(Graphic_Single), graphicPaths[i], ShaderDatabase.Transparent, Vector2.one, Color.white, Color.white);
            }
            initialized = true;
        }

        public Graphic GeyserGraphicPerTerrain(TerrainDef terrain)
        {
            //Log.Warning("FOUND TERRAIN: " + terrain);
            if (!initialized)
                Initialize();
            var terrainIndex = terrains.IndexOf(terrain);
            if (terrainIndex == -1 || graphics[terrainIndex] == null)
                return null;
            return graphics[terrainIndex];
        }
    }
}
