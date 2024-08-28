using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public struct PatrolPosition
{
    public Vector3 position;
    public bool isTeleportDestination;

    public PatrolPosition(Vector3 position, bool isTeleportDestination)
    {
        this.position = position;
        this.isTeleportDestination = isTeleportDestination;
    }
}
public class Patrol : MonoBehaviour
{
    public List<PatrolPosition> patrolPositions;
    //public List<Vector3> patrolPositions;
    //public List<int> teleportPoints;
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

            if (patrolPositions[currPatrolIndex].isTeleportDestination)
            {
                self.transform.position = patrolPositions[currPatrolIndex].position;
            }

            if (!isMoving && !isStopped)
            {
                rigid.velocity = (patrolPositions[currPatrolIndex].position - position).normalized * stats.GetMovementSpeed();
                isMoving = true;
            }
            else if (Vector3.Distance(position, patrolPositions[currPatrolIndex].position) <= 1.2 && isMoving)
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

[CustomEditor(typeof(Patrol))]
class PatrolEditor : Editor
{
    public void OnSceneGUI()
    {
        Patrol t = (Patrol)target;
        Vector3[] positions = new Vector3[t.patrolPositions.Count];
        Handles.color = Color.red;
        EditorGUI.BeginChangeCheck();
        for (int i = 0; i < t.patrolPositions.Count; i++)
        {
            positions[i] = Handles.PositionHandle(t.patrolPositions[i].position, Quaternion.Euler(0, 0, 0));
            if (i< t.patrolPositions.Count)
            {
                if (t.patrolPositions[i].isTeleportDestination)
                {
                    Handles.color = Color.blue;
                    if(i == 0)
                    {
                        Handles.DrawDottedLine(t.transform.position, t.patrolPositions[i].position, 5);
                    }
                    else
                    {
                        Handles.DrawDottedLine(t.patrolPositions[i-1].position, t.patrolPositions[i].position, 5);
                    }
                    Handles.color = Color.red;
                }
                else
                {
                    if (i == 0)
                    {
                        Handles.DrawLine(t.transform.position, t.patrolPositions[i].position, 2);
                    }
                    else
                    {
                        Handles.DrawLine(t.patrolPositions[i-1].position, t.patrolPositions[i].position, 2);
                    }
                }
            }
        }
        if (EditorGUI.EndChangeCheck())
        {
            List<PatrolPosition> newPatrol = new List<PatrolPosition>();
            for (int i = 0; i < t.patrolPositions.Count; i++)
            {
                Undo.RecordObject(t, "Changing Position");
                newPatrol.Add(new PatrolPosition(positions[i], t.patrolPositions[i].isTeleportDestination));
            }
            t.patrolPositions = newPatrol;
        }
    }
}
