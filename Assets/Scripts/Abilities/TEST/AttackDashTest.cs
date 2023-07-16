using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackDashTest : DashTest
{
    // Start is called before the first frame update
    [SerializeField] float damage;
    protected override AbilityReturn AbilityScript(WeaponTest weapon)
    {
        if (!isDashing)
        {
            weapon.EnableCollision(true);
            weapon.EnableRender(true);
        }
        AbilityReturn returnValue = base.AbilityScript(weapon);
        List<GameObject> enemies = weapon.GetObjectHit();
        int size = enemies.Count;
        for (int i = 0; i < size; i++)
        {
            if(enemies[i] && enemies[i].tag == "Enemy")
            {
                if(enemies[i])
                {
                    Camera.main.GetComponent<GameControl>().Stop();
                    enemies[i].GetComponent<EnemyStatistics>().DealDamage(damage, weapon.GetAttackType());
                    FindObjectOfType<AudioManager>().Play("EnemyHitWithShield");
                    enemies.RemoveAt(i);
                    i--;
                    size--;
                }
            }
            else if(enemies[i] && enemies[i].tag == "BreakableTerrain")
            {
                enemies[i].GetComponent<BarrelDestroy>().Destruct();
                i--;
                size--;
            }
            else if (!enemies[i])
            {
                enemies.RemoveAt(i);
                i--;
                size--;
            }
        }
        if (returnValue == AbilityReturn.True)
        {
            weapon.EnableCollision(false);
            weapon.EnableRender(false);
        }
        return returnValue;
    }
}
