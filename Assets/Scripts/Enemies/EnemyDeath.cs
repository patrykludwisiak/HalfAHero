using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    GameObject self;
    EnemyStatistics stats;
    Drop drop;
    [SerializeField] bool isDropping;

    private void Start()
    {
        self = gameObject;
        stats = gameObject.GetComponent<EnemyStatistics>();
        if (isDropping)
        {
            drop = gameObject.GetComponent<Drop>();
        }
        stats.OnDeath += OnDeath;
    }

    void OnDeath()
    {
        if (isDropping)
        {
            drop.DropItem(transform.position);
        }
        Destroy(gameObject);
    }
}
