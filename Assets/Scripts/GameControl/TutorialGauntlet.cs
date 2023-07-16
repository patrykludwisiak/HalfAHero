using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGauntlet : MonoBehaviour
{
    GameControl gameControl;
    DialogController dialogController;
    List<GameObject> enemies;
    [SerializeField] GameObject rangedInBarrier1;
    [SerializeField] GameObject rangedInBarrier2;
    [SerializeField] GameObject barrier;
    [SerializeField] GameObject barrierCrystal;
    [SerializeField] GameObject winds;
    [SerializeField] GameObject nivek;
    [SerializeField] EnemyRespawn [] Respawn1;
    [SerializeField] EnemyRespawn [] Respawn2;
    Statistics barrierCrystalStats;
    EnemyMovement ranged1Stats;
    EnemyMovement ranged2Stats;
    Patrol nivekPatrol;
    bool gauntletStart, gauntletEnd, dialog1, dialog2;
    [SerializeField] float meleeRespawnTimer;
    [SerializeField] float rangedRespawnTimer;
    float meleeRespawnCurrentTimer;
    float rangedRespawnCurrentTimer;

    // Start is called before the first frame update
    void Start()
    {
        gameControl = Camera.main.GetComponent<GameControl>();
        enemies = new List<GameObject>();
        meleeRespawnCurrentTimer = 2;
        rangedRespawnCurrentTimer = 7;
        barrierCrystalStats = barrierCrystal.GetComponent<Statistics>();
        nivekPatrol = nivek.GetComponent<Patrol>();
        dialogController = Camera.main.GetComponent<DialogController>();
        ranged1Stats = rangedInBarrier1.GetComponent<EnemyMovement>();
        ranged2Stats = rangedInBarrier2.GetComponent<EnemyMovement>();
        gauntletStart = gauntletEnd = dialog1 = dialog2 = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (ranged1Stats.IsAlert() || ranged2Stats.IsAlert())
        {
            gauntletStart = true;
        }

        if (gauntletStart)
        {
            meleeRespawnCurrentTimer -= Time.deltaTime;
            rangedRespawnCurrentTimer -= Time.deltaTime;
            enemies.RemoveAll(enemy => enemy == null);
            if (enemies.Count < 4)
            {
                GameObject enemy = null;
                if (meleeRespawnCurrentTimer <= 0)
                {
                    int rand = Random.Range(0, 2);
                    enemy = Respawn1[rand].Respawn();
                    meleeRespawnCurrentTimer = meleeRespawnTimer;
                }
                if (rangedRespawnCurrentTimer <= 0)
                {
                    int rand = Random.Range(0, 2);
                    enemy = Respawn2[rand].Respawn();
                    rangedRespawnCurrentTimer = rangedRespawnTimer;
                }
                if (enemy != null)
                {
                    enemies.Add(enemy);
                }
            }
            if (barrierCrystalStats.GetHealth() <= 0)
            {
                gauntletStart = false;
                winds.SetActive(false);
                barrier.SetActive(false);
                gauntletEnd = true;
            }
        }
        if (!dialog1 && nivekPatrol.GetPatrolIndex() == 10 && gauntletStart)
        {
            nivekPatrol.SetBreakPoint(11);
            dialogController.AddTextToQueue("Nivek", "nivekTutorial18", 2f);
            dialogController.AddTextToQueue("Nivek", "nivekTutorial19", 4f);
            dialogController.AddTextToQueue("Nivek", "nivekTutorial20", 4f);
            dialog1 = true;
        }
        if(!dialog2 && gauntletEnd)
        {
            nivekPatrol.SetBreakPoint(13);
            dialogController.AddTextToQueue("Nivek", "nivekTutorial21", 2f);
            dialog2 = true;
        }
    }

    public void OnPlayerDeath()
    {
        gauntletStart = false;
    }
}
