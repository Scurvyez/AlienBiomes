using System.Collections.Generic;
using Verse;

namespace AlienBiomes
{
    public class PartWeightRule
    {
        public BodyPartDef part;
        public float multiplier = 1f;
    }
    
    public class ModExt_PlantAcidPrimary : DefModExtension
    {
        public float keepExistingHitPartChance = 0.35f;

        public float outsideDepthWeight = 4.0f;
        public float insideDepthMultiplier = 0.35f;
        public float nonCoreMultiplier = 1.2f;

        public float smallInsideCoveragePenalty = 0.25f;
        public float smallInsideCoverageCutoff = 0.03f;

        public List<PartWeightRule> partWeights;

        [Unsaved] private Dictionary<BodyPartDef, float> _partMultCache;

        public float GetMultiplierFor(BodyPartDef partDef)
        {
            if (partDef == null) return 1f;

            _partMultCache ??= BuildCache(partWeights);

            return _partMultCache.TryGetValue(partDef, out float m) ? m : 1f;
        }

        private static Dictionary<BodyPartDef, float> BuildCache(List<PartWeightRule> rules)
        {
            var dict = new Dictionary<BodyPartDef, float>();
            if (rules == null) return dict;

            for (int i = 0; i < rules.Count; i++)
            {
                PartWeightRule r = rules[i];
                if (r?.part == null) continue;
                dict[r.part] = r.multiplier;
            }

            return dict;
        }
    }
}