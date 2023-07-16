using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FireRune : RuneItem
{
    [SerializeField] GameObject crashPrefab;
    public override bool UseItem(GameObject inventory)
    {
        bool toReturn = base.UseItem(inventory);
        if (!gameObject.GetComponent<FireAffinity>() && !toReturn)
        {
            FireAffinity aff = gameObject.AddComponent<FireAffinity>();
            aff.Init(crashPrefab);
            StatusEffect.AddWeaponAffinityStatusEffect(GetComponent<FireAffinity>(), playerControlTest.GetStatusEffects());
        }
        return toReturn;
    }

    public override void OnDrop()
    {
        StatusEffect.RemoveStatusEffect<FireAffinity>(playerControlTest.GetStatusEffects());
    }
}
