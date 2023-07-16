using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShieldSwipeTest : AbilityTest
{
    [SerializeField] private float lifespan;
    [SerializeField] private float damage;
    [SerializeField] private float swipeDistance;
    [SerializeField] private float knockbackStrength;
    [SerializeField] private float knockbackTime;
    private List<GameObject> attackedEnemies;
    private List<GameObject> hitBullets;
    private bool casting;
    private float lifespanTimer;

    private void Start()
    {
        lifespanTimer = lifespan;
        attackedEnemies = new List<GameObject>();
        hitBullets = new List<GameObject>();
    }

    protected override AbilityReturn AbilityScript(WeaponTest weapon)
    {
        casting = true;
        if (lifespanTimer == lifespan)
        {
            attackedEnemies.Clear();
            GetComponent<Animator>().PlayInFixedTime("SwipeAnimation");
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<Collider2D>().enabled = true;
            transform.position = (Vector2)weapon.GetPlayerTransform().position + (Vector2)Vector3.Normalize(weapon.GetLookVector()) * swipeDistance + new Vector2(0.0f, 0.2f);
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.Normalize(weapon.GetLookVector()));
        }
        else if(lifespanTimer > 0)
        {
            foreach(GameObject bullet in hitBullets)
            {
                weapon.AddToSharedObjects(bullet);
            }
        }
        lifespanTimer -= Time.fixedDeltaTime;
        if (lifespanTimer <= 0)
        {
            foreach(GameObject enemy in attackedEnemies)
            {
                enemy.GetComponent<Statistics>().DealDamage(damage, weapon.GetAttackType());
                if (!enemy.GetComponent<Knockback>())
                {
                    Vector2 knockback = Vector3.Normalize(enemy.transform.position - weapon.GetPlayerTransform().position);
                    enemy.AddComponent<Knockback>();
                    enemy.GetComponent<Knockback>().PassData(enemy.GetComponent<Rigidbody2D>(), knockback * knockbackStrength);
                    StatusEffect.AddTimedStatusEffect(enemy.GetComponent<Knockback>(), enemy.GetComponent<EnemyStatistics>().GetStatusEffects(), knockbackTime);
                }
            }
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            lifespanTimer = lifespan;
            casting = false;
            hitBullets.Clear();
            return AbilityReturn.True;
        }
        return AbilityReturn.False;
    }
    // Update is called once per frame

    private void OnTriggerStay2D(Collider2D collider)
    {
        GameObject col = collider.gameObject;

        if (casting && col.tag == "Enemy" && !attackedEnemies.Contains(col.gameObject))
        {
            attackedEnemies.Add(col);
        }
        else if (casting && col.CompareTag("BreakableTerrain"))
        {
            col.GetComponent<BarrelDestroy>().Destruct();
        }
        else if (casting && col.CompareTag("Bullet"))
        {
            if(hitBullets.IndexOf(col) == -1)
            {
                hitBullets.Add(col);
            }
        }

    }
}
