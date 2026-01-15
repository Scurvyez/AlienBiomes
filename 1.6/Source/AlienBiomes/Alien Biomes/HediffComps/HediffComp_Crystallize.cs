using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class HediffComp_Crystallize : HediffComp
    {
        public HediffCompProperties_Crystallize Props => (HediffCompProperties_Crystallize)props;
        
        private bool _crystalDeath = false;
        private IntVec3 _pawnPos = IntVec3.Invalid;
        
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            if (Pawn is { IsColonist: false } 
                && (Pawn.MapHeld == null || Pawn.Destroyed || Pawn.Discarded))
            {
                Pawn.health.RemoveHediff(parent);
            }
        }
        
        /// <summary>
        /// Generates a new ThingDef where a pawn died if the pawn died with a specific hediff.
        /// </summary>
        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            base.Notify_PawnDied(dinfo, culprit);
            
            Map map = Pawn.Corpse.MapHeld;
            _pawnPos = Pawn.Position;
            
            if (map == null) return;
            if (Mathf.Approximately(Pawn.health.hediffSet
                    .GetFirstHediffOfDef(InternalDefOf.SZ_Crystallize).Severity, 1.0f))
            {
                _crystalDeath = true;
            }
            
            if (!_crystalDeath) return;
            if (map.Biome == InternalDefOf.SZ_CrystallineFlats)
            {
                for (int i = 0; i < map.cellIndices.NumGridCells; i++)
                {
                    TerrainDef terrain = map.terrainGrid.TerrainAt(i);
                    if (terrain == TerrainDefOf.WaterMovingShallow)
                        map.terrainGrid.SetTerrain(map.cellIndices.IndexToCell(i),
                            InternalDefOf.SZ_BloodWaterMovingShallow);
                    else if (terrain == TerrainDefOf.WaterMovingChestDeep)
                        map.terrainGrid.SetTerrain(map.cellIndices.IndexToCell(i),
                            InternalDefOf.SZ_BloodWaterMovingChestDeep);
                }
            }
            
            if (Pawn.IsColonist)
            {
                Find.LetterStack.ReceiveLetter("SZAB_LetterLabelCrystallized".Translate(),
                    "SZAB_LetterCrystallized".Translate(Pawn),
                    InternalDefOf.SZ_PawnCrystallizedLetter, null, null);
                Find.TickManager.slower.SignalForceNormalSpeedShort();
            }
            
            GenSpawn.Spawn(Props.targetCrystal, TryFindRandomValidCell(map), map);
            FilthMaker.TryMakeFilth(GenRadial
                    .RadialCellsAround(_pawnPos, 1f, true)
                    .RandomElement(), 
                Pawn.Corpse.Map, ThingDefOf.Filth_Blood);
            
            Pawn.Corpse.Destroy();
        }
        
        private static IntVec3 TryFindRandomValidCell(Map map)
        {
            List<IntVec3> potentialSpawnCells = [];
            
            foreach (IntVec3 cell in  map.AllCells)
            {
                TerrainDef terrain = map.terrainGrid.TerrainAt(cell);
                if (terrain == InternalDefOf.SZ_CrystallineSoil)
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