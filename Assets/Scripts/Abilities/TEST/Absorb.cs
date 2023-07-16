using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Absorb : AbilityTest
{
    [SerializeField] float decreasedStaminaOnHit;
    protected override AbilityReturn AbilityScript(WeaponTest weapon)
    {
        if (weapon.GetCurrentStamina() > 0)
        {
            List<GameObject> objectsHit = weapon.GetObjectHit();
            for (int i = 0; i < objectsHit.Count; i++)
            {
                if (weapon.GetCurrentStamina() > 0)
                {
                    if (objectsHit[i].GetComponent<Bullet>())
                    {
                        weapon.DecreaseCurrentStamina(decreasedStaminaOnHit);
                        Destroy(objectsHit[i]);
                    }
                }
                else
                {
                    break;
                }
            }
        }
        return AbilityReturn.True;
    }
}
