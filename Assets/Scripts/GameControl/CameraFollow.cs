using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //[SerializeField] private Transform staminaBar;
    [SerializeField] private Transform healthBar;
    [SerializeField] private float barOffset;

    [Space]

    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [Range(1, 10)]
    [SerializeField] private float smoothFactor;

    private void FixedUpdate()
    {
        if (target)
        {
            //following player
            Vector3 targetPosition = target.position + offset;
            Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.fixedDeltaTime);
            transform.position = smoothPosition;

            //stamina bar location
            //staminaBar.position = new Vector3(transform.position.x, transform.position.y + barOffset, 0);

            //health bar location
            healthBar.position = new Vector3(transform.position.x - 5, transform.position.y + barOffset, 0);
        }
    }
}
