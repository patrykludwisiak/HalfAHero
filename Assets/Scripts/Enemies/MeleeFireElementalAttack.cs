using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeFireElementalAttack : MonoBehaviour
{
    private EnemyStatistics stats;
    private float attackSpeed;
    private float attackSpeedTimer;
    [SerializeField] private float attackDuration;
    private float attackDurationTimer;
    [SerializeField] private float attackTick;
    private float attackTickTimer;
    private float attackBreak;
    private float attackBreakTimer;
    private Vector2 playerVector;
    private Vector3 rotation;
    private GameObject player;
    private PlayerStatistics playerStats;
    [SerializeField] private SpriteRenderer spriteGameObject;
    [SerializeField] private Transform coverageObjectTransform;
    private Vector3 finalScale;
    private SpriteRenderer spriteTarget;
    private EnemyMovement movement;
    private void Awake()
    {
        movement = GetComponentInParent<EnemyMovement>();
        spriteTarget = transform.GetChild(1).GetComponent<SpriteRenderer>();
        stats = GetComponentInParent<EnemyStatistics>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStatistics>();
        attackSpeed = stats.GetAttackSpeed();
        attackBreak = stats.GetAttackBreak();
        finalScale = transform.GetChild(1).localScale;
        attackDurationTimer = 0;
        attackTickTimer = 0;
    }

    private float WeaponAngle(Vector2 toPlayerVector)
    {
        return Mathf.Floor((Mathf.Atan2(toPlayerVector.y, toPlayerVector.x) / Mathf.Deg2Rad + 202.5f) / 45f);
    }

    private void Update()
    {
        if (!stats.IsInRange() && !stats.IsAttacking())
        {
        }
        else
        {
            if (!stats.IsAttacking() && attackBreakTimer <= 0)
            {
                stats.ChangeAttacking();
                attackSpeedTimer = attackSpeed;

                spriteTarget.enabled = true;
            }
        }

        if (!stats.IsAttacking())
        {
            attackBreakTimer -= Time.deltaTime;
            coverageObjectTransform.localScale = new Vector3(0, 0, 0);
        }
        else
        {
            if(attackSpeedTimer>0)
            {
                float scale = attackSpeed - attackSpeedTimer / attackSpeed;
                coverageObjectTransform.localScale = new Vector3(scale * finalScale.x, scale * finalScale.y, 1);
                attackDurationTimer = attackDuration;
                attackSpeedTimer -= Time.deltaTime;
            }
            if (attackSpeedTimer <= 0)
            {
                if(attackDurationTimer > 0)
                {
                    if (stats.IsInRange() && attackTickTimer <=0)
                    {
                        playerStats.GetDamage(stats.GetAttack(), AttackTypes.Fire);
                        attackTickTimer = attackTick;
                    }
                    attackTickTimer -= Time.deltaTime;
                    attackDurationTimer -= Time.deltaTime;
                }
                else
                {
                    stats.ChangeAttacking();
                    attackBreakTimer = attackBreak;
                    spriteTarget.enabled = false;
                }
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
