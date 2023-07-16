using UnityEngine;
using UnityEngine.AI;

public class EnemyRespawn : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int amountOfEnemies;
    [SerializeField] private Vector3 respawnPosition;
    GameObject[] spawnedEnemies;
    

    private void Start()
    {
        spawnedEnemies = new GameObject[amountOfEnemies];
    }
    public GameObject Respawn()
    {
        GameObject enemy = null;
        bool found = false;
        int i = 0;
        if (amountOfEnemies > 0)
        {
            while (!found && i < amountOfEnemies)
            {
                if (spawnedEnemies[i] == null)
                {
                    enemy = spawnedEnemies[i] = Spawn();
                    found = true;
                }
                i++;
            }
        }
        else
        {
            enemy = Spawn();
        }
        return enemy;
    }
    
    private GameObject Spawn()
    {
        GameObject enemy = Instantiate(enemyPrefab);
        enemy.transform.position = respawnPosition;
        enemy.GetComponent<NavMeshAgent>().Warp(respawnPosition);
        return enemy;
    }
}
