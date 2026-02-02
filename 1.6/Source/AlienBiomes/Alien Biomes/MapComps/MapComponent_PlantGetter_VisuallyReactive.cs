using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class MapComponent_PlantGetter_VisuallyReactive : MapComponent
    {
        public Dictionary<IntVec3, HashSet<Plant_VisuallyReactive>> ActiveLocationTriggers = new ();
        
        private readonly HashSet<IntVec3> _globalEffectCellsSet = [];
        private List<IntVec3> _globalEffectCellsList = [];
        private List<Plant_VisuallyReactive> _selectedPlants = [];
        private Color _highlightColor = Color.white;
        private int _lastSelectionSignature;
        
        public MapComponent_PlantGetter_VisuallyReactive(Map map) : base(map) { }

        public override void MapComponentUpdate()
        {
            base.MapComponentUpdate();
            
            if (Find.CurrentMap == null) return;
            
            int signature = ComputeSelectionSignature();
            if (signature != _lastSelectionSignature)
            {
                _lastSelectionSignature = signature;
                RebuildSelectionHighlightCache();
            }

            if (_globalEffectCellsList.Count > 0)
                GenDraw.DrawFieldEdges(_globalEffectCellsList, _highlightColor);
        }
        
        private int ComputeSelectionSignature()
        {
            int hash = 17;

            var selectedPlants = Find.Selector.SelectedObjectsListForReading;
            for (int i = 0; i < selectedPlants.Count; i++)
            {
                if (selectedPlants[i] is Plant_VisuallyReactive plant)
                {
                    hash = (hash * 31) ^ plant.thingIDNumber;
                }
            }
            return hash;
        }

        private void RebuildSelectionHighlightCache()
        {
            _selectedPlants.Clear();
            _globalEffectCellsSet.Clear();
            _globalEffectCellsList.Clear();

            var selected = Find.Selector.SelectedObjectsListForReading;
            for (int i = 0; i < selected.Count; i++)
            {
                if (selected[i] is Plant_VisuallyReactive plant)
                    _selectedPlants.Add(plant);
            }
            
            if (_selectedPlants.Count == 0) return;

            for (int i = 0; i < _selectedPlants.Count; i++)
            {
                Plant_VisuallyReactive plant = _selectedPlants[i];
                ModExt_PlantVisuallyReactive ext = plant.Ext;
                
                if (ext == null) continue;
                
                _highlightColor = ext.triggerRadiusColor;

                foreach (IntVec3 cell in GenRadial
                             .RadialCellsAround(plant.Position, ext.triggerRadius, true))
                {
                    if (_globalEffectCellsSet.Add(cell))
                        _globalEffectCellsList.Add(cell);
                }
            }
        }
    }
}