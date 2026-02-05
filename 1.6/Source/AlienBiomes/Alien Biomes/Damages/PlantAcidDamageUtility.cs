using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public static class PlantAcidDamageUtility
    {
        private static readonly ModExt_PlantAcidPrimary DefaultPrimaryExt = new();
        private static readonly ModExt_PlantAcidSplash DefaultSplashExt = new();
        
        private const float MinWeight = 0.0001f;
        private const float MinCoverage = 0.001f;
        
        public static ModExt_PlantAcidPrimary PrimaryExt(DamageDef def)
            => def?.GetModExtension<ModExt_PlantAcidPrimary>() ?? DefaultPrimaryExt;
        
        public static ModExt_PlantAcidSplash SplashExt(DamageDef def)
            => def?.GetModExtension<ModExt_PlantAcidSplash>() ?? DefaultSplashExt;
        
        public static bool IsValidPawnTarget(Thing thing, out Pawn pawn)
        {
            pawn = thing as Pawn;
            return pawn is { Dead: false, health.hediffSet: not null };
        }
        
        public static BodyPartRecord ChoosePrimaryPart(Pawn pawn, DamageInfo dinfo, ModExt_PlantAcidPrimary ext)
        {
            if (dinfo.HitPart != null && Rand.Chance(ext.keepExistingHitPartChance))
                return dinfo.HitPart;
            
            BodyPartRecord chosen = null;
            float best = 0f;
            
            foreach (BodyPartRecord part in pawn.health.hediffSet
                         .GetNotMissingParts(depth: BodyPartDepth.Undefined))
            {
                float w = PrimaryPartWeight(part, ext);
                if (w <= 0f) continue;
                
                best += w;
                if (Rand.Value * best < w)
                    chosen = part;
            }
            
            return chosen;
        }
        
        public static void TryApplySplashHits(Pawn pawn, DamageInfo source, BodyPartRecord primaryPart,
            ModExt_PlantAcidPrimary primaryExt, ModExt_PlantAcidSplash splashExt, DamageDef splashDamageDef)
        {
            if (pawn.Dead) return;
            if (!Rand.Chance(splashExt.splashChance)) return;
            
            // avoid garbage...
            var parts = SimplePool<List<BodyPartRecord>>.Get();
            parts.Clear();
            
            try
            {
                foreach (BodyPartRecord part in pawn.health.hediffSet
                             .GetNotMissingParts(depth: BodyPartDepth.Outside))
                {
                    if (part != null) parts.Add(part);
                }
                
                if (parts.Count == 0) return;
                
                if (primaryPart != null)
                    parts.Remove(primaryPart);
                
                if (parts.Count == 0) return;
                
                int hits = Rand.RangeInclusive(splashExt.splashHits.min, splashExt.splashHits.max);
                
                for (int i = 0; i < hits && parts.Count > 0 && !pawn.Dead; i++)
                {
                    BodyPartRecord chosen = ChooseSplashPart(parts, primaryExt, splashExt);
                    if (chosen == null) break;
                    
                    parts.Remove(chosen);
                    
                    int dmg = ComputeSplashDamage(source.Amount, splashExt.splashDamageFrac);
                    
                    var dinfo = new DamageInfo(splashDamageDef, dmg, source.ArmorPenetrationInt, source.Angle,
                        source.Instigator, chosen, source.Weapon, source.Category, source.IntendedTarget,
                        source.InstigatorGuilty);
                    
                    dinfo.SetAllowDamagePropagation(source.AllowDamagePropagation);
                    pawn.TakeDamage(dinfo);
                }
            }
            finally
            {
                parts.Clear();
                SimplePool<List<BodyPartRecord>>.Return(parts);
            }
        }

        private static int ComputeSplashDamage(float sourceAmount, FloatRange fracRange)
        {
            float frac = fracRange.RandomInRange;
            int dmg = Mathf.RoundToInt(sourceAmount * frac);
            return Mathf.Max(1, dmg);
        }

        private static BodyPartRecord ChooseSplashPart(List<BodyPartRecord> parts, ModExt_PlantAcidPrimary primaryExt,
            ModExt_PlantAcidSplash splashExt)
        {
            BodyPartRecord chosen = null;
            float total = 0f;
            
            for (int i = 0; i < parts.Count; i++)
            {
                BodyPartRecord part = parts[i];
                float w = SplashPartWeight(part, primaryExt, splashExt);
                if (w <= 0f) continue;
                
                total += w;
                if (Rand.Value * total < w)
                    chosen = part;
            }
            
            return chosen;
        }

        private static float PrimaryPartWeight(BodyPartRecord part, ModExt_PlantAcidPrimary ext)
        {
            if (part == null) return 0f;
            
            float coverage = part.coverageAbs;
            if (coverage <= 0f) coverage = MinCoverage;
            
            float w = coverage;
            
            switch (part.depth)
            {
                case BodyPartDepth.Outside:
                    w *= ext.outsideDepthWeight;
                    break;
                case BodyPartDepth.Inside:
                    w *= ext.insideDepthMultiplier;
                    break;
            }
            
            if (!part.IsCorePart)
                w *= ext.nonCoreMultiplier;
            
            w *= ext.GetMultiplierFor(part.def);
            
            if (part.depth != BodyPartDepth.Outside && coverage < ext.smallInsideCoverageCutoff)
                w *= ext.smallInsideCoveragePenalty;
            
            return w < MinWeight ? MinWeight : w;
        }

        private static float SplashPartWeight(BodyPartRecord part, ModExt_PlantAcidPrimary primaryExt,
            ModExt_PlantAcidSplash splashExt)
        {
            float w = PrimaryPartWeight(part, primaryExt);
            
            float coverage = part?.coverageAbs ?? 0f;
            if (coverage <= 0f) coverage = MinCoverage;
            
            float t = Mathf.Clamp01(coverage * splashExt.coverageScale);
            float curve = Mathf.Lerp(splashExt.splashWeightAtLowCoverage, splashExt.splashWeightAtHighCoverage, t);
            
            w *= curve;
            return w < MinWeight ? MinWeight : w;
        }
    }
}