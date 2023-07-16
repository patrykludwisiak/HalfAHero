using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colliding : MonoBehaviour
{
    [SerializeField] private string collidedTag;
    private bool isColliding = false;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == collidedTag)
        {
            isColliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == collidedTag)
        {
            isColliding = false;
        }
    }

    public bool IsColliding()
    {
        return isColliding;
    }
}
