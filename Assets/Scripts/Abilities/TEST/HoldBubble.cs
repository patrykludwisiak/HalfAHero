using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HoldBubble : AbilityTest
{
    [SerializeField] GameObject bubble;
    [SerializeField] private string mainButton;
    [SerializeField] bool isTimed;
    protected override AbilityReturn AbilityScript(WeaponTest weapon)
    {
        if(isTimed)
        {
            Instantiate(bubble).transform.position = weapon.GetPlayerTransform().position;
        }
        else
        {
            GameObject temp = Instantiate(bubble);
            temp.transform.position = weapon.GetPlayerTransform().position;
            weapon.AddToSharedObjects(temp);
        }
        return AbilityReturn.True;
    }
}
