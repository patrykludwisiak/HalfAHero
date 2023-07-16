using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCStatistics : Statistics
{
    [SerializeField] private float movementSpeed;
    
    public float GetMovementSpeed()
    {
        return movementSpeed;
    }

}
