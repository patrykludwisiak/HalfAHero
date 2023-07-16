using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackAffinity : StatusEffect
{
    protected Inventory inventory;
    // Start is called before the first frame update
    void Awake()
    {
        inventory = GameObject.Find("Ifer").GetComponentInChildren<Inventory>();
    }

    public override float Effect()
    {
        return 0f;
    }
}
