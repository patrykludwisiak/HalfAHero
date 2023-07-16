using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehaviour : MonoBehaviour
{
    [SerializeField] AttackTypes type;
    public void Destroy(AttackTypes type)
    {
        if(type == this.type)
        {
            Destroy(gameObject);
        }
    }
}
