using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage;
    [SerializeField] private bool penetrate;
    [SerializeField] private bool destroyOnTime;
    [SerializeField] private int bounceCount = 3;
    [SerializeField] private float lifetime;
    [SerializeField] private float damageCooldown;
    [SerializeField] private AttackTypes attackType;
    private float lifeTimer = 0;
    private bool dealDamage = true;
    private List<AbilityOvertime> abilitiesOvertime = new List<AbilityOvertime>();
    protected string targetTag;
 
    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject col = collider.gameObject;
        OnHitDecision(col);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            int index = abilitiesOvertime.FindIndex(enemy => enemy.enemy == collision.gameObject);
            if(index != -1)
            {
                abilitiesOvertime.RemoveAt(index);
            }    
        }
    }
    private void Update()
    {
        if(destroyOnTime)
        {
            lifeTimer += Time.deltaTime;
            if(lifeTimer >= lifetime)
            {
                Destroy(gameObject);
            }
            for(int i = 0; i < abilitiesOvertime.Count; i++)
            {
                abilitiesOvertime[i].AddTime(Time.deltaTime);
                if(abilitiesOvertime[i].cooldown >= damageCooldown)
                {
                    abilitiesOvertime[i].ResetCooldown();
                    abilitiesOvertime[i].enemy.GetComponent<Statistics>().DealDamage(damage, attackType);
                }
            }
        }
    }
    protected void OnHitDecision(GameObject col)
    {
        OnSpawnerHit(col);
        OnBlockadeHit(col);
        OnTargetHit(col);
        OnBounceHit(col);
    }

    protected void OnSpawnerHit(GameObject col)
    {
        if (col.tag == "Spawner")
        {
            Destroy(gameObject);
        }
    }

    protected void OnBlockadeHit(GameObject col)
    {
        if (col.tag == "Blockade")
        {
            Destroy(gameObject);
        }
    }

    protected void OnTargetHit(GameObject col)
    {
        if (col.tag == targetTag)
        {
            if(!destroyOnTime)
            {
                col.GetComponentInParent<Statistics>().DealDamage(damage);
                Destroy(gameObject);
            }
            else
            {
                if(dealDamage)
                {
                    abilitiesOvertime.Add(new AbilityOvertime() { cooldown = 0, enemy = col } );
                }
            }
        }
    }

    protected void OnBounceHit(GameObject col)
    {
        if (col.tag == "BounceBullet")
        {
            float speed = GetComponent<Rigidbody2D>().velocity.magnitude;
            Vector2 normal = col.GetComponent<BounceWall>().GetNormal();
            Vector2 bounceVector = Vector2.Reflect(GetComponent<Rigidbody2D>().velocity, normal);
            GetComponent<Rigidbody2D>().velocity = bounceVector.normalized * speed;

            bounceCount--;

            if (bounceCount <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetTargetTag(string targetTag)
    {
        this.targetTag = targetTag;
    }

    public void SetPenetrate(bool isPenetrating)
    {
        penetrate = isPenetrating;
    }

    public bool IsPenetrating()
    {
        return penetrate;
    }
}
