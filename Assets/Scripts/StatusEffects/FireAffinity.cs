using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAffinity : AttackAffinity
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
            if (weapon.GetAttackType() != AttackTypes.Fire)
            {
                weapon.SetAttackType(AttackTypes.Fire);
                weapon.SetColor(Color.red);
            }
            if (!inventory.GetCurrentWeapon().GetComponentInChildren<FlameCrash>())
            {
                //GameObject flameCrash = Instantiate(crashPrefab);
                //inventory.GetCurrentWeapon().GetComponent<Weapons>().GetAbilitiesList().Add(flameCrash);
                GameObject flameCrash = Instantiate(crashPrefab);
                AbilityTest[] abilities = weapon.GetComponentsInChildren<AttackDashTest>();
                foreach (AbilityTest ability in abilities)
                {
                    ability.SetEndAbility(flameCrash.GetComponent<FlameCrash>());
                }
                flameCrash.transform.parent = inventory.GetCurrentWeapon().transform;
                flameCrash.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
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
        //Ability.DestroyAbility<FlameCrash>(inventory.GetCurrentWeapon().GetComponent<Weapons>().GetAbilitiesList());
        Destroy(inventory.GetComponentInChildren<FlameCrash>().gameObject);
        //Weapons weapon = inventory.GetCurrentWeapon().GetComponent<Weapons>();
        weapon.SetAttackType(AttackTypes.Normal);
        weapon.ResetColor();
    }
}