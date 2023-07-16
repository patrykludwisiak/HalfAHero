using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : StatusEffect
{
    Rigidbody2D rigid;
    Vector2 knockback;
    
    public void PassData(Rigidbody2D rigid, Vector2 knockback)
    {
        this.rigid = rigid;
        this.knockback = knockback;
    }

    public override float Effect()
    {
        rigid.AddForce(knockback);
        return base.Effect();
    }

    private void OnDestroy()
    {
        if(rigid)
        {
            rigid.velocity = Vector2.zero;
        }
    }
}
