using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    GameObject player;
    [SerializeField] GameObject respawnPoint;

    private void Start()
    {
        player = gameObject;
    }

    public void RespawnPlayer()
    {
        player.SetActive(true);
        player.GetComponent<PlayerStatistics>().ResetHealth();
        Vector3 respawn = respawnPoint.transform.position;
        respawn.y -= 0.5f;
        player.transform.position = respawnPoint.transform.position;
        Camera.main.transform.GetChild(0).gameObject.SetActive(false);
    }
}
