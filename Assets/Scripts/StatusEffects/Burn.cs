using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : StatusEffect
{
    float timeBetweenDamage;
    float countdown;
    float damage;
    GameObject burningPrefab;

    public void PassData(float timeBetweenDamage, float damage, GameObject burningPrefab)
    {
        this.timeBetweenDamage = timeBetweenDamage;
        countdown = timeBetweenDamage;
        this.burningPrefab = Instantiate(burningPrefab, gameObject.transform);
        this.damage = damage;
    }

    public override float Effect()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            gameObject.GetComponent<Statistics>().DealDamage(damage, AttackTypes.Fire);
            countdown = timeBetweenDamage;
        }
        return base.Effect();
    }

    private void OnDestroy()
    {
        Destroy(burningPrefab);
        Effect();
    }
}
