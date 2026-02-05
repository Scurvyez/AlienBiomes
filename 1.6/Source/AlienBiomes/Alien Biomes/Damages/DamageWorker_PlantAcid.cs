using Verse;

namespace AlienBiomes
{
    public class DamageWorker_PlantAcid : DamageWorker_AddInjury
    {
        public override DamageResult Apply(DamageInfo dinfo, Thing thing)
        {
            if (!PlantAcidDamageUtility.IsValidPawnTarget(thing, out Pawn pawn))
                return base.Apply(dinfo, thing);

            var primaryExt = PlantAcidDamageUtility.PrimaryExt(dinfo.Def);
            var splashExt  = PlantAcidDamageUtility.SplashExt(dinfo.Def);

            BodyPartRecord primaryPart = PlantAcidDamageUtility.ChoosePrimaryPart(pawn, dinfo, primaryExt);
            if (primaryPart != null)
                dinfo.SetHitPart(primaryPart);

            DamageResult result = base.Apply(dinfo, thing);

            if (!pawn.Dead)
            {
                PlantAcidDamageUtility.TryApplySplashHits(
                    pawn, dinfo, primaryPart, primaryExt, splashExt, InternalDefOf.SZ_PlantAcidSplash);
            }

            return result;
        }
    }
}