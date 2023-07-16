using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AbilityOvertime
{
    public GameObject enemy;
    public float cooldown;

    public void AddTime(float time)
    {
        cooldown += time;
    }

    public void ResetCooldown()
    {
        cooldown = 0;
    }

}
