using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private bool moveDuringAttack;
    [SerializeField] private bool escaping;
    [SerializeField] private float escapingRange;
    [SerializeField] private bool goesToNearestAlly;
    [SerializeField] private float nearestAllyFinderRange;
    [SerializeField] private float nearestAllyStopRange;
    [SerializeField] private float nearestAllyStopRangeModifier;
    private List<GameObject> allies;
    private Transform nearestAllyTranform;
    private bool isEscaping;
    private bool alert;
    private float movementSpeed;
    private float alertRange;
    private EnemyStatistics stats;
    private Vector2 playerPosition;
    private Vector2 position;
    private GameObject player;
    private NavMeshAgent navAgent;

    private void Awake()
    {
        stats = GetComponent<EnemyStatistics>();
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;
        movementSpeed = stats.GetMovementSpeed();
        alertRange = stats.GetAlertRange();
        player = GameObject.FindGameObjectWithTag("Player");
        isEscaping = false;
        nearestAllyTranform = null;
        allies = new List<GameObject>();
    }

    private float CombineSlow()
    {
        List<float> slowValues = StatusEffect.GetListOfEffectsValues<Slow>(stats.GetStatusEffects());
        float combinedSlow = 1;
        foreach (float slow in slowValues)
        {
            combinedSlow *= slow;
        }
        return combinedSlow;
    }

    public void Move()
    {
        bool isGoingToNearestEnemy = false;
        if (goesToNearestAlly)
        {
            if (!nearestAllyTranform)
            {
                Transform nearestAlly = GetNearestAlly();
                if (nearestAlly)
                {
                    navAgent.destination = nearestAlly.position;
                    navAgent.stoppingDistance = nearestAllyStopRange - nearestAllyStopRangeModifier;
                    isGoingToNearestEnemy = true;
                }
                else
                {
                    isGoingToNearestEnemy = false;
                }
            }
        }
        if (!goesToNearestAlly || !isGoingToNearestEnemy)
        {
            navAgent.destination = playerPosition;
            navAgent.speed = movementSpeed * CombineSlow();
        }
    }

    private void FixedUpdate()
    {
        StatusEffect.DoStatusEffects(stats.GetStatusEffects(), true);
        try
        {
            position = gameObject.transform.position;
            playerPosition = player.transform.position;
            float distance = Vector2.Distance(position, playerPosition);
            if (distance <= alertRange && Physics2D.Linecast(position, playerPosition, LayerMask.GetMask("Not Enemies")).collider.tag == "Player")
            {
                alert = true;
            }
            else if (distance > alertRange)
            {
                alert = false;
            }
        }
        catch (System.Exception)
        {
            alert = false;
        }
        if (!moveDuringAttack)
        {
            if ((stats.IsInRange() || !alert || stats.IsAttacking()) && !stats.IsAttacked())
            {
                navAgent.isStopped = true;
            }
            else
            {
                Move();
                navAgent.isStopped = false;
            }
        }
        else
        {
            if ((stats.IsInRange() || !alert) && !stats.IsAttacked())
            {
                navAgent.isStopped = true;
            }
            else
            {
                Move();
                navAgent.isStopped = false;
            }
        }
        if (escaping)
        {
            if (Vector2.Distance(position, playerPosition) < escapingRange)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = -GetToPlayerVector().normalized * stats.GetMovementSpeed();
                isEscaping = true;
            }
            else
            {
                isEscaping = false;
            }
        }
    }
    public Vector2 GetPlayerPosition()
    {
        return playerPosition;
    }

    public Vector2 GetToPlayerVector()
    {
        return playerPosition - position;
    }

    public Vector2 GetToPlayerVector(Vector2 position)
    {
        return playerPosition - position;
    }

    public bool IsAlert()
    {
        return alert;
    }

    public bool IsEscaping()
    {
        return isEscaping;
    }

    private Transform GetNearestAlly()
    {
        allies = GameData.GetEnemiesList(stats.GetEnemyFamily());
        Transform nearestEnemy = null;
        if (allies.Capacity > 0)
        {
            float nearestDistance = nearestAllyFinderRange;
            foreach (GameObject ally in allies)
            {
                Transform allyTransform = ally.transform;
                NavMeshPath path = new NavMeshPath();
                if (navAgent.CalculatePath(allyTransform.position, path))
                {
                    navAgent.path = path;
                    if (navAgent.remainingDistance < nearestDistance)
                    {
                        nearestDistance = navAgent.remainingDistance;
                        nearestEnemy = allyTransform;
                    }
                }
            }
        }
        return nearestEnemy;
    }

    public float GetNearestAllyStopRange()
    {
        return nearestAllyStopRange;
    }

    public Transform GetNearestAllyTransform()
    {
        return nearestAllyTranform;
    }

    public bool IsNearAlly()
    {
        return navAgent.isStopped;
    }

    public List<GameObject> GetNearAllies()
    {
        NavMeshPath oldPath = navAgent.path;
        List<GameObject> nearAllies = new List<GameObject>();
        foreach (GameObject ally in allies)
        {
            NavMeshPath path = new NavMeshPath();
            if (ally)
            {
                if (navAgent.CalculatePath(ally.transform.position, path))
                {
                    navAgent.path = path;
                    if (navAgent.remainingDistance < nearestAllyStopRange)
                    {
                        nearAllies.Add(ally);
                    }
                }
            }
        }
        navAgent.path = oldPath;
        return nearAllies;
    }
}
