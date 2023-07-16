using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField] GameObject droppedItem;

    public void DropItem(Vector2 position)
    {
        GameObject obj = Instantiate(droppedItem);
        obj.name = droppedItem.name;
        obj.transform.position = new Vector3(position.x,position.y,1);
    }
}
