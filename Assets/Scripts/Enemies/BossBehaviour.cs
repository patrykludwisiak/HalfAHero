using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    void Update()
    {

        GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        
    }
}
