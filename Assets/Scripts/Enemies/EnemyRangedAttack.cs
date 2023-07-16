using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    private EnemyStatistics stats;
    private EnemyMovement movement;
    private float attackSpeed;
    private float bulletSpeed;
    private float attackSpeedTimer;
    private float attackBreak;
    private float attackBreakTimer;
    private Vector2 playerVector;
    [SerializeField] private SpriteRenderer sprite;
    private void Awake()
    {
        stats = GetComponentInParent<EnemyStatistics>();
        movement = GetComponentInParent<EnemyMovement>();
        attackSpeed = stats.GetAttackSpeed();
        bulletSpeed = stats.GetBulletSpeed();
        attackBreak = stats.GetAttackBreak();
    }

    private void Update()
    {
        playerVector = GetComponentInParent<EnemyMovement>().GetToPlayerVector().normalized;
        if (stats.IsInRange() && movement.IsAlert())
        {
            if (!stats.IsAttacking() && attackBreakTimer <= 0)
            {
                stats.ChangeAttacking();
                attackSpeedTimer = attackSpeed;

                //to delete later
                sprite.color = Color.red;
            }
        }

        if (!stats.IsAttacking())
        {
            attackBreakTimer -= Time.deltaTime;

            //to delete later
            if (attackBreakTimer <= 0)
            {
                sprite.color = Color.green;
            }
        }
        else
        {
            attackSpeedTimer -= Time.deltaTime;
            if (attackSpeedTimer <= 0)
            {
                Shoot();
                stats.ChangeAttacking();
                attackBreakTimer = attackBreak;

                //to delete later
                sprite.color = Color.blue;
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

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = gameObject.transform.position;
        bullet.GetComponent<Rigidbody2D>().velocity = playerVector * bulletSpeed;
        bullet.GetComponent<Bullet>().SetDamage(stats.GetAttack());
        bullet.GetComponent<Bullet>().SetTargetTag("PlayerRanged");
    }
}
