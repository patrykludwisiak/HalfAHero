using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeeleAttack : MonoBehaviour
{
    private EnemyStatistics stats;
    private float attackSpeed;
    private float attackSpeedTimer;
    private float attackBreak;
    private float attackBreakTimer;
    private Vector2 playerVector;
    private Vector3 rotation;
    private GameObject player;
    private PlayerStatistics playerStats;
    [SerializeField] private SpriteRenderer spriteGameObject;
    private SpriteRenderer spriteTarget;
    private EnemyMovement movement;
    private void Awake()
    {
        movement = GetComponentInParent<EnemyMovement>();
        spriteTarget = GetComponentInChildren<SpriteRenderer>();
        stats = GetComponentInParent<EnemyStatistics>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStatistics>();
        gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
        attackSpeed = stats.GetAttackSpeed();
        attackBreak = stats.GetAttackBreak();
    }

    private float WeaponAngle(Vector2 toPlayerVector)
    {
        return Mathf.Floor((Mathf.Atan2(toPlayerVector.y, toPlayerVector.x) / Mathf.Deg2Rad + 202.5f) / 45f);
    }

    private void Update()
    {
        if (!stats.IsInRange() && !stats.IsAttacking())
        {
            playerVector = movement.GetToPlayerVector().normalized;
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, WeaponAngle(playerVector) * 45f);
        }
        else
        {
            if (!stats.IsAttacking() && attackBreakTimer <= 0) {
                stats.ChangeAttacking();
                attackSpeedTimer = attackSpeed;
                
                spriteTarget.enabled = true;
            }
        }

        if (!stats.IsAttacking())
        {
            attackBreakTimer -= Time.deltaTime;
        }
        else
        {
            attackSpeedTimer -= Time.deltaTime;
            if (attackSpeedTimer <= attackSpeed * 0.4f)
            {
                spriteGameObject.color = Color.red;
                spriteTarget.color = Color.black;
            }
            if (attackSpeedTimer <= 0)
            {
                if (stats.IsInRange())
                {
                    playerStats.GetDamage(stats.GetAttack());
                }
                stats.ChangeAttacking();
                attackBreakTimer = attackBreak;
                spriteTarget.color = Color.white;
                spriteGameObject.color = Color.white;
                spriteTarget.enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            stats.ChangeInRange(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            stats.ChangeInRange(false);
        }
    }
}
