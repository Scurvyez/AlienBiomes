using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class Plant_VisuallyReactive : Plant
    {
        private const int MaxTicks = 720;
        
        public int TouchSensitiveStartTime;
        public float CurrentScale = 1f;

        private float[] _scaleDeltaCache;
        private Map _cachedMap;
        private int _timeSinceLastStep;
        private float _curPlantGrowth;
        private float _scaleY;
        private float _drawSizeY;
        private ModExt_PlantVisuallyReactive _ext;
        private Material _randMat;
        private Vector3 _drawPos = new (0, 0, 0);
        private Matrix4x4 _matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
        private readonly List<Vector3> _instanceOffsets = [];
        private readonly Mesh _mesh = MeshPool.plane10;
        
        public ModExt_PlantVisuallyReactive Ext => _ext;
        
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            _ext = def.GetModExtension<ModExt_PlantVisuallyReactive>();
            _cachedMap = map;
            
            if (_ext == null)
            {
                ABLog.Warning($"{_ext} is null for {def.defName}");
                return;
            }
            
            InitializeRandomOffsets();
            _scaleDeltaCache = NasticScaleCache.Get(def, _ext, maxTicks: MaxTicks);
                
            for (int i = 0; i < _instanceOffsets.Count; i++)
            {
                if (def.graphicData == null) continue;
                _randMat = Graphic.MatSingle;
                _drawSizeY = def.graphicData.drawSize.y;
            }
            
            IntVec3[] cells = GenRadial.RadialCellsAround(Position, _ext.triggerRadius, useCenter: true).ToArray();
            var plantGetterTouchSensitive = map.GetComponent<MapComponent_PlantGetter_VisuallyReactive>();
            
            foreach (IntVec3 cell in cells)
            {
                if (!plantGetterTouchSensitive.ActiveLocationTriggers.ContainsKey(cell))
                {
                    plantGetterTouchSensitive.ActiveLocationTriggers[cell] = [];
                }
                plantGetterTouchSensitive.ActiveLocationTriggers[cell].Add(this);
            }
        }
        
        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            var plantGetterTouchSensitive = _cachedMap?.GetComponent<MapComponent_PlantGetter_VisuallyReactive>();
            plantGetterTouchSensitive?.ActiveLocationTriggers
                .Where(kvp => kvp.Value.Remove(this) && 
                              kvp.Value.Count == 0)
                .Select(kvp => kvp.Key)
                .ToList()
                .ForEach(key => plantGetterTouchSensitive.ActiveLocationTriggers.Remove(key));
            
            base.DeSpawn(mode);
            _cachedMap = null;
        }
        
        // using TickInterval may be why we are seeing some weirdness with the visuals
        // it runs in parallel with the camera logic
        protected override void Tick()
        {
            base.Tick();
            if (_scaleDeltaCache != null)
            {
                _curPlantGrowth = def.plant.visualSizeRange.LerpThroughRange(Growth);
                if (_ext != null)
                {
                    _timeSinceLastStep = Find.TickManager.TicksGame - TouchSensitiveStartTime;
                    if (_timeSinceLastStep < MaxTicks)
                    {
                        float scaleChangeRate = _scaleDeltaCache[_timeSinceLastStep];
                        CurrentScale = Mathf.Clamp(CurrentScale + scaleChangeRate, _ext.minDrawScale, 1);
                    }
                }
            }
        }
        
        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            for (int i = 0; i < _instanceOffsets.Count; i++)
            {
                // Draw the mesh with the modified UV coordinates
                _drawPos = _instanceOffsets[i];
                
                // Calculate the adjusted z-coordinate based on the change in scale
                _scaleY = Mathf.Lerp(-1f, 0.5f, CurrentScale);
                // This ensures our individual textures on the mesh shrink down to their base and not into their center
                _drawPos.z += _drawSizeY * _scaleY / 10f;
                
                _matrix = Matrix4x4.TRS(_drawPos, Rotation.AsQuat, 
                    new Vector3(CurrentScale * _curPlantGrowth, 1, CurrentScale * _curPlantGrowth));
                Graphics.DrawMesh(_mesh, _matrix, _randMat, 0, null, 0, null, 
                    false, false, false);
            }
        }

        /// <summary>
        /// Calculates the initial position of each texture on our mesh on spawn.
        /// </summary>
        private void InitializeRandomOffsets()
        {
            for (int i = 0; i < _ext.textureInstancesPerMesh; i++)
            {
                float xOffset = Rand.Range(-0.5f, 0.5f);
                float zOffset = Rand.Range(-0.5f, 0.5f);

                Vector3 instancePos = DrawPos;
                instancePos.x += xOffset;
                instancePos.z += zOffset;

                _instanceOffsets.Add(instancePos);
            }
        }
        
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref CurrentScale, "CurrentScale");
            Scribe_Values.Look(ref _curPlantGrowth, "_curPlantGrowth");
            Scribe_Values.Look(ref TouchSensitiveStartTime, "TouchSensitiveStartTime");
        }
    }
}