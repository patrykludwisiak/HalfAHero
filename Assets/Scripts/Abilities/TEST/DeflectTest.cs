using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeflectTest : AbilityTest
{
    // Start is called before the first frame update
    protected override AbilityReturn AbilityScript(WeaponTest weapon)
    {
        List<GameObject> objects = weapon.GetSharedObjects();
        List<GameObject> toRemove = new List<GameObject>();
        foreach (GameObject hit in objects)
        {
            if(hit == null)
            {
                toRemove.Add(hit);
            }
            else if (hit.GetComponent<Bullet>())
            {
                Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
                rb.velocity = weapon.GetLookVector() * rb.velocity.magnitude;
                hit.GetComponent<Bullet>().SetTargetTag("Enemy");
                toRemove.Add(hit);
            }
        }
        foreach (GameObject hit in toRemove)
        {
            weapon.RemoveFromSharedObjects(hit);
        }
        toRemove.Clear();
        return AbilityReturn.True;
    }
}
