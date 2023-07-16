using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaterRune : RuneItem
{
    [SerializeField] GameObject crashPrefab;
    public override bool UseItem(GameObject inventory)
    {
        bool toReturn = base.UseItem(inventory);
        if (!gameObject.GetComponent<WaterAffinity>() && !toReturn)
        {
            WaterAffinity aff = gameObject.AddComponent<WaterAffinity>();
            aff.Init(crashPrefab);
            StatusEffect.AddWeaponAffinityStatusEffect(GetComponent<WaterAffinity>(), playerControlTest.GetStatusEffects());
        }
        return toReturn;
    }

    public override void OnDrop()
    {
        StatusEffect.RemoveStatusEffect<WaterAffinity>(playerControlTest.GetStatusEffects());
    }
}
