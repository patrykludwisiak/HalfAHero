using UnityEngine;

public class BarrelDestroy : MonoBehaviour, IDestruction
{
    [SerializeField] GameObject debris;

    public void Destruct()
    {
        Instantiate(debris, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        Destroy(gameObject);
    }

}
