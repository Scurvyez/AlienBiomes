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
        /// Sets a range of ints based on Props.
        /// </summary>
        public override void CompPostMake()
        {
            base.CompPostMake();
        }
        
        /// <summary>
        /// Checks for a hediff, if found the pawn is killed and a letter is generated.
        /// </summary>
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            var instance = parent.pawn.health.hediffSet.GetFirstHediffOfDef(AlienBiomes_HediffDefOf.SZ_Crystallize, true);
            if (instance != null && instance.Severity >= instance.def.lethalSeverity)
            {
                CrystalDeath = true;
                parent.pawn.Kill(null);
                Find.LetterStack.ReceiveLetter("SZ_LetterLabelCrystallized".Translate(), "SZ_LetterCrystallized".Translate(parent.pawn), AlienBiomes_LetterDefOf.SZ_PawnCrystallized, null, null, null);
                Find.TickManager.slower.SignalForceNormalSpeedShort();
            }
        }

        /// <summary>
        /// Generates a new ThingDef where a pawn died.
        /// </summary>
        public override void Notify_PawnDied()
        {
            Map map = parent.pawn.Corpse.MapHeld;
            IntVec3 pos = parent.pawn.Position;

            if (map != null)
            {
                if (CrystalDeath)
                {
                    if (map.Biome == AlienBiomes_BiomeDefOf.SZ_CrystallineFlats)
                    {
                        for (int i = 0; i < map.cellIndices.NumGridCells; i++)
                        {
                            var terrain = map.terrainGrid.TerrainAt(i);
                            if (terrain == TerrainDefOf.WaterMovingShallow)
                                map.terrainGrid.SetTerrain(map.cellIndices.IndexToCell(i), AlienBiomes_TerrainDefOf.SZ_BloodWaterMovingShallow);
                            else if (terrain == TerrainDefOf.WaterMovingChestDeep)
                                map.terrainGrid.SetTerrain(map.cellIndices.IndexToCell(i), AlienBiomes_TerrainDefOf.SZ_BloodWaterMovingChestDeep);
                        }
                    }

                    GenSpawn.Spawn(ThingDef.Named(Props.targetCrystal), pos, map, WipeMode.Vanish);
                    FilthMaker.TryMakeFilth(parent.pawn.Position, parent.pawn.Corpse.Map, ThingDefOf.Filth_Blood);
                    parent.pawn.Corpse.Destroy();
                }
            }
        }
    }
}
