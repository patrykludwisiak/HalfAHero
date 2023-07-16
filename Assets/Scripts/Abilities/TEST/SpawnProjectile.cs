using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnProjectile : AbilityTest
{
    [SerializeField] float speed;
    [SerializeField] float damage;
    [SerializeField] GameObject bulletPrefab;
    protected override AbilityReturn AbilityScript(WeaponTest weapon)
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = weapon.transform.position;
        bullet.GetComponent<Rigidbody2D>().velocity = weapon.GetLookVector() * speed;
        bullet.GetComponent<Bullet>().SetDamage(damage);
        bullet.GetComponent<Bullet>().SetTargetTag("Enemy");
        return AbilityReturn.True;
    }
}
