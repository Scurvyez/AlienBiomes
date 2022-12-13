using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;

namespace AlienBiomes
{
    public class HediffComp_Crystallize : HediffComp
    {
        public HediffCompProperties_Crystallize Props => (HediffCompProperties_Crystallize)props;
        public bool CrystalDeath;

        /// <summary>
        /// Generates a new ThingDef where a pawn died if the pawn died with a specific hediff.
        /// </summary>
        public override void Notify_PawnDied()
        {
            Map map = parent.pawn.Corpse.MapHeld;
            IntVec3 pos = map.AllCells.RandomElement();

            if (map != null)
            {
                CrystalDeath = true;
                if (CrystalDeath)
                {
                    if (map.Biome == AlienBiomes_BiomeDefOf.SZ_CrystallineFlats)
                    {
                        for (int i = 0; i < map.cellIndices.NumGridCells; i++)
                        {
                            var terrain = map.terrainGrid.TerrainAt(i);
                            if (terrain == TerrainDefOf.WaterMovingShallow)
                                map.terrainGrid.SetTerrain(map.cellIndices.IndexToCell(i), 
                                    AlienBiomes_TerrainDefOf.SZ_BloodWaterMovingShallow);
                            else if (terrain == TerrainDefOf.WaterMovingChestDeep)
                                map.terrainGrid.SetTerrain(map.cellIndices.IndexToCell(i), 
                                    AlienBiomes_TerrainDefOf.SZ_BloodWaterMovingChestDeep);
                        }
                    }
                    Find.LetterStack.ReceiveLetter("SZ_LetterLabelCrystallized".Translate(), 
                        "SZ_LetterCrystallized".Translate(parent.pawn), 
                        AlienBiomes_LetterDefOf.SZ_PawnCrystallized, null, null, null);
                    Find.TickManager.slower.SignalForceNormalSpeedShort();

                    GenSpawn.Spawn(ThingDef.Named(Props.targetCrystal), pos, map, WipeMode.Vanish);
                    FilthMaker.TryMakeFilth(parent.pawn.Position, parent.pawn.Corpse.Map, ThingDefOf.Filth_Blood);
                    parent.pawn.Corpse.Destroy();
                }
            }
        }
    }
}
