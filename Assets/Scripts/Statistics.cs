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

    public delegate void OnDeath();
    public OnDeath onDeath;

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
    }

    public void DealDamage(float damage)
    {
        health -= damage;
        if (gameObject)
        {
            FindObjectOfType<SpawnParticles>().SpawnBloodParticles(transform.position);
        }
        if(health <= 0)
        {
            onDeath.Invoke();
        }
    }

    public int GetScorePoints()
    {
        return scorePoints;
    }

    public void DealDamage(float damage, AttackTypes attackType)
    {
        if (vulnerability != AttackTypes.Normal)
        {
            if (attackType == vulnerability)
            {
                DealDamage(damage * vulnerabilityValue / 100);
            }
            else
            {
                DealDamage(damage * (100 - resistanceValue));
            }
        }
        else
        {
            DealDamage(damage);
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
