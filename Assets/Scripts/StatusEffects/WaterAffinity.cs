using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAffinity : AttackAffinity
{
    private GameObject crashPrefab;
    public void Init(GameObject crashPrefab)
    {
        this.crashPrefab = crashPrefab;
    }
    public override float Effect()
    {
        if (inventory.GetCurrentWeapon())
        {
            //Weapons weapon = inventory.GetCurrentWeapon().GetComponent<Weapons>();
            WeaponTest weapon = inventory.GetCurrentWeapon().GetComponent<WeaponTest>();
            if (weapon.GetAttackType() != AttackTypes.Water)
            {
                weapon.SetAttackType(AttackTypes.Water);
                weapon.SetColor(Color.blue);
            }
            if (!inventory.GetCurrentWeapon().GetComponentInChildren<IceCrash>())
            {
                //GameObject iceCrash = Instantiate(crashPrefab);
                //inventory.GetCurrentWeapon().GetComponent<Weapons>().GetAbilitiesList().Add(iceCrash);
                GameObject iceCrash = Instantiate(crashPrefab);
                AbilityTest[] abilities = weapon.GetComponentsInChildren<AttackDashTest>();
                foreach (AbilityTest ability in abilities)
                {
                    ability.SetEndAbility(iceCrash.GetComponent<IceCrash>());
                }
                iceCrash.transform.parent = inventory.GetCurrentWeapon().transform;
                iceCrash.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            }
        }
        return base.Effect();
    }

    private void OnDestroy()
    {
        WeaponTest weapon = inventory.GetCurrentWeapon().GetComponent<WeaponTest>();
        AbilityTest[] abilities = weapon.GetComponentsInChildren<AttackDashTest>();
        foreach (AbilityTest ability in abilities)
        {
            ability.SetEndAbility(null);
        }
        //Ability.DestroyAbility<IceCrash>(inventory.GetCurrentWeapon().GetComponent<Weapons>().GetAbilitiesList());
        Destroy(inventory.GetComponentInChildren<IceCrash>().gameObject);
        //Weapons weapon = inventory.GetCurrentWeapon().GetComponent<Weapons>();
        weapon.SetAttackType(AttackTypes.Normal);
        weapon.ResetColor();
    }
}
