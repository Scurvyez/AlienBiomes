using RimWorld;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    [UsedImplicitly]
    public class MapComponent_PlantGetter : MapComponent
    {
        public Dictionary<IntVec3, HashSet<Plant_Nastic>> ActiveLocationTriggers = new ();
        public float SunStrength;
        
        private readonly HashSet<IntVec3> _globalEffectCells = [];
        private readonly HashSet<IntVec3> _globalExplosionCells = [];
        private List<Plant_Nastic> _selectedPlants = [];
        private Color _hediffColor = Color.white;
        private Color _explosionColor = Color.white;
        
        public MapComponent_PlantGetter(Map map) : base(map) { }
        
        public override void MapComponentTick()
        {
            base.MapComponentTick();
            SunStrength = GenCelestial.CurCelestialSunGlow(map);
        }
        
        public override void MapComponentUpdate()
        {
            base.MapComponentUpdate();
            
            if (Find.CurrentMap == null) return;
            _selectedPlants = Find.Selector.SelectedObjectsListForReading
                .OfType<Plant_Nastic>()
                .ToList();
            
            if (_selectedPlants.Count == 0) return;
            _globalEffectCells.Clear();
            _globalExplosionCells.Clear();
            
            foreach (Plant_Nastic plant in _selectedPlants)
            {
                Comp_NasticInfo comp = plant.TryGetComp<Comp_NasticInfo>();
                if (comp?._ext == null)
                    continue;
                
                _hediffColor = comp._ext.hediffEffectRadiusColor;
                _explosionColor = comp._ext.explosionEffectRadiusColor;
                
                if (comp._ext.hediffToGive != null && 
                    plant.Growth >= comp._ext.givesHediffGrowthThreshold)
                {
                    foreach (IntVec3 cell in GenRadial
                                 .RadialCellsAround(plant.Position, 
                                     comp._ext.effectRadius, true))
                        _globalEffectCells.Add(cell);
                }
                
                if (comp._ext.explosionDamageDef != null && 
                    plant.Growth >= comp._ext.explosionGrowthThreshold)
                {
                    foreach (IntVec3 cell in GenRadial
                                 .RadialCellsAround(plant.Position, 
                                     comp._ext.effectRadius, true))
                        _globalExplosionCells.Add(cell);
                }
            }
            
            if (_globalEffectCells.Count > 0)
                GenDraw.DrawFieldEdges(_globalEffectCells.ToList(), _hediffColor);
            
            if (_globalExplosionCells.Count > 0)
                GenDraw.DrawFieldEdges(_globalExplosionCells.ToList(), _explosionColor);
        }
    }
}