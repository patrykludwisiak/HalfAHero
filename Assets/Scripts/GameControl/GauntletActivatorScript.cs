using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GauntletActivatorScript : MonoBehaviour
{
    [SerializeField] private GameObject water;
    [SerializeField] private GameObject fire;
    [SerializeField] private GameObject earth;
    [SerializeField] private GameObject wind;
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject icePortal;
    [SerializeField] private GameObject firePortal;
    [SerializeField] private GameObject waterWall;

    private void Start()
    {
        fire.GetComponent<ItemHolder>().onActive += DestroyFireWall;
        water.GetComponent<ItemHolder>().onActive += DestroyWaterWall;
    }

    private void DestroyFireWall()
    {
        GameObject player = GameObject.Find("Ifer");
        player.transform.position = new Vector3(-52.4f, 50.2f, 0);
        Camera.main.transform.position = new Vector3(-52.4f, 50.2f, -10);
        GameObject rune = fire.GetComponent<ItemHolder>().RemoveItem();
        rune.transform.parent = transform.root;
        rune.transform.position = player.transform.position;
        Destroy(fire);
        Destroy(wall);
        Destroy(firePortal);
        wall = null;
    }

    private void DestroyWaterWall()
    {
        GameObject player = GameObject.Find("Ifer");
        player.transform.position = new Vector3(-65, 51, 0);
        Camera.main.transform.position = new Vector3(-65, 51, -10);
        GameObject rune = water.GetComponent<ItemHolder>().RemoveItem();
        rune.transform.parent = transform.root;
        rune.transform.position = player.transform.position;
        Destroy(water);
        Destroy(waterWall);
        Destroy(icePortal);
        waterWall = null;
    }
}
