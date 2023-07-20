using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSwipe : MonoBehaviour
{
    [SerializeField] float lifespan;
    [SerializeField] float damage;
    [SerializeField] float knockbackStrength;
    [SerializeField] float knockbackTime;
    private List<GameObject> attackedEnemies;
    private Vector3 directionVector;

    private void Start()
    {
        directionVector = new Vector3(1.0f, 0, 0);
        attackedEnemies = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

        lifespan -= Time.deltaTime;

        if(lifespan < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        GameObject col = collider.gameObject;

        if (col.tag == "Enemy" && !attackedEnemies.Contains(col.gameObject))
        {
            col.GetComponentInParent<Statistics>().GetDamage(damage, GetComponentInParent<Weapons>().GetAttackType());
            attackedEnemies.Add(col.gameObject);
            //col.GetComponentInParent<Rigidbody2D>().velocity = directionVector * 2.0f;
            if (!col.GetComponent<Knockback>())
            {
                Vector2 knockback = Vector3.Normalize(col.transform.position - transform.position);
                col.AddComponent<Knockback>();
                col.GetComponent<Knockback>().PassData(col.GetComponent<Rigidbody2D>(), knockback*knockbackStrength);
                StatusEffect.AddTimedStatusEffect(col.GetComponent<Knockback>(), col.GetComponent<EnemyStatistics>().GetStatusEffects(), knockbackTime);
            }
        }

    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetDirectionVector(Vector3 dir)
    {
        directionVector = dir;
    }
}
