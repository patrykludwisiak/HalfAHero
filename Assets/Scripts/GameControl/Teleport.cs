using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] Vector3 playerTeleportPosition;
    [SerializeField] Vector3 cameraTeleportPosition;
    int teleportTimes;

    private void Start()
    {
        teleportTimes = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.transform.position = playerTeleportPosition;
            Camera.main.transform.position = cameraTeleportPosition;
            teleportTimes++;
        }
    }

    public Vector3 GetPlayerTeleportPosition()
    {
        return playerTeleportPosition;
    }

    public Vector3 GetCameraTeleportPosition()
    {
        return cameraTeleportPosition;
    }

    public int GetTeleportTimes()
    {
        return teleportTimes;
    }
}
