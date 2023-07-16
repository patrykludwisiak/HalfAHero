using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjBehaviour : RuneProjectile
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("BurnableWall"))
        {
            collision.GetComponent<EnemyStatistics>().DealDamage(1);
            Destroy(gameObject);
        }
    }
}
