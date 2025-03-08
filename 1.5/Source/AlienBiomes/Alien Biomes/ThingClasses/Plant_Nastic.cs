using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class Plant_Nastic : Plant
    {
        private const int MaxTicks = 720;
        
        public bool GasExpelled;
        public int TouchSensitiveStartTime;
        
        private float _currentScale = 1f;
        private Color _fleckEmissionColor;
        private int _timeSinceLastStep;
        private int _gasCounter;
        private float _curPlantGrowth;
        private float _scaleY;
        private float _drawSizeY;
        private Plant_Nastic_ModExt _ext;
        private List<Vector3> _instanceOffsets = [];
        private Material _randMat;
        private Vector3 _drawPos = new (0, 0, 0);
        private Mesh _mesh = MeshPool.plane10;
        private Matrix4x4 _matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
        
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            _ext = def.GetModExtension<Plant_Nastic_ModExt>();
            
            if (_ext == null)
            {
                ABLog.Warning($"PlantNastic_ModExtension is null for {def.defName}");
                return;
            }
            
            if (_ext.isVisuallyReactive)
            {
                InitializeRandomOffsets();
                PrecomputeScaleDeltas();
                
                for (int i = 0; i < _instanceOffsets.Count; i++)
                {
                    if (def.graphicData == null) continue;
                    _randMat = Graphic.MatSingle;
                    _drawSizeY = def.graphicData.drawSize.y;
                }
            }
            
            IntVec3[] cells = GenRadial.RadialCellsAround(Position, _ext.effectRadius, useCenter: true).ToArray();
            MapComponent_PlantGetter plantGetter = map.GetComponent<MapComponent_PlantGetter>();
            
            foreach (IntVec3 cell in cells)
            {
                if (!plantGetter.ActiveLocationTriggers.ContainsKey(cell))
                {
                    plantGetter.ActiveLocationTriggers[cell] = [];
                }
                plantGetter.ActiveLocationTriggers[cell].Add(this);
            }
        }

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            base.DeSpawn(mode);
            MapComponent_PlantGetter plantGetter = Map?.GetComponent<MapComponent_PlantGetter>();
            
            if (plantGetter == null) return;
            foreach (HashSet<Plant_Nastic> set in plantGetter.ActiveLocationTriggers.Values)
            {
                set.Remove(this);
            }
        }
        
        public override void Tick()
        {
            if (_ext.isVisuallyReactive)
            {
                _curPlantGrowth = def.plant.visualSizeRange.LerpThroughRange(Growth);
                if (_ext != null)
                {
                    _timeSinceLastStep = Find.TickManager.TicksGame - TouchSensitiveStartTime;
                    if (_timeSinceLastStep < MaxTicks)
                    {
                        float scaleChangeRate = _ext.scaleDeltaCache[_timeSinceLastStep];
                        _currentScale = Mathf.Clamp(_currentScale + scaleChangeRate, _ext.minScale, 1);
                    }
                }
            }
            
            if (GasExpelled)
            {
                _gasCounter++;
                if (_gasCounter > _ext.explosionReleaseCooldown)
                {
                    GasExpelled = false;
                }
            }
            else
            {
                _gasCounter = 0;
            }
        }
        
        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            if (!_ext.isVisuallyReactive) return;
            for (int i = 0; i < _instanceOffsets.Count; i++)
            {
                // Draw the mesh with the modified UV coordinates
                _drawPos = _instanceOffsets[i];
                
                // Calculate the adjusted z-coordinate based on the change in scale
                _scaleY = Mathf.Lerp(-1f, 0.5f, _currentScale);
                // This ensures our individual textures on the mesh shrink down to their base and not into their center
                _drawPos.z += _drawSizeY * _scaleY / 10f;
                
                _matrix = Matrix4x4.TRS(_drawPos, Rotation.AsQuat, 
                    new Vector3(_currentScale * _curPlantGrowth, 1, _currentScale * _curPlantGrowth));
                Graphics.DrawMesh(_mesh, _matrix, _randMat, 0, null, 0, null, 
                    false, false, false);
            }
        }

        /// <summary>
        /// Calculates the initial position of each texture on our mesh on spawn.
        /// </summary>
        private void InitializeRandomOffsets()
        {
            for (int i = 0; i < _ext.texInstances; i++)
            {
                float xOffset = Rand.Range(-0.5f, 0.5f);
                float zOffset = Rand.Range(-0.5f, 0.5f);

                Vector3 instancePos = DrawPos;
                instancePos.x += xOffset;
                instancePos.z += zOffset;

                _instanceOffsets.Add(instancePos);
            }
        }

        /// <summary>
        /// Calculates one full scale cycle for the plant on spawn.
        /// </summary>
        private void PrecomputeScaleDeltas()
        {
            _ext.scaleDeltaCache = new float[MaxTicks];
            
            for (int i = 0; i < MaxTicks; i++)
            {
                float easedTime = i / 360f;
                float scaleChangeRateDecrease = Mathf.Lerp(-_ext.scaleDeltaDecrease, 0f, easedTime);
                float scaleDeltaIncreaseEased = ABEasingFunctions.EaseOutQuad(easedTime);
                float scaleChangeRateIncrease = Mathf.Lerp(0f, _ext.scaleDeltaIncrease, scaleDeltaIncreaseEased);
                float scaleChangeRate = scaleChangeRateDecrease + scaleChangeRateIncrease;
                
                _ext.scaleDeltaCache[i] = scaleChangeRate;
            }
        }
        
        public void DrawNasticFlecks()
        {
            if (Map == null 
                || _ext is not { emitFlecks: true } 
                || _ext.fleckDef == null) return;
            
            for (int i = 0; i < _ext.fleckBurstCount; ++i)
            {
                _fleckEmissionColor = Color.Lerp(_ext.colorA, _ext.colorB, Rand.Value);
                Vector3 drawPos = new (DrawPos.x + Rand.InsideUnitCircleVec3.x, 
                    DrawPos.y, DrawPos.z + Rand.InsideUnitCircleVec3.z);

                FleckCreationData fCD = FleckMaker.GetDataStatic(drawPos, Map, 
                    _ext.fleckDef, _ext.fleckScale.RandomInRange);
                fCD.rotationRate = Rand.RangeInclusive(-240, 240);
                fCD.instanceColor = _fleckEmissionColor;
                Map.flecks.CreateFleck(fCD);
            }
        }

        public void DoNasticExplosion()
        {
            GenExplosion.DoExplosion(Position, Map, _ext.explosionDamageEffectRadius, _ext.explosionDamageDef, null, 
                (_ext.explosionDamage.RandomInRange), -1, null, null, null, 
                null, null, 0, 0, null, 
                false, null, 0, 0, 0, 
                false, null, null, null, true, 1, 
                0, true, null, 0.02f);
        }
        
        public void TryGiveNasticHediff(Pawn pawn)
        {
            if (pawn.health.hediffSet.HasHediff(_ext.hediffToGive)) return;
            if (!Rand.Chance(_ext.hediffChance)) return;
            
            if (_ext.hediffToGive == ABDefOf.SZ_Crystallize
                || pawn.IsColonist || !pawn.IsColonyMech)
            {
                pawn.health.AddHediff(_ext.hediffToGive, null, null, null);
                HealthUtility.AdjustSeverity(pawn, _ext.hediffToGive, 0.01f);
                Find.LetterStack.ReceiveLetter("SZ_LetterLabelCrystallizing".Translate(), 
                    "SZ_LetterCrystallizing".Translate(pawn), ABDefOf.SZ_PawnCrystallizingLetter); 
            }
        }
        
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref GasExpelled, "GasExpelled");
            Scribe_Values.Look(ref _gasCounter, "GasCounter");
            Scribe_Values.Look(ref _currentScale, "CurrentScale", 1f);
            Scribe_Values.Look(ref _curPlantGrowth, "CurPlantGrowth");
            Scribe_Values.Look(ref TouchSensitiveStartTime, "TouchSensitiveStartTime", 0);
        }
    }
}