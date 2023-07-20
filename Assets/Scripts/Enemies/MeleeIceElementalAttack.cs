using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeIceElementalAttack : MonoBehaviour
{
    private EnemyStatistics stats;
    [SerializeField] private float meleeAttackSpeed;
    private float meleeAttackSpeedTimer;
    [SerializeField] private float meleeAttackBreak;
    private float meleeAttackBreakTimer;
    [SerializeField] private float meleeDamage;
    [SerializeField] Colliding meleeRangeColliding;
    private bool isMeleeAttacking;
    [SerializeField] private float rangedAttackSpeed;
    private float rangedAttackSpeedTimer;
    [SerializeField] private float rangedAttackBreak;
    private float rangedAttackBreakTimer;
    private bool isRangedAttacking;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform snowballTransform;
    [SerializeField] private Vector3 snowballTargetLocalScale;
    [SerializeField] private float snowballDamage;
    [SerializeField] Colliding rangedColliding;
    private float bulletSpeed;
    private Vector2 playerVector;
    private Vector3 rotation;
    private GameObject player;
    private PlayerStatistics playerStats;
    [SerializeField] private SpriteRenderer spriteGameObject;
    private SpriteRenderer spriteTarget;
    private EnemyMovement movement;
    private bool meleeCollision;
    private void Awake()
    {
        meleeCollision = false;
        movement = GetComponentInParent<EnemyMovement>();
        spriteTarget = GetComponentInChildren<SpriteRenderer>();
        stats = GetComponentInParent<EnemyStatistics>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStatistics>();
        gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
        bulletSpeed = stats.GetBulletSpeed();
        rangedAttackBreakTimer = 0;
        meleeAttackBreakTimer = 0;
    }

    private float WeaponAngle(Vector2 toPlayerVector)
    {
        return Mathf.Floor((Mathf.Atan2(toPlayerVector.y, toPlayerVector.x) / Mathf.Deg2Rad + 202.5f) / 45f);
    }

    private void Update()
    {
        stats.ChangeInRange(rangedColliding.IsColliding());
        if(stats.IsInRange())
        {
            snowballTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
            if (!stats.IsAttacking())
            {
                playerVector = movement.GetToPlayerVector().normalized;
                gameObject.transform.rotation = Quaternion.Euler(0f, 0f, WeaponAngle(playerVector) * 45f);
                if (rangedAttackBreakTimer <= 0 && !meleeRangeColliding.IsColliding())
                {
                    stats.ChangeAttacking(true);
                    isRangedAttacking = true;
                    rangedAttackSpeedTimer = rangedAttackSpeed;
                }
                else if(meleeAttackBreakTimer <= 0 && meleeRangeColliding.IsColliding())
                {
                    stats.ChangeAttacking(true);
                    isMeleeAttacking = true;
                    meleeAttackSpeedTimer = meleeAttackSpeed;
                    spriteTarget.enabled = true;
                }
            }
        }

        if (!stats.IsAttacking() || isMeleeAttacking)
        {
            if (rangedAttackBreakTimer > 0)
            {
                rangedAttackBreakTimer -= Time.deltaTime;
            }
        }
        if (!stats.IsAttacking() || isRangedAttacking)
        {
            if (meleeAttackBreakTimer > 0)
            {
                meleeAttackBreakTimer -= Time.deltaTime;
            }
        }
        
        if(stats.IsAttacking())
        {
            if (meleeAttackSpeedTimer > 0 && isMeleeAttacking)
            {
                float color = (meleeAttackSpeed - meleeAttackSpeedTimer) / meleeAttackSpeed;
                spriteTarget.color = new Color(color, color, color);
                meleeAttackSpeedTimer -= Time.deltaTime;
            }
            else if(rangedAttackSpeedTimer > 0 && isRangedAttacking)
            {
                float scale = (rangedAttackSpeed - rangedAttackSpeedTimer) / rangedAttackSpeed;
                snowballTransform.localScale = new Vector3(scale * snowballTargetLocalScale.x, scale * snowballTargetLocalScale.x, 1);
                rangedAttackSpeedTimer -= Time.deltaTime;
            }
            if (meleeAttackSpeedTimer <= 0 && isMeleeAttacking)
            {
                if (meleeCollision)
                {
                    playerStats.GetDamage(meleeDamage, AttackTypes.Water);
                }
                stats.ChangeAttacking(false);
                meleeAttackBreakTimer = meleeAttackBreak;
                spriteTarget.color = Color.cyan;
                spriteTarget.enabled = false;
                isMeleeAttacking = false;
            }
            else if (rangedAttackSpeedTimer <= 0 && isRangedAttacking)
            {
                Shoot();
                stats.ChangeAttacking(false);
                snowballTransform.localScale = new Vector3(0, 0, 0);
                rangedAttackBreakTimer = rangedAttackBreak;
                isRangedAttacking = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            meleeCollision = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            meleeCollision = false;
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = snowballTransform.position;
        bullet.transform.localScale = snowballTransform.lossyScale;
        bullet.GetComponent<Rigidbody2D>().velocity = playerVector * bulletSpeed;
        bullet.GetComponent<Bullet>().SetDamage(snowballDamage);
        bullet.GetComponent<Bullet>().SetTargetTag("PlayerRanged");
    }
}
