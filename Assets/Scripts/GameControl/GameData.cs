using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static GameObject player;
    public static PlayerStatistics playerStats;
    public static bool dead = false, isEnd = false;
    public static List<GameObject> enemiesOnScene;
    public static bool hitStopped = false;
    public static float hitStopTime;
    public static List<FadeAway> footSteps = new List<FadeAway>();
    public static List<FadeAway> footStepsToRemove = new List<FadeAway>();
    public static bool IsDead()
    {
        return dead;
    }

    public static void SetLanguage(string newLanguage)
    {
    }

    public static void SetEnd(bool end)
    {
        isEnd = end;
    }

    public static bool IsEnd()
    {
        return isEnd;
    }

    public static void AddEnemy(GameObject enemy)
    {
        enemiesOnScene.Add(enemy);
    }

    public static List<GameObject> GetEnemiesList()
    {
        return enemiesOnScene;
    }

    public static List<GameObject> GetEnemiesList(EnemyFamillies enemyFamiliy)
    {
        List<GameObject> enemies = new List<GameObject>();
        foreach (GameObject enemy in enemiesOnScene)
        {
            if (enemy.GetComponent<EnemyStatistics>().GetEnemyFamily() == enemyFamiliy)
            {
                enemies.Add(enemy);
            }
        }
        return enemies;
    }

    public static void AddFootstep(FadeAway footStep)
    {
        footSteps.Add(footStep);
    }

    public static void Reset()
    {
        player = null;
        playerStats = null;
        dead = false;
        isEnd = false;
    }
}
