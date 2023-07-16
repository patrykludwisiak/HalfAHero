using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceWall : MonoBehaviour
{
    Vector2 normal;

    // Start is called before the first frame update
    void Start()
    {
        float angle = transform.rotation.eulerAngles.z - 90.0f;
        normal = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Vector2 GetNormal()
    {
        return normal;
    }
}
