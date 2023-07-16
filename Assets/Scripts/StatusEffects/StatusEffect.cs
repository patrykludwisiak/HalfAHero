using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class StatusEffect : MonoBehaviour
{
    protected float timer;
    protected bool isTimed;

    public virtual float Effect()
    {
        return 0.0f;
    }

    public static List<float> GetListOfEffectsValues<T>(List<StatusEffect> statusEffects) where T : StatusEffect
    {
        List<float> multipliers = new List<float>();
        List<T> states = statusEffects.OfType<T>().ToList();
        foreach(T status in states)
        {
            multipliers.Add(status.Effect());
        }
        return multipliers;
    }
    
    public static void AddStatusEffect(StatusEffect statusEffect, List<StatusEffect> statusEffects)
    {
        statusEffects.Add(statusEffect);
    }

    public static void AddTimedStatusEffect(StatusEffect statusEffect, List<StatusEffect> statusEffects, float timer)
    {
        statusEffects.Add(statusEffect);
        statusEffect.timer = timer;
        statusEffect.isTimed = true;
    }

    public static void AddUniqueStatusEffect<T>(StatusEffect statusEffect, List<StatusEffect> statusEffects) where T : StatusEffect
    {
        int index = GetIndexOfEffect<T>(statusEffects);
        if (index != -1)
        {
            Destroy(statusEffects[index]);
            statusEffects[index] = statusEffect;
        }
        else
        {
            statusEffects.Add(statusEffect);
        }
    }
    
    public static void AddUniqueTimedStatusEffect<T>(StatusEffect statusEffect, List<StatusEffect> statusEffects, float timer) where T : StatusEffect
    {
        int index = GetIndexOfEffect<T>(statusEffects);
        if (index != -1)
        {
            Destroy(statusEffects[index]);
            statusEffects[index] = statusEffect;
            statusEffects[index].SetTimer(timer);
        }
        else
        {
            statusEffects.Add(statusEffect);
            statusEffect.timer = timer;
            statusEffect.isTimed = true;
        }
    }

    public static void AddRefreshingTimedStatusEffect<T>(StatusEffect statusEffect, List<StatusEffect> statusEffects, float timer) where T : StatusEffect
    {
        int index = GetIndexOfEffect<T>(statusEffects);
        if (index != -1)
        {
            statusEffects[index].SetTimer(timer);
        }
        else
        {
            statusEffects.Add(statusEffect);
            statusEffect.timer = timer;
            statusEffect.isTimed = true;
        }
    }

    public static void AddWeaponAffinityStatusEffect(StatusEffect statusEffect, List<StatusEffect> statusEffects, float timer)
    {
        AddUniqueTimedStatusEffect<AttackAffinity>(statusEffect, statusEffects, timer);
    }

    public static void AddWeaponAffinityStatusEffect(StatusEffect statusEffect, List<StatusEffect> statusEffects)
    {
        AddUniqueStatusEffect<AttackAffinity>(statusEffect, statusEffects);
    }

    public static bool IsStatusEffectApplied<T>(List<StatusEffect> statusEffects) where T : StatusEffect
    {
        if (statusEffects.OfType<T>().Count() > 0)
        {
            return true;
        }
        return false;
    }

    public static int GetIndexOfEffect<T>(List<StatusEffect> statusEffects) where T : StatusEffect
    {
        if (IsStatusEffectApplied<T>(statusEffects))
        {
            return statusEffects.IndexOf(statusEffects.OfType<T>().First());
        }
        return -1;
    }

    public static bool RemoveStatusEffect<T>(List<StatusEffect> statusEffects) where T : StatusEffect
    {
        if (IsStatusEffectApplied<T>(statusEffects))
        {
            int index = GetIndexOfEffect<T>(statusEffects);
            Destroy(statusEffects[index]);
            return true;
        }
        return false;
    }

    public static void DoStatusEffects(List<StatusEffect> statusEffects, bool isFixedUpdate)
    {
        statusEffects.RemoveAll(IsNull);
        foreach (StatusEffect effect in statusEffects)
        {
            if (effect != null)
            {
                effect.Effect();
                if(effect.isTimed)
                {
                    if(isFixedUpdate)
                    {
                        effect.DecreseTimerFixed();
                    }
                    else
                    {
                        effect.DecreseTimer();
                    }
                }
            }
        }
    }

    private static bool IsNull(StatusEffect status)
    {
        if(status == null)
        {
            return true;
        }
        return false;
    }

    public bool IsTimed()
    {
        return isTimed;
    }

    public void ChangeIsTimed(bool isTimed)
    {
        this.isTimed = isTimed;
    }

    public float GetTimer()
    {
        return timer;
    }

    public void SetTimer(float timer)
    {
        this.timer = timer;
    }

    public void DecreseTimer()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            Destroy(this);
        }
    }

    public void DecreseTimerFixed()
    {
        timer -= Time.fixedDeltaTime;
        if (timer <= 0)
        {
            Destroy(this);
        }
    }

}
