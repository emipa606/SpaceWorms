using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace Scuttlebugs;

public class Verb_ScuttlebugDamage : Verb_MeleeAttackDamage
{
    private const float MeleeDamageRandomFactorMin = 0.8f;

    private const float MeleeDamageRandomFactorMax = 1.2f;

    private IEnumerable<DamageInfo> DamageInfosToApply(LocalTargetInfo target)
    {
        var damAmount = verbProps.AdjustedMeleeDamageAmount(this, base.CasterPawn);
        var armorPenetration = verbProps.AdjustedArmorPenetration(this, base.CasterPawn);
        var damDef = verbProps.meleeDamageDef;
        BodyPartGroupDef bodyPartGroupDef = null;
        HediffDef hediffDef = null;
        damAmount = Rand.Range(damAmount * 0.8f, damAmount * 1.2f);
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
        var mainDinfo = new DamageInfo(def, num, num2, -1f, instigator, null, source);
        mainDinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
        mainDinfo.SetWeaponBodyPartGroup(bodyPartGroupDef);
        mainDinfo.SetWeaponHediff(hediffDef);
        mainDinfo.SetAngle(direction);
        yield return mainDinfo;
        if (!surpriseAttack ||
            (verbProps.surpriseAttack == null || verbProps.surpriseAttack.extraMeleeDamages.NullOrEmpty()) &&
            (tool?.surpriseAttack == null || tool.surpriseAttack.extraMeleeDamages.NullOrEmpty()))
        {
            yield break;
        }

        var extraDamages = Enumerable.Empty<ExtraDamage>();
        if (verbProps.surpriseAttack is { extraMeleeDamages: { } })
        {
            extraDamages = extraDamages.Concat(verbProps.surpriseAttack.extraMeleeDamages);
        }

        if (tool is { surpriseAttack: { } } && !tool.surpriseAttack.extraMeleeDamages.NullOrEmpty())
        {
            extraDamages = extraDamages.Concat(tool.surpriseAttack.extraMeleeDamages);
        }

        foreach (var extraDamage in extraDamages)
        {
            var extraDamageAmount =
                GenMath.RoundRandom(extraDamage.AdjustedDamageAmount(this, base.CasterPawn));
            var extraDamageArmorPenetration = extraDamage.AdjustedArmorPenetration(this, base.CasterPawn);
            def = extraDamage.def;
            num2 = extraDamageAmount;
            num = extraDamageArmorPenetration;
            instigator = caster;
            var extraDinfo = new DamageInfo(def, num2, num, -1f, instigator, null, source);
            extraDinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
            extraDinfo.SetWeaponBodyPartGroup(bodyPartGroupDef);
            extraDinfo.SetWeaponHediff(hediffDef);
            extraDinfo.SetAngle(direction);
            yield return extraDinfo;
        }
    }

    protected override DamageWorker.DamageResult ApplyMeleeDamageToTarget(LocalTargetInfo target)
    {
        var result = new DamageWorker.DamageResult();
        foreach (var current in DamageInfosToApply(target))
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