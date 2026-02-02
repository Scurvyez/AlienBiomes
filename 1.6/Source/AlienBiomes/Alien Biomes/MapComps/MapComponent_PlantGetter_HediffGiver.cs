using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class MapComponent_PlantGetter_HediffGiver : MapComponent
    {
        public Dictionary<IntVec3, HashSet<Plant_Improved>> ActiveLocationTriggers = new ();
        
        private readonly HashSet<IntVec3> _globalEffectCellsSet = [];
        private List<IntVec3> _globalEffectCellsList = [];
        private List<Plant_Improved> _selectedPlants = [];
        private Color _highlightColor = Color.white;
        private int _lastSelectionSignature;
        
        public MapComponent_PlantGetter_HediffGiver(Map map) : base(map) { }

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
        
        private static int ComputeSelectionSignature()
        {
            int hash = 17;

            var selectedPlants = Find.Selector.SelectedObjectsListForReading;
            for (int i = 0; i < selectedPlants.Count; i++)
            {
                if (selectedPlants[i] is not Plant_Improved plant) continue;
                
                var comp = plant.TryGetComp<Comp_HediffGiver>();
                if (comp == null) continue;
                
                hash = (hash * 31) ^ plant.thingIDNumber;
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
                if (selected[i] is not Plant_Improved plant) continue;

                var comp = plant.TryGetComp<Comp_HediffGiver>();
                if (comp == null) continue;

                _selectedPlants.Add(plant);
            }
            
            if (_selectedPlants.Count == 0) return;

            for (int i = 0; i < _selectedPlants.Count; i++)
            {
                Plant plant = _selectedPlants[i];
                var comp = plant.TryGetComp<Comp_HediffGiver>();
                if (comp == null) continue;

                var props = (CompProperties_HediffGiver)comp.props;

                _highlightColor = props.triggerRadiusColor;

                if (props.hediffToGive != null && plant.Growth >= props.triggerGrowthThreshold)
                {
                    foreach (IntVec3 cell in GenRadial
                                 .RadialCellsAround(plant.Position, props.triggerRadius, true))
                    {
                        if (_globalEffectCellsSet.Add(cell))
                            _globalEffectCellsList.Add(cell);
                    }
                }
            }
        }
    }
}