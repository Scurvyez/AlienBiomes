﻿using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class HediffComp_Crystallize : HediffComp
    {
        public HediffCompProperties_Crystallize Props => (HediffCompProperties_Crystallize)props;
        
        private bool _crystalDeath;
        private IntVec3 _pawnPos;
        
        /// <summary>
        /// Generates a new ThingDef where a pawn died if the pawn died with a specific hediff.
        /// </summary>
        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            base.Notify_PawnDied(dinfo, culprit);
            
            Map map = parent.pawn.Corpse.MapHeld;
            _pawnPos = parent.pawn.Position;
            
            if (map == null) return;
            if (Mathf.Approximately(parent.pawn.health.hediffSet
                    .GetFirstHediffOfDef(ABDefOf.SZ_Crystallize).Severity, 1.0f))
            {
                _crystalDeath = true;
            }
            
            if (!_crystalDeath) return;
            if (map.Biome == ABDefOf.SZ_CrystallineFlats)
            {
                for (int i = 0; i < map.cellIndices.NumGridCells; i++)
                {
                    TerrainDef terrain = map.terrainGrid.TerrainAt(i);
                    if (terrain == TerrainDefOf.WaterMovingShallow)
                        map.terrainGrid.SetTerrain(map.cellIndices.IndexToCell(i),
                            ABDefOf.SZ_BloodWaterMovingShallow);
                    else if (terrain == TerrainDefOf.WaterMovingChestDeep)
                        map.terrainGrid.SetTerrain(map.cellIndices.IndexToCell(i),
                            ABDefOf.SZ_BloodWaterMovingChestDeep);
                }
            }
            
            if (parent.pawn.IsColonist)
            {
                Find.LetterStack.ReceiveLetter("SZAB_LetterLabelCrystallized".Translate(),
                    "SZAB_LetterCrystallized".Translate(parent.pawn),
                    ABDefOf.SZ_PawnCrystallizedLetter, null, null);
                Find.TickManager.slower.SignalForceNormalSpeedShort();
            }
            
            GenSpawn.Spawn(ThingDef.Named(Props.targetCrystal),
                TryFindRandomValidCell(map), map);
            
            FilthMaker.TryMakeFilth(GenRadial
                    .RadialCellsAround(_pawnPos, 1f, true).RandomElement(), 
                parent.pawn.Corpse.Map, ThingDefOf.Filth_Blood);
            
            parent.pawn.Corpse.Destroy();
        }
        
        private static IntVec3 TryFindRandomValidCell(Map map)
        {
            List<IntVec3> potentialSpawnCells = [];
            
            foreach (IntVec3 cell in  map.AllCells)
            {
                TerrainDef terrain = map.terrainGrid.TerrainAt(cell);
                if (terrain == ABDefOf.SZ_CrystallineSoil)
                {
                    potentialSpawnCells.Add(cell);
                }
            }
            return potentialSpawnCells.Count > 0 
                ? potentialSpawnCells.RandomElement() 
                : IntVec3.Invalid;
        }
    }
}