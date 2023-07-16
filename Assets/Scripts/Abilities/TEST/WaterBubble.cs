using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBubble : MonoBehaviour
{
    [SerializeField] float time;
    [SerializeField] GameObject slowPrefab;
    List<GameObject> enemies = new List<GameObject>();
    float timer;
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            timer += Time.deltaTime;
            if (timer > time)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Bullet>())
        {
            Destroy(collision.gameObject);
        }
        else if(collision.CompareTag("Enemy"))
        {
            if (!collision.GetComponent<Slow>())
            {
                collision.gameObject.AddComponent<Slow>();
                collision.GetComponent<Slow>().PassData(0.5f,slowPrefab);
                StatusEffect.AddStatusEffect(collision.GetComponent<Slow>(), collision.GetComponent<EnemyStatistics>().GetStatusEffects());
                enemies.Add(collision.gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if(collision)
            {
                Slow slow = collision.GetComponent<Slow>();
                if (slow)
                {
                    StatusEffect.RemoveStatusEffect<Slow>(collision.GetComponent<EnemyStatistics>().GetStatusEffects());
                }
            }
        }
    }

    private void OnDestroy()
    {
        foreach(GameObject enemy in enemies)
        {
            if(enemy)
            {
                Slow slow = enemy.GetComponent<Slow>();
                if (slow)
                {
                    StatusEffect.RemoveStatusEffect<Slow>(enemy.GetComponent<EnemyStatistics>().GetStatusEffects());
                }
            }
        }
    }
}
