using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBars : MonoBehaviour
{
    PlayerStatistics stats;
    Transform healthBar;
    //Transform staminaBar;
    Transform abilityBar;
    private void Start()
    {
        stats = GameObject.Find("Ifer").GetComponent<PlayerStatistics>();
        healthBar = transform.GetChild(0).GetChild(1);
        //staminaBar = transform.GetChild(1).GetChild(1);
        //abilityBar = transform.GetChild(1).GetChild(1);
    }
    // Update is called once per frame
    void Update()
    {
        if (stats.GetHealth() >= 0)
        {
            healthBar.localScale = new Vector3(stats.GetHealth() / stats.GetMaxHealth(), 1f);
        }
        else
        {
            healthBar.localScale = new Vector3(0f, 1f);
        }
        /*
        if (stats.GetStamina() >= 0)
        {
            staminaBar.localScale = new Vector3(stats.GetStamina() / stats.GetMaxStamina(), 1f);
        }
        else
        {
            staminaBar.localScale = new Vector3(0f, 1f);
        }
        if (stats.GetAbilityCharge() >= 0)
        {
            abilityBar.localScale = new Vector3(stats.GetAbilityCharge() / stats.GetMaxAbilityCharge(), 1f);
        }
        else
        {
            abilityBar.localScale = new Vector3(0f, 1f);
        }
        */
    }
}
