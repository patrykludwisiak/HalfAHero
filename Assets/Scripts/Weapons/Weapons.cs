using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapons : MonoBehaviour
{
    protected bool isAttacking;
    protected bool isCharging;
    protected AttackTypes attackType;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected Collider2D collision;
    protected PlayerControl playerControl;
    protected PlayerStatistics playerStats;
    protected List<Statistics> enemiesToAttack;
    [SerializeField] protected Sprite inventorySprite;
    [SerializeField] protected List<GameObject> abilities;

    private void Awake()
    {
        abilities = Ability.LoadAbilities(gameObject, abilities);
    }
    protected virtual void Start()
    {
        enemiesToAttack = new List<Statistics>();
        attackType = AttackTypes.Normal;
        playerControl = GameObject.Find("Ifer").GetComponent<PlayerControl>();
        playerStats = playerControl.GetPlayerStats();
    }
    
    protected void OnTriggerStay2D(Collider2D collision)
    {
        string tag = collision.tag;
        if (isAttacking)
        {
            switch (tag)
            {
                case "Enemy":
                    EnemyStatistics enemy = collision.gameObject.GetComponent<EnemyStatistics>();
                    if (!enemy.IsAttacked())
                    {
                        enemy.ChangeAttacked(true);
                        playerControl.AddEnemyToAttackedEnemies(collision.gameObject);
                        enemiesToAttack.Add(enemy);
                    }
                    break;
                case "Spawner":
                    collision.GetComponent<EnemyRespawn>().Respawn();
                    break;
                case "BreakableTerrain":
                    collision.GetComponent<BarrelDestroy>().Destruct();
                    break;
            }
        }
    }

    public virtual bool Attack()
    {
        return isAttacking || isCharging;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    public bool IsCharging()
    {
        return isCharging;
    }

    public bool Move()
    {
        return false;
    }

    public Sprite GetInventorySprite()
    {
        return inventorySprite;
    }

    public void SetAttackType(AttackTypes attackType)
    {
        this.attackType = attackType;
    }

    public AttackTypes GetAttackType()
    {
        return attackType;
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }

    public void ResetColor()
    {
        spriteRenderer.color = Color.white;
    }

    protected AttackTypes DecideAttackType(AttackTypes attackType)
    {
        if (this.attackType != AttackTypes.Normal)
        {
            return this.attackType;
        }
        else
        {
            return attackType;
        }
    }

    public List<GameObject> GetAbilitiesList()
    {
        return abilities;
    }

    private void OnDestroy()
    {
        Ability.DestroyAbilities(abilities);
    }
}
