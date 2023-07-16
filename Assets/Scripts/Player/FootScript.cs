using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FootScript : MonoBehaviour
{

    [SerializeField] private float respawnX = -2f;
    [SerializeField] private float respawnY = 1.7f;
    
    private Rigidbody2D iferBody;
    private PlayerControl iferMovement;
    private Vector2 zeroVector;
    private CircleCollider2D FootCollider;

    private ContactFilter2D filter;

    void Start()
    {
        iferBody = GetComponentInParent<Rigidbody2D>();
        iferMovement = GetComponentInParent<PlayerControl>();
        zeroVector = new Vector2(0, 0);
        FootCollider = GetComponent<CircleCollider2D>();
        filter = new ContactFilter2D().NoFilter();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("WaterSinkable"))
        {
            if (iferBody.velocity.Equals(zeroVector) || iferMovement.Is_moving_by_walk())
            {
                List<Collider2D> list = new List<Collider2D>();
                FootCollider.OverlapCollider(filter, list);

                bool sink = true;
                foreach (Collider2D col in list)
                {
                    if (col.gameObject.name.Equals("Stone"))
                    {
                        sink = false;
                    }
                }

                if(sink)
                {
                    iferBody.transform.position = new Vector2(respawnX, respawnY);
                    FindObjectOfType<AudioManager>().Play("SinkingSound");
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Wind") || collision.gameObject.name.Equals("TempleWind"))
        {
             iferBody.velocity = zeroVector;
        }
    }

    public void setRespawn(float x, float y)
    {
        respawnX = x;
        respawnY = y;
    }

    public bool checkHazardUnderPlayer()
    {

        List<Collider2D> list = new List<Collider2D>();
        FootCollider.OverlapCollider(filter, list);

        bool sinkable = false;
        foreach (Collider2D col in list)
        {
            if (col.gameObject.tag == "TerrainHazard")
            {
                sinkable = true;
            }
        }

        return sinkable;
    }
}
