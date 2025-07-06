using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace Scuttlebugs;

public class Verb_ScuttlebugDamage : Verb_MeleeAttackDamage
{
    private const float MeleeDamageRandomFactorMin = 0.8f;

    private const float MeleeDamageRandomFactorMax = 1.2f;

    private IEnumerable<DamageInfo> damageInfosToApply(LocalTargetInfo target)
    {
        var damAmount = verbProps.AdjustedMeleeDamageAmount(this, base.CasterPawn);
        var armorPenetration = verbProps.AdjustedArmorPenetration(this, base.CasterPawn);
        var damDef = verbProps.meleeDamageDef;
        BodyPartGroupDef bodyPartGroupDef = null;
        HediffDef hediffDef = null;
        damAmount = Rand.Range(damAmount * MeleeDamageRandomFactorMin, damAmount * MeleeDamageRandomFactorMax);
        if (base.CasterIsPawn)
        {
            bodyPartGroupDef = verbProps.AdjustedLinkedBodyPartsGroup(tool);
            if (damAmount >= 1f)
            {
                if (HediffCompSource != null)
                {
                    hediffDef = HediffCompSource.Def;
                }
            }
            else
            {
                damAmount = 1f;
                damDef = DamageDefOf.Blunt;
            }
        }

        var source = EquipmentSource != null ? EquipmentSource.def : base.CasterPawn.def;

        var direction = (target.Thing.Position - base.CasterPawn.Position).ToVector3();
        var def = damDef;
        var num = damAmount;
        var num2 = armorPenetration;
        var instigator = caster;
        var damageInfo = new DamageInfo(def, num, num2, -1f, instigator, null, source);
        damageInfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
        damageInfo.SetWeaponBodyPartGroup(bodyPartGroupDef);
        damageInfo.SetWeaponHediff(hediffDef);
        damageInfo.SetAngle(direction);
        yield return damageInfo;
        if (!surpriseAttack ||
            (verbProps.surpriseAttack == null || verbProps.surpriseAttack.extraMeleeDamages.NullOrEmpty()) &&
            (tool?.surpriseAttack == null || tool.surpriseAttack.extraMeleeDamages.NullOrEmpty()))
        {
            yield break;
        }

        var extraDamages = Enumerable.Empty<ExtraDamage>();
        if (verbProps.surpriseAttack is { extraMeleeDamages: not null })
        {
            extraDamages = extraDamages.Concat(verbProps.surpriseAttack.extraMeleeDamages);
        }

        if (tool is { surpriseAttack: not null } && !tool.surpriseAttack.extraMeleeDamages.NullOrEmpty())
        {
            extraDamages = extraDamages.Concat(tool.surpriseAttack.extraMeleeDamages);
        }

        foreach (var extraDamage in extraDamages)
        {
            var extraDamageAmount = GenMath.RoundRandom(extraDamage.AdjustedDamageAmount(this, base.CasterPawn));
            var extraDamageArmorPenetration = extraDamage.AdjustedArmorPenetration(this, base.CasterPawn);
            def = extraDamage.def;
            num2 = extraDamageAmount;
            num = extraDamageArmorPenetration;
            instigator = caster;
            var extraDamageInfo = new DamageInfo(def, num2, num, -1f, instigator, null, source);
            extraDamageInfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
            extraDamageInfo.SetWeaponBodyPartGroup(bodyPartGroupDef);
            extraDamageInfo.SetWeaponHediff(hediffDef);
            extraDamageInfo.SetAngle(direction);
            yield return extraDamageInfo;
        }
    }

    protected override DamageWorker.DamageResult ApplyMeleeDamageToTarget(LocalTargetInfo target)
    {
        var result = new DamageWorker.DamageResult();
        foreach (var current in damageInfosToApply(target))
        {
            if (target.ThingDestroyed)
            {
                break;
            }

            result = target.Thing.TakeDamage(current);
        }

        return result;
    }
}