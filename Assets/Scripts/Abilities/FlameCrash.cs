using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameCrash : Crash
{
    [SerializeField] float damage;
    [SerializeField] float knockbackStrength;
    [SerializeField] float knockbackTime;
    [SerializeField] float burnDuration;
    [SerializeField] float timeBetweenBurnDamage;
    [SerializeField] float burnDamage;
    [SerializeField] GameObject burningPrefab;

    private void OnTriggerStay2D(Collider2D collider)
    {
        GameObject col = collider.gameObject;

        if (col.CompareTag("Enemy") && remainingTime > 0 && attackedEnemies.IndexOf(col) == -1)
        {
            col.GetComponentInParent<Statistics>().GetDamage(damage, AttackTypes.Fire);
            attackedEnemies.Add(col);
            //col.GetComponentInParent<Rigidbody2D>().velocity = directionVector * 2.0f;
            if (col)
            {
                if (!col.GetComponent<Knockback>())
                {
                    try
                    {
                        Vector2 knockback = Vector3.Normalize(col.transform.position - transform.position);
                        col.AddComponent<Knockback>();
                        col.GetComponent<Knockback>().PassData(col.GetComponent<Rigidbody2D>(), knockback * knockbackStrength);
                        StatusEffect.AddUniqueTimedStatusEffect<Knockback>(col.GetComponent<Knockback>(), col.GetComponent<EnemyStatistics>().GetStatusEffects(), knockbackTime);
                    }
                    catch (System.Exception)
                    { }
                }
                try
                {
                    col.AddComponent<Burn>();
                    col.GetComponent<Burn>().PassData(timeBetweenBurnDamage, burnDamage, burningPrefab);
                    StatusEffect.AddRefreshingTimedStatusEffect<Burn>(col.GetComponent<Burn>(), col.GetComponent<EnemyStatistics>().GetStatusEffects(), burnDuration);
                }
                        catch (System.Exception)
                { }
            }
        }

    }
}
