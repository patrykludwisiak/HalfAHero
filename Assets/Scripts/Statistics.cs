using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Statistics : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private AttackTypes vulnerability;
    [SerializeField] private float vulnerabilityValue;
    [SerializeField] private float resistanceValue;
    [SerializeField] private int scorePoints;
    [SerializeField] private float height;

    public delegate void OnDeathHandler();
    public event OnDeathHandler OnDeath;
    public delegate void OnHealthChangeHandler();
    public event OnHealthChangeHandler OnHealthChange;

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealth()
    {
        return health;
    }

    public void ResetHealth()
    {
        health = maxHealth;
        OnHealthChange.Invoke();
    }

    public void GetDamage(float damage)
    {
        health -= damage;
        OnHealthChange.Invoke();
        if (gameObject)
        {
            FindObjectOfType<SpawnParticles>().SpawnBloodParticles(transform.position);
        }
        if(health <= 0)
        {
            OnDeath.Invoke();
        }
    }

    public int GetScorePoints()
    {
        return scorePoints;
    }

    public void GetDamage(float damage, AttackTypes attackType)
    {
        if (vulnerability != AttackTypes.Normal)
        {
            if (attackType == vulnerability)
            {
                GetDamage(damage * vulnerabilityValue / 100);
            }
            else
            {
                GetDamage(damage * (100 - resistanceValue));
            }
        }
        else
        {
            GetDamage(damage);
        }
    }

    public AttackTypes GetVulnerability()
    {
        return vulnerability;
    }

    public float GetVulnerabilityValue()
    {
        return vulnerabilityValue;
    }

    public float GetResistanceValue()
    {
        return vulnerabilityValue;
    }

    public void SetHeight(float height)
    {
        this.height = height;
    }

    public float GetHeight()
    {
        return height;
    }
}
