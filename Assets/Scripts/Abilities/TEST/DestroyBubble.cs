using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DestroyBubble : AbilityTest
{
    protected override AbilityReturn AbilityScript(WeaponTest weapon)
    {
        GameObject bubble = weapon.GetSharedObject("Bubble");
        if (bubble != null)
        {
            weapon.RemoveFromSharedObjects(bubble);
            Destroy(bubble);
        }
        return AbilityReturn.True;
    }
}
