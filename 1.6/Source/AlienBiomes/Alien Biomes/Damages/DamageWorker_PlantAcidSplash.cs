using Verse;

namespace AlienBiomes
{
    public class DamageWorker_PlantAcidSplash : DamageWorker_AddInjury
    {
        public override DamageResult Apply(DamageInfo dinfo, Thing thing)
        {
            return base.Apply(dinfo, thing);
        }
    }
}