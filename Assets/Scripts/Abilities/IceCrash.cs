using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCrash : Crash
{
    [SerializeField] float damage;
    [SerializeField] float freezeLength;
    [SerializeField] GameObject freezePrefab;
    
    private void OnTriggerStay2D(Collider2D collider)
    {
        GameObject col = collider.gameObject;

        if (col.tag == "Enemy" && remainingTime > 0 && attackedEnemies.IndexOf(col) == -1)
        {
            col.GetComponentInParent<Statistics>().DealDamage(damage, AttackTypes.Water);
            attackedEnemies.Add(col);
            //col.GetComponentInParent<Rigidbody2D>().velocity = directionVector * 2.0f;
            if (col && !col.GetComponent<Slow>())
            {
                try
                {
                    col.AddComponent<Slow>();
                    col.GetComponent<Slow>().PassData(0f, freezePrefab);
                    StatusEffect.AddTimedStatusEffect(col.GetComponent<Slow>(), col.GetComponent<EnemyStatistics>().GetStatusEffects(), freezeLength);
                }
                catch (System.Exception)
                {}
            }
        }

    }
}
