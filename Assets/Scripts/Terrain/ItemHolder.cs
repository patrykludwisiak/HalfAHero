using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] Sprite iconWithoutItem;
    [SerializeField] Sprite iconWithItem;
    Transform pickUpIcon;
    private bool active;
    private bool destroy;
    Inventory playerInventory;
    GameObject rune;
    GameObject runeSlot;
    SpriteRenderer pickUpIconSP;

    public delegate void OnActive();
    public OnActive onActive;

    public delegate void OnDeactive();
    public OnActive onDeactive;

    private void Start()
    {
        active = false;
        destroy = false;
        rune = null;
        playerInventory = GameObject.Find("Ifer").transform.Find("Inventory").GetComponent<Inventory>();
        pickUpIcon = transform.Find("PickUpIcon");
        pickUpIconSP = pickUpIcon.GetComponent<SpriteRenderer>();
        runeSlot = gameObject.transform.Find("Slot").gameObject;
    }

    private void Update()
    {
        if (destroy && rune)
        {
            RemoveItem();
        }
        if (rune)
        {
            pickUpIcon.localScale = new Vector3(1.5f, 1.5f);
            pickUpIconSP.sprite = iconWithItem;
        }
        else
        {
            bool found = false;
            for(int i = 0; i < 4; i++)
            {
                GameObject item = playerInventory.GetItem(i);
                if (item && item.name.Contains("Rune"))
                {
                    pickUpIcon.localScale = new Vector3(2.5f, 2.5f);
                    pickUpIconSP.sprite = iconWithoutItem;
                    found = true;
                    break;
                }
            }
            if(!found)
            {
                pickUpIconSP.sprite = null;
            }
        }
    }

    public bool IsActive()
    {
        return active;
    }

    public void SetDestroyRunes(bool a)
    {
        destroy = a;
    }

    public void AddItem(GameObject gameObject)
    {
        runeSlot.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        rune = gameObject;
        if(rune.name.Contains(itemName))
        {
            Activate();
        }
    }

    public GameObject RemoveItem()
    {
        GameObject gameObject = rune.gameObject;
        runeSlot.GetComponent<SpriteRenderer>().sprite = null;
        rune = null;
        Deactivate();
        return gameObject;
    }

    public GameObject GetItem()
    {
        return rune;
    }

    private void Activate()
    {
        active = true;
        onActive.Invoke();
    }

    private void Deactivate()
    {
        active = false;
        onDeactive.Invoke();
    }
}
