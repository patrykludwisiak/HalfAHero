using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RuneItem : Item
{
    ContactFilter2D filter;
    List<Collider2D> colliders;
    protected override void Start()
    {
        colliders = new List<Collider2D>();
        base.Start();
        filter = new ContactFilter2D()
        {
            layerMask = LayerMask.GetMask("Default"),
            useTriggers = true
        };
    }

    public override bool UseItem(GameObject inventory)
    {
        inventory.GetComponent<Collider2D>().OverlapCollider(filter, colliders);
        foreach (Collider2D collider in colliders)
        {
            GameObject gameObject = collider.gameObject;
            if (gameObject.name.Contains("Altar"))
            {
                ItemHolder holder = gameObject.GetComponent<ItemHolder>();
                if (!holder.GetItem())
                {
                    holder.AddItem(this.gameObject);
                    OnDrop();
                }
                return true;
            }
        }
        return false;
    }
}
