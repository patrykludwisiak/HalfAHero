using UnityEngine;

public class RuneProjectile : MonoBehaviour
{
    [SerializeField] private float lifespan;
    [SerializeField] private float speed;
    private float spawnTime;

    private void Start()
    {
        spawnTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - spawnTime > lifespan)
        {
            Destroy(gameObject);
        }
    }

    public float GetSpeed()
    {
        return speed;
    }

}
