using System.Collections.Generic;
using System.Linq;
using Verse;

namespace AlienBiomes
{
    public class MapComponent_MobilePlantCellsGetter : MapComponent
    {
        // Dictionary to store valid next cells for each Plant_Mobile instance
        private Dictionary<Plant_Mobile, List<IntVec3>> validNextCellsDict = new ();

        public MapComponent_MobilePlantCellsGetter(Map map) : base(map)
        {

        }

        public void RegisterPlant(Plant_Mobile plant)
        {
            // Register a Plant_Mobile instance and create an empty list for its valid next cells
            validNextCellsDict.Add(plant, []);
        }

        public void UnregisterPlant(Plant_Mobile plant)
        {
            // Unregister a Plant_Mobile instance and remove its entry from the dictionary
            validNextCellsDict.Remove(plant);
        }

        public List<IntVec3> GetValidNextCells(Plant_Mobile plant)
        {
            // Get the valid next cells for a specific Plant_Mobile instance
            return validNextCellsDict.TryGetValue(plant, out List<IntVec3> validNextCells) 
                ? validNextCells 
                : null;
        }
        
        public override void MapComponentTick()
        {
            base.MapComponentTick();

            // Iterate over all registered Plant_Mobile instances
            foreach (Plant_Mobile plant in validNextCellsDict.Keys.ToList())
            {
                // Get the list of valid next cells for the current Plant_Mobile instance
                List<IntVec3> validNextCells = validNextCellsDict[plant];
                // Clear the list to prepare for updating
                validNextCells.Clear();

                // Store the current position of the Plant_Mobile instance
                IntVec3 oldPos = plant.Position;

                // Iterate over all adjacent cells to the Plant_Mobile instance
                foreach (IntVec3 cell in GenAdj.CellsAdjacent8Way(plant))
                {
                    // Check if the cell meets the criteria for a valid next cell
                    if (cell.InBounds(map)
                        && cell.IsValid
                        && !map.Size.OnEdge(map)
                        && !cell.Filled(map)
                        && map.terrainGrid.TerrainAt(cell) == map.terrainGrid.TerrainAt(oldPos))
                        // CHECK TO SEE IF CELL IS OCCUPIED AS WELL
                    {
                        // Add the cell to the list of valid next cells
                        validNextCells.Add(cell);
                    }
                }
            }
        }
    }
}