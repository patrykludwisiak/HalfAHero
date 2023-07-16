using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStatistics : Statistics
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float attackDamage;
    [SerializeField] private float maxStamina;
    [SerializeField] private float slowMoveSpeed = 1;
    [SerializeField] private float abilityCharge = 0.0f;
    [SerializeField] private float maxAbilityCharge = 100.0f;
    private float stamina;

    public void Awake()
    {
        GameData.player = gameObject;
        GameData.playerStats = this;
    }

    public float GetAttackDamage()
    {
        return attackDamage;
    }

    public void SetAttackDamage(float attack)
    {
        attackDamage = attack;
    }

    public float GetMaxStamina()
    {
        return maxStamina;
    }

    public void IncreaseStamina(float stamina)
    {
        this.stamina += stamina;
        if(stamina > maxStamina)
        {
            this.stamina = maxStamina;
        }
        if(stamina < 0)
        {
            this.stamina = 0;
        }
    }

    public float GetStamina()
    {
        return stamina;
    }

    public float GetMovementSpeed()
    {
        return movementSpeed;
    }

    public float GetSlowMovementSpeed()
    {
        return slowMoveSpeed;
    }

    public float GetAbilityCharge()
    {
        return abilityCharge;
    }

    public float GetMaxAbilityCharge()
    {
        return maxAbilityCharge;
    }

    public void SetAbilityCharge(float charge)
    {
        abilityCharge = charge;
    }

    public void GetMaxAbilityCharge(float max)
    {
        maxAbilityCharge = max;
    }

    public void AddAbilityCharge(float charge)
    {
        if(abilityCharge + charge > maxAbilityCharge)
        {
            abilityCharge = maxAbilityCharge;
        }
        else
        {
            abilityCharge += charge;
        }
    }
}
