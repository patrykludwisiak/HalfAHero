using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField] List<Vector3> patrolPositions;
    [SerializeField] List<int> teleportPoints;
    GameObject self;
    TutorialBeach control;
    Rigidbody2D rigid;
    NPCStatistics stats;
    Vector3 position;
    bool isMoving;
    bool isStopped;
    bool isPatroling;
    int currPatrolIndex;
    int breakPoint;
    private void Start()
    {
        isStopped = false;
        isMoving = false;
        isPatroling = true;
        breakPoint = -1;
        self = gameObject;
        rigid = gameObject.GetComponent<Rigidbody2D>();
        stats = gameObject.GetComponent<NPCStatistics>();
        control = GameObject.Find("TutorialControl").transform.GetChild(0).GetComponent<TutorialBeach>();
        currPatrolIndex = 0;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isPatroling)
        {
            position = self.transform.position;

            if (teleportPoints.Contains(currPatrolIndex))
            {
                self.transform.position = patrolPositions[currPatrolIndex];
            }

            if (!isMoving && !isStopped)
            {
                rigid.velocity = (patrolPositions[currPatrolIndex] - position).normalized * stats.GetMovementSpeed();
                isMoving = true;
            }
            else if (Vector3.Distance(position, patrolPositions[currPatrolIndex]) <= 1.2 && isMoving)
            {
                isStopped = true;
                isMoving = false;
            }

            if(isStopped && currPatrolIndex != breakPoint)
            {
                isStopped = false;
                currPatrolIndex++;
            }

            if (currPatrolIndex >= patrolPositions.Capacity)
            {
                rigid.velocity = Vector3.zero;
                isPatroling = false;
            }
            if (isStopped)
            {
                rigid.velocity = Vector3.zero;
            }
        }
        
    }

    public void SetBreakPoint(int breakPoint)
    {
        this.breakPoint = breakPoint;
    }

    public int GetBreakPoint()
    {
        return breakPoint;
    }

    public int GetPatrolIndex()
    {
        return currPatrolIndex;
    }

    public bool IsStopped()
    {
        return isStopped;
    }
}
