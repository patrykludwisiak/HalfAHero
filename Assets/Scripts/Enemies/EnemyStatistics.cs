using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyStatistics : Statistics
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private EnemyFamillies enemyFamily;
    [Space]
    [SerializeField] private float attackDamage;
    [SerializeField] private float alertRange;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float attackBreak;
    private List<StatusEffect> statusEffects;
    private bool isAttacking;
    private bool isInRange;
    [SerializeField] private bool attacked;

    private void Awake()
    {
        isAttacking = false;
        isInRange = false;
        statusEffects = new List<StatusEffect>();
    }

    public float GetAttack()
    {
        return attackDamage;
    }

    public float GetMovementSpeed()
    {
        return movementSpeed;
    }

    public float GetAlertRange()
    {
        return alertRange;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }

    public float GetBulletSpeed()
    {
        return bulletSpeed;
    }

    public float GetAttackBreak()
    {
        return attackBreak;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    public void ChangeAttacking()
    {
        isAttacking = !isAttacking;
    }

    public void ChangeAttacking(bool state)
    {
        isAttacking = state;
    }

    public bool IsInRange()
    {
        return isInRange;
    }

    public void ChangeInRange()
    {
        isInRange = !isInRange;
    }

    public void ChangeInRange(bool state)
    {
        isInRange = state;
    }

    public bool IsAttacked()
    {
        return attacked;
    }

    public void ChangeAttacked()
    {
        attacked = !attacked;
    }

    public void ChangeAttacked(bool state)
    {
        attacked = state;
    }

    public List<StatusEffect> GetStatusEffects()
    {
        return statusEffects;
    }

    public EnemyFamillies GetEnemyFamily()
    {
        return enemyFamily;
    }
}
