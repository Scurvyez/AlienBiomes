using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class Plant_VisuallyReactive : Plant
    {
        private const int MaxTicks = 720;
        private const int ScaleUpdateIntervalTicks = 2;

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
        private Vector3 _drawPos = new(0, 0, 0);
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
                ABLog.Warning($"ModExt_PlantVisuallyReactive is null for {def.defName}");
                return;
            }

            if (def.graphicData == null)
                return;

            _randMat = Graphic.MatSingle;
            _drawSizeY = def.graphicData.drawSize.y;

            _instanceOffsets.Clear();
            InitializeRandomOffsets();

            _scaleDeltaCache = NasticScaleCache.Get(def, _ext, maxTicks: MaxTicks);

            if (!respawningAfterLoad)
            {
                CurrentScale = 1f;
                TouchSensitiveStartTime = Find.TickManager.TicksGame - MaxTicks;
            }
            
            _curPlantGrowth = def.plant.visualSizeRange.LerpThroughRange(Growth);
            
            var plantGetterTouchSensitive = map.GetComponent<MapComponent_PlantGetter_VisuallyReactive>();
            if (plantGetterTouchSensitive != null)
            {
                foreach (IntVec3 cell in GenRadial.RadialCellsAround(Position, _ext.triggerRadius, useCenter: true))
                {
                    if (!plantGetterTouchSensitive.ActiveLocationTriggers.TryGetValue(cell, out var set))
                    {
                        set = new HashSet<Plant_VisuallyReactive>();
                        plantGetterTouchSensitive.ActiveLocationTriggers[cell] = set;
                    }
                    set.Add(this);
                }
            }
        }

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            // no full-dictionary scan)
            if (_ext != null)
            {
                var plantGetterTouchSensitive = _cachedMap?.GetComponent<MapComponent_PlantGetter_VisuallyReactive>();
                if (plantGetterTouchSensitive != null)
                {
                    foreach (IntVec3 cell in GenRadial.RadialCellsAround(Position, _ext.triggerRadius, useCenter: true))
                    {
                        if (!plantGetterTouchSensitive.ActiveLocationTriggers.TryGetValue(cell, out var set))
                            continue;

                        set.Remove(this);
                        if (set.Count == 0)
                            plantGetterTouchSensitive.ActiveLocationTriggers.Remove(cell);
                    }
                }
            }

            base.DeSpawn(mode);
            _cachedMap = null;
        }

        protected override void Tick()
        {
            base.Tick();
            
            if (_ext == null || _scaleDeltaCache == null)
                return;
            
            _curPlantGrowth = def.plant.visualSizeRange.LerpThroughRange(Growth);
            
            int ticksGame = Find.TickManager.TicksGame;
            _timeSinceLastStep = ticksGame - TouchSensitiveStartTime;
            if ((uint)_timeSinceLastStep >= MaxTicks)
                return;
            
            if ((ticksGame & (ScaleUpdateIntervalTicks - 1)) != 0)
                return;
            
            float scaleChangeRate = _scaleDeltaCache[_timeSinceLastStep];
            if (ScaleUpdateIntervalTicks == 2 && _timeSinceLastStep > 0)
                scaleChangeRate += _scaleDeltaCache[_timeSinceLastStep - 1];
            
            CurrentScale = Mathf.Clamp(CurrentScale + scaleChangeRate, _ext.minDrawScale, 1f);
        }

        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            if (_randMat == null || _instanceOffsets.Count == 0)
                return;

            _scaleY = Mathf.Lerp(-1f, 0.5f, CurrentScale);
            float zAdjust = _drawSizeY * _scaleY / 10f;

            float scaled = CurrentScale * _curPlantGrowth;
            var scaleVec = new Vector3(scaled, 1f, scaled);
            var rot = Rotation.AsQuat;

            for (int i = 0; i < _instanceOffsets.Count; i++)
            {
                _drawPos = drawLoc + _instanceOffsets[i];
                _drawPos.z += zAdjust;

                _matrix = Matrix4x4.TRS(_drawPos, rot, scaleVec);
                Graphics.DrawMesh(_mesh, _matrix, _randMat, 0, null, 
                    0, null, false, false, false);
            }
        }

        private void InitializeRandomOffsets()
        {
            for (int i = 0; i < _ext.textureInstancesPerMesh; i++)
            {
                float xOffset = Rand.Range(-0.5f, 0.5f);
                float zOffset = Rand.Range(-0.5f, 0.5f);

                _instanceOffsets.Add(new Vector3(xOffset, 0f, zOffset));
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