using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelDebris : MonoBehaviour
{

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.AddForce(new Vector2(Random.Range(-200,200), Random.Range(-200, 200)));
        rb.AddTorque(Random.Range(-400, 400));
    }
}
