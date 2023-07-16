using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Immovable : StatusEffect
{
    List<StatusEffect> statusEffects;
    SpriteRenderer playerRenderer;

    void Awake()
    {
        timer = 0;
        isTimed = true;
    }

    public void PassData(List<StatusEffect> statusEffects, SpriteRenderer renderer)
    {
        this.statusEffects = statusEffects;
        playerRenderer = renderer;
    }

    public override float Effect()
    {
        if (timer > 0 )
        {
            RemoveStatusEffect<Knockback>(statusEffects);
            playerRenderer.color = Color.gray;
        }
        else if (timer <= 0)
        {
            playerRenderer.color = Color.white;
        }
        return base.Effect();
    }
}
