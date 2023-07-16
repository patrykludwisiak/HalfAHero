using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBars : MonoBehaviour
{
    Statistics stats;
    GameObject healthBar;
    GameObject backgroundHealthBar;

    private void Start()
    {
        stats = GetComponentInParent<Statistics>();
        healthBar = transform.Find("Bar").gameObject;
        backgroundHealthBar = transform.Find("HealthBackground").gameObject;
    }

    void Update()
    {
        if(stats.GetHealth() == stats.GetMaxHealth())
        {
            healthBar.SetActive(false);
            backgroundHealthBar.SetActive(false);
        }
        else
        {
            if(!healthBar.activeSelf || !backgroundHealthBar.activeSelf)
            {
                healthBar.SetActive(true);
                backgroundHealthBar.SetActive(true);
            }
            Transform bar = healthBar.transform;
            if (stats.GetHealth() >= 0)
            {
                bar.localScale = new Vector3(stats.GetHealth() / stats.GetMaxHealth(), 1f);
            }
            else
            {
                bar.localScale = new Vector3(0f, 1f);
            }
        }
        
    }
}
