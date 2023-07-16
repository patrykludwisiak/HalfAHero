using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Slow : StatusEffect
{
    private float slowValue;
    private GameObject slowPrefab
;
    public void PassData(float slowValue, GameObject slowPrefab)
    {
        this.slowPrefab = Instantiate(slowPrefab, gameObject.transform);
        if (slowValue < 0)
        {
            this.slowValue = 0;
        }
        else if(slowValue > 1)
        {
            this.slowValue = 1;
        }
        else
        {
            this.slowValue = slowValue;
        }
    }

    public override float Effect()
    {
        return base.Effect()+slowValue;
    }

    private void OnDestroy()
    {
        Destroy(slowPrefab);
    }
}
