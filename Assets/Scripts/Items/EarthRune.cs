using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EarthRune : RuneItem
{ 
    public override bool UseItem(GameObject inventory)
    {
        bool toReturn = base.UseItem(inventory);
        if (!gameObject.GetComponent<Immovable>() && !toReturn)
        {
            gameObject.AddComponent<Immovable>();
            gameObject.GetComponent<Immovable>().PassData(playerControlTest.GetStatusEffects(), playerControlTest.GetPlayerRenderer());
            StatusEffect.AddUniqueTimedStatusEffect<Immovable>(GetComponent<Immovable>(), playerControlTest.GetStatusEffects(), 10f);
        }
        return toReturn;
    }
}
    