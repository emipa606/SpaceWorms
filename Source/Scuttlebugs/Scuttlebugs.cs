using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace Scuttlebugs
{

    //Scuttlebug HediffDefOf - need this to be able to check if the pawn has the hediff
    [DefOf]
    public static class Scuttlebugs_HediffDefOf
    {
        public static HediffDef ScuttlebugInfection;
        public static HediffDef ScuttlebugQueenInfection;
    }

    public class Verb_ScuttlebugDamage : Verb_MeleeAttackDamage
    {
        private const float MeleeDamageRandomFactorMin = 0.8f;

        private const float MeleeDamageRandomFactorMax = 1.2f;

        private IEnumerable<DamageInfo> DamageInfosToApply(LocalTargetInfo target)
        {
            float damAmount = this.verbProps.AdjustedMeleeDamageAmount(this, base.CasterPawn);
            float armorPenetration = this.verbProps.AdjustedArmorPenetration(this, base.CasterPawn);
            DamageDef damDef = this.verbProps.meleeDamageDef;
            BodyPartGroupDef bodyPartGroupDef = null;
            HediffDef hediffDef = null;
            damAmount = Rand.Range(damAmount * 0.8f, damAmount * 1.2f);
            if (base.CasterIsPawn)
            {
                bodyPartGroupDef = this.verbProps.AdjustedLinkedBodyPartsGroup(this.tool);
                if (damAmount >= 1f)
                {
                    if (base.HediffCompSource != null)
                    {
                        hediffDef = base.HediffCompSource.Def;
                    }
                }
                else
                {
                    damAmount = 1f;
                    damDef = DamageDefOf.Blunt;
                }
            }
            ThingDef source;
            if (base.EquipmentSource != null)
            {
                source = base.EquipmentSource.def;
            }
            else
            {
                source = base.CasterPawn.def;
            }
            Vector3 direction = (target.Thing.Position - base.CasterPawn.Position).ToVector3();
            DamageDef def = damDef;
            float num = damAmount;
            float num2 = armorPenetration;
            Thing caster = this.caster;
            DamageInfo mainDinfo = new DamageInfo(def, num, num2, -1f, caster, null, source, DamageInfo.SourceCategory.ThingOrUnknown, null);
            mainDinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
            mainDinfo.SetWeaponBodyPartGroup(bodyPartGroupDef);
            mainDinfo.SetWeaponHediff(hediffDef);
            mainDinfo.SetAngle(direction);
            yield return mainDinfo;
            if (this.surpriseAttack && ((this.verbProps.surpriseAttack != null && !this.verbProps.surpriseAttack.extraMeleeDamages.NullOrEmpty<ExtraDamage>()) || (this.tool != null && this.tool.surpriseAttack != null && !this.tool.surpriseAttack.extraMeleeDamages.NullOrEmpty<ExtraDamage>())))
            {
                IEnumerable<ExtraDamage> extraDamages = Enumerable.Empty<ExtraDamage>();
                if (this.verbProps.surpriseAttack != null && this.verbProps.surpriseAttack.extraMeleeDamages != null)
                {
                    extraDamages = extraDamages.Concat(this.verbProps.surpriseAttack.extraMeleeDamages);
                }
                if (this.tool != null && this.tool.surpriseAttack != null && !this.tool.surpriseAttack.extraMeleeDamages.NullOrEmpty<ExtraDamage>())
                {
                    extraDamages = extraDamages.Concat(this.tool.surpriseAttack.extraMeleeDamages);
                }
                foreach (ExtraDamage extraDamage in extraDamages)
                {
                    int extraDamageAmount = GenMath.RoundRandom(extraDamage.AdjustedDamageAmount(this, base.CasterPawn));
                    float extraDamageArmorPenetration = extraDamage.AdjustedArmorPenetration(this, base.CasterPawn);
                    def = extraDamage.def;
                    num2 = (float)extraDamageAmount;
                    num = extraDamageArmorPenetration;
                    caster = this.caster;
                    DamageInfo extraDinfo = new DamageInfo(def, num2, num, -1f, caster, null, source, DamageInfo.SourceCategory.ThingOrUnknown, null);
                    extraDinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
                    extraDinfo.SetWeaponBodyPartGroup(bodyPartGroupDef);
                    extraDinfo.SetWeaponHediff(hediffDef);
                    extraDinfo.SetAngle(direction);
                    yield return extraDinfo;
                }
            }
        }

        protected override DamageWorker.DamageResult ApplyMeleeDamageToTarget(LocalTargetInfo target)
        {
            DamageWorker.DamageResult result = new DamageWorker.DamageResult();
            foreach (DamageInfo current in this.DamageInfosToApply(target))
            {
                if (target.ThingDestroyed)
                {
                    break;
                }
                else
                {
                    result = target.Thing.TakeDamage(current);
                }
                
            }
            return result;
        }
    }

    //Kill the Scuttlebug once it's transmitted the virus
    public class Scuttlebugs_DamageWorker : DamageWorker
    {
        //public override float Apply(DamageInfo dinfo, Thing thing)
        public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing thing)
        {
            if (thing as Pawn == null)
            {
                return base.Apply(dinfo, thing);
            }
            else
            {   //return base.Apply(dinfo, thing);
                Pawn pawn = thing as Pawn;          //targeted Pawn
                Pawn scuttlebug = thing as Pawn;    //scuttlebug Pawn

                DamageWorker.DamageResult result = base.Apply(dinfo, thing);

                Log.Warning(pawn + "has been infected!");
                BodyPartRecord torso = pawn.health.hediffSet.GetNotMissingParts().First(bpr => bpr.def == BodyPartDefOf.Torso);

                pawn.health.AddHediff(HediffDef.Named("ScuttlebugInfection"), torso, null);
         
                HealthUtility.DamageUntilDowned(pawn);

                var worm = dinfo.Instigator as ScuttleBugClass;
                if (worm != null)
                    worm.shouldDie = true;

                return result;
            }
        }
    }

    //Scuttlebug PawnKindDef - need this to be able to spawn it later
    [DefOf]
    public class Scuttlebugs_DefOf
    {
        public static PawnKindDef Scuttlebug;
    }

    public class ScuttleBugClass : Pawn
    {
        public bool shouldDie;

        public override void Tick()
        {
            base.Tick();
            if (shouldDie)
                Kill(null);
        }

    }

    //The Impregnation Hediff - runs down on a 24hr tick and then births another Scuttlebug
    public class ScuttlebugsHediff : HediffWithComps
    {

        public static void SpawnScuttlebug(Pawn pawn)
        {

            HediffDef ScuttlebugInfection = DefDatabase<HediffDef>.GetNamed("ScuttlebugInfection", true);
            BodyPartRecord torso = pawn.health.hediffSet.GetNotMissingParts().First(bpr => bpr.def == BodyPartDefOf.Torso);

            //Log.Warning(pawn + "has spawned a Scuttlebug!");

            int noWorms = UnityEngine.Random.Range(1,3);

            for (int i = 0; i < noWorms; i++)
            {

                PawnKindDef scuttlebug = Scuttlebugs_DefOf.Scuttlebug;
                Pawn newPawn = PawnGenerator.GeneratePawn(scuttlebug, null);

                GenSpawn.Spawn(newPawn, pawn.Position, pawn.MapHeld);
                newPawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent);
            }

            //remove the parasite infection before killing the colonist
            pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(ScuttlebugInfection));
            pawn.health.DropBloodFilth();

            pawn.TakeDamage(new DamageInfo(DamageDefOf.Bite, 50, 100, -1, null, torso));
        }

        int iCounter = 0;

        public override void Tick()
        {
            //ensure larvae spawns if colonist is killed
            ++iCounter;
            if (iCounter > 120000)
            {
                SpawnScuttlebug(pawn);
            }
            base.Tick();
        }


        public override bool Visible => ResearchProjectDef.Named("ScuttlebugsBiology").IsFinished;

    }

 
}