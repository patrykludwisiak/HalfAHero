using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    [SerializeField] private float dragStrength;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Vector2 knockback = Vector3.Normalize(transform.position - collision.transform.position);
            if(!collision.GetComponent<Knockback>())
            {
                collision.gameObject.AddComponent<Knockback>();
                StatusEffect.AddUniqueStatusEffect<Knockback>(collision.GetComponent<Knockback>(), collision.GetComponent<EnemyStatistics>().GetStatusEffects());
            }
            collision.GetComponent<Knockback>().PassData(collision.GetComponent<Rigidbody2D>(), knockback * dragStrength);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision.GetComponent<Knockback>())
        {
            StatusEffect.RemoveStatusEffect<Knockback>(collision.GetComponent<EnemyStatistics>().GetStatusEffects());
        }
    }
}
