using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfectChargeBlastCtrl : MonoBehaviour
{
    [SerializeField] float lifespan = 0.25f;

    // Update is called once per frame
    void Update()
    {

        lifespan -= Time.deltaTime;

        if (lifespan < 0)
        {
            Destroy(gameObject);
        }
    }
}
