using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneProjectileBehaviour : RuneProjectile
{
    [SerializeField] private float damage = 10f;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("WindRunePickable"))
        {
            collision.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity;
            Destroy(gameObject);
        }

        if(collision.tag == "Enemy")
        {
            collision.GetComponent<Statistics>().GetDamage(damage);
            Destroy(gameObject);
        }
    }
}
