using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticles : MonoBehaviour
{
    [SerializeField] private GameObject BloodPS;
    public void SpawnBloodParticles(Vector2 position)
    {
        Instantiate(BloodPS, position, Quaternion.identity);
    }
}
