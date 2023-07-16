using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Deflect : Ability
{
    [SerializeField] private float speed;
    public override bool Cast()
    {
        return base.Cast();
    }

    public bool Cast(GameObject gameObject, Vector2 deflectDirection, float speed, string targetTag)
    {
        if(gameObject.GetComponent<Bullet>())
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = deflectDirection * speed;
            gameObject.GetComponent<Bullet>().SetTargetTag(targetTag);
            return true;
        }
        return false;
    }

    public bool Cast(GameObject gameObject, Vector2 deflectDirection, string targetTag)
    {
        return Cast(gameObject, deflectDirection, speed, targetTag);
    }
}
