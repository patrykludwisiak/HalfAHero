using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityReturn
{
    False, True, Skip
}

public enum AbilityState
{
    start, during, end
}

[System.Serializable]
public class AbilityTest : MonoBehaviour
{
    [SerializeField] protected AbilityTest abilityOnCastingStart;
    [SerializeField] protected AbilityTest abilityDuringCasting;
    [SerializeField] protected AbilityTest abilityOnCastingEnd;
    [SerializeField] protected bool IsSkippable;
    protected AbilityTest skippedStartAbility;
    protected AbilityTest skippedEndbility;
    protected AbilityState abilityState = AbilityState.start;
    protected bool isOnStart;
    protected bool isDuring;
    protected bool isOnEnd;
    // Start is called before the first frame update

    // Update is called once per frame
    public virtual AbilityReturn Cast(WeaponTest weapon)
    {
        if (abilityState == AbilityState.start)
        {
            if (!abilityOnCastingStart)
            {
                abilityState = AbilityState.during;
            }
            else
            {
                if (abilityOnCastingStart.Cast(weapon) == AbilityReturn.True)
                {
                    abilityState = AbilityState.during;
                }
                else if (abilityOnCastingStart.Cast(weapon) == AbilityReturn.Skip)
                {
                    abilityState = AbilityState.during;
                    skippedStartAbility = abilityDuringCasting;
                }
            }
        }
        if (abilityState == AbilityState.during)
        {
            if (AbilityScript(weapon) == AbilityReturn.True)
            {
                abilityState = AbilityState.end;
            }
            if (abilityDuringCasting)
            {
                abilityDuringCasting.Cast(weapon);
            }
            if(skippedStartAbility)
            {
                if(skippedStartAbility.Cast(weapon) == AbilityReturn.True)
                {
                    skippedStartAbility = null;
                }
            }
        }
        if(abilityState == AbilityState.end)
        {
            if (abilityOnCastingEnd)
            {
                if (abilityOnCastingEnd.IsSkippable)
                {
                    skippedEndbility = abilityOnCastingEnd;
                    abilityState = AbilityState.start;
                    return AbilityReturn.Skip;
                }
                else if (abilityOnCastingEnd.Cast(weapon) == AbilityReturn.True)
                {
                    abilityState = AbilityState.start;
                    return AbilityReturn.True;
                }
            }
            else
            {
                abilityState = AbilityState.start;
                return AbilityReturn.True;
            }
        }
        if(IsSkippable)
        {
            return AbilityReturn.Skip;
        }
        return AbilityReturn.False;
    }

    public AbilityReturn CastSkippedEnd(WeaponTest weapon)
    {
        if(skippedEndbility.Cast(weapon) == AbilityReturn.True)
        {
            skippedEndbility = null;
            return AbilityReturn.True;
        }
        return AbilityReturn.False;
    }

    protected virtual AbilityReturn AbilityScript(WeaponTest weapon)
    {
        return AbilityReturn.True;
    }

    public void InstantiateAbilities()
    {
        if(abilityOnCastingStart)
        {
            abilityOnCastingStart = Instantiate(abilityOnCastingStart, transform);
            abilityOnCastingStart.InstantiateAbilities();
        }
        if (abilityDuringCasting)
        {
            abilityDuringCasting = Instantiate(abilityDuringCasting, transform);
            abilityDuringCasting.InstantiateAbilities();
        }
        if (abilityOnCastingEnd)
        {
            abilityOnCastingEnd = Instantiate(abilityOnCastingEnd, transform);
            abilityOnCastingEnd.InstantiateAbilities();
        }
    }

    public void SetStartAbility(AbilityTest ability)
    {
        abilityOnCastingStart = ability;
    }

    public void SetDuringAbility(AbilityTest ability)
    {
        abilityDuringCasting = ability;
    }

    public void SetEndAbility(AbilityTest ability)
    {
        abilityOnCastingEnd = ability;
    }
}
