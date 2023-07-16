using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackDash : Dash
{
    [SerializeField] private float damage;
    [SerializeField] private AttackTypes attackType;


    public bool Cast(float dashSpeed, List<float> dashMultipliers, Rigidbody2D rigidBody, Vector2 chargeVector, List<Statistics> targets,
        float damage, AttackTypes attackType, GameObject spriteObject)
    {
        bool returnValue = base.Cast(dashSpeed, dashMultipliers, rigidBody, chargeVector, spriteObject);
        if (isDashing)
        {
            transform.position = GameObject.Find("Ifer").transform.position;
            if (targets.Capacity > 0)
            {
                foreach (Statistics target in targets)
                {
                    if (target)
                    {
                        FindObjectOfType<GameControl>().Stop();
                        GameObject.Find("Ifer").GetComponent<PlayerStatistics>().AddAbilityCharge(10.0f);
                        target.DealDamage(damage, attackType);
                        FindObjectOfType<AudioManager>().Play("EnemyHitWithShield");
                    }
                }
                targets.Clear();
            }
        }
        return returnValue;
    }

    public bool Cast(List<float> dashMultipliers, Rigidbody2D rigidBody, Vector2 chargeVector, List<Statistics> targets, GameObject spriteObject)
    {
        return Cast(dashSpeed, dashMultipliers, rigidBody, chargeVector, targets, damage, attackType, spriteObject);
    }

    public bool Cast(List<float> dashMultipliers, Rigidbody2D rigidBody, Vector2 chargeVector, List<Statistics> targets,
        float damage, GameObject spriteObject)
    {
        return Cast(dashSpeed, dashMultipliers, rigidBody, chargeVector, targets, damage, attackType, spriteObject);
    }

    public bool Cast(List<float> dashMultipliers, Rigidbody2D rigidBody, Vector2 chargeVector, List<Statistics> targets,
        AttackTypes attackType, GameObject spriteObject)
    {
        return Cast(dashSpeed, dashMultipliers, rigidBody, chargeVector, targets, damage, attackType, spriteObject);
    }

    public bool Cast(List<float> dashMultipliers, Rigidbody2D rigidBody, Vector2 chargeVector, List<Statistics> targets,
        float damage, AttackTypes attackType, GameObject spriteObject)
    {
        return Cast(dashSpeed, dashMultipliers, rigidBody, chargeVector, targets, damage, attackType, spriteObject);
    }

    public bool Cast(float dashSpeed, List<float> dashMultipliers, Rigidbody2D rigidBody, Vector2 chargeVector, List<Statistics> targets, GameObject spriteObject)
    {
        return Cast(dashSpeed, dashMultipliers, rigidBody, chargeVector, targets, damage, attackType, spriteObject);
    }

    public bool Cast(float dashSpeed, List<float> dashMultipliers, Rigidbody2D rigidBody, Vector2 chargeVector, List<Statistics> targets,
        float damage, GameObject spriteObject)
    {
        return Cast(dashSpeed, dashMultipliers, rigidBody, chargeVector, targets, damage, attackType, spriteObject);
    }

    public bool Cast(float dashSpeed, List<float> dashMultipliers, Rigidbody2D rigidBody, Vector2 chargeVector, List<Statistics> targets,
        AttackTypes attackType, GameObject spriteObject)
    {
        return Cast(dashSpeed, dashMultipliers, rigidBody, chargeVector, targets, damage, attackType, spriteObject);
    }

    public override bool Cast(float dashSpeed, List<float> dashMultipliers, Rigidbody2D rigidBody, Vector2 chargeVector, GameObject spriteObject)
    {
        return false;
    }

    public override bool Cast(List<float> dashMultipliers, Rigidbody2D rigidBody, Vector2 chargeVector, GameObject spriteObject)
    {
        return false;
    }
}
