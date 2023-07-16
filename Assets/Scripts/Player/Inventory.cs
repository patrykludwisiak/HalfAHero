using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    GUIStyle style;
    GameObject weapon;
    GameObject[] items;
    GameObject weaponSlot;
    GameObject[] itemSlots;
    Rect[] itemsCount;
    List<string> weaponTags;
    Collider2D pickUpCollision;
    Transform playerTrans;
    Vector3 storePosition;
    Vector2 zero;
    [SerializeField] GameObject emptyWeapon;
    [SerializeField] int itemSpace;
    int itemsInInventory;
    [SerializeField] float stopVelocity;
    float friction = 0.90f;
    bool pickingUp;
    [SerializeField] Material highlightMaterial;
    [SerializeField] Material defaultMaterial;
    [SerializeField] float holdTime;

    float firstHoldTime;
    float secondHoldTime;
    float thirdHoldTime;
    float fourthHoldTime;

    void Start()
    {
        weaponSlot = GameObject.Find("Weapon1");
        itemSlots = new GameObject[] { GameObject.Find("Item1"), GameObject.Find("Item2"), GameObject.Find("Item3"), GameObject.Find("Item4") };
        itemsCount = new Rect[itemSpace];
        for (int i = 0; i < itemSpace; i++)
        {
            Vector2 position = Camera.main.WorldToScreenPoint(itemSlots[i].transform.GetChild(0).position);
            itemsCount[i] = new Rect(position.x + Screen.width * 0.005f, Screen.height - position.y, 10f, 10f);
        }
        weaponTags = new List<string>
        {
            "Shield","Sword"
        };
        style = new GUIStyle
        {
            fontSize = 15
        };
        storePosition = new Vector3(10000, 10000, 0);
        zero = Vector2.zero;
        pickingUp = false;
        playerTrans = gameObject.GetComponentInParent<Transform>();
        itemsInInventory = 0;
        weapon = null;
        items = new GameObject[itemSpace];
        weapon = emptyWeapon;


        firstHoldTime = 0;
        secondHoldTime = 0;
        thirdHoldTime = 0;
        fourthHoldTime = 0;
    }

    private void Update()
    {
        int hold = CheckHold();
        if (weapon == null)
        {
            weapon = emptyWeapon;
        }
        SpriteRenderer slot = weaponSlot.transform.GetChild(0).GetComponent<SpriteRenderer>();
        if (weapon != null)
        {
            slot.sprite = weapon.GetComponent<WeaponTest>().GetInventorySprite();
        }
        else
        {
            slot.sprite = null;
        }
        for (int i = 0; i < itemSpace; i++)
        {
            SpriteRenderer itemSlot = itemSlots[i].transform.GetChild(0).GetComponent<SpriteRenderer>();
            if (items[i] != null)
            {
                itemSlot.sprite = items[i].GetComponent<Item>().GetInventorySprite();
            }
            else
            {
                itemSlot.sprite = null;
            }
        }
        if (!pickingUp && (Input.GetMouseButtonDown(3) || Input.GetButtonDown(PlayerControlTest.GetButtonOfControllerType("Throw"))|| hold > 0))
        {
            ThrowItem(false, hold);
        }
        if (hold == 0)
        {
            if(Input.GetAxisRaw(PlayerControlTest.GetButtonOfControllerType("ItemTopDown")) == 1)
            {
                UseItem(0);
            }
            else if (Input.GetAxisRaw(PlayerControlTest.GetButtonOfControllerType("ItemLeftRight")) == -1)
            {
                UseItem(1);
            }
            else if (Input.GetAxisRaw(PlayerControlTest.GetButtonOfControllerType("ItemLeftRight")) == 1)
            {
                UseItem(2);
            }
            else if (Input.GetAxisRaw(PlayerControlTest.GetButtonOfControllerType("ItemTopDown")) == -1)
            {
                UseItem(3);
            }
        }

        if (pickUpCollision)
        {
            if (pickUpCollision.tag == "Items")
            {
                pickingUp = true;
                if (Input.GetMouseButtonDown(3) || Input.GetButtonDown(PlayerControlTest.GetButtonOfControllerType("Pickup")) || hold > 0)
                {
                    PickUpItem(pickUpCollision.gameObject, hold);
                    pickingUp = false;
                }
            }
            else if (pickUpCollision.tag == "ItemHolder" && pickUpCollision.GetComponent<ItemHolder>().GetItem())
            {
                pickingUp = true;
                if (Input.GetMouseButtonDown(3) || Input.GetButtonDown(PlayerControlTest.GetButtonOfControllerType("Pickup")) || hold > 0)
                {
                    PickUpItem(pickUpCollision.gameObject.GetComponent<ItemHolder>().RemoveItem(), hold);
                    pickingUp = false;
                }
            }
            else if (weaponTags.IndexOf(pickUpCollision.tag) != -1)
            {
                pickingUp = true;
                if (Input.GetMouseButtonDown(3) || Input.GetButtonDown(PlayerControlTest.GetButtonOfControllerType("Pickup")) || hold > 0)
                {
                    PickUpWeapon(pickUpCollision.gameObject);
                    pickingUp = false;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Items" || collision.tag == "ItemHolder" || (collision.gameObject != weapon && weaponTags.IndexOf(collision.tag) != -1))
        {
            pickUpCollision = collision;

            pickUpCollision.gameObject.GetComponent<SpriteRenderer>().material = highlightMaterial;
            pickUpCollision.gameObject.transform.Find("PickUpIcon").GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Items" || collision.tag == "ItemHolder" || weaponTags.IndexOf(collision.tag) != -1)
        {
            pickUpCollision = null;
            pickingUp = false;
            collision.gameObject.GetComponent<SpriteRenderer>().material = defaultMaterial;
            collision.gameObject.transform.Find("PickUpIcon").GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void OnGUI()
    {
        for (int i = 0; i < itemSpace; i++)
        {
            if (items[i] != null)
            {
                Item item = items[i].GetComponent<Item>();
                int amount = item.GetAmount();
                if (amount > 0)
                {
                    GUI.Label(itemsCount[i], item.GetAmount().ToString(), style);
                }
            }
        }
    }

    private int CheckHold()
    {
        bool anyHold = false;
        if(Input.GetAxisRaw(PlayerControlTest.GetButtonOfControllerType("ItemTopDown")) == 1)
        {
            firstHoldTime += Time.deltaTime;
            anyHold = true;
        }
        else
        {
            firstHoldTime = 0;
        }
        if (Input.GetAxisRaw(PlayerControlTest.GetButtonOfControllerType("ItemLeftRight")) == -1)
        {
            secondHoldTime += Time.deltaTime;
            anyHold = true;
        }
        else
        {
            secondHoldTime = 0;
        }
        if (Input.GetAxisRaw(PlayerControlTest.GetButtonOfControllerType("ItemLeftRight")) == 1)
        {
            thirdHoldTime += Time.deltaTime;
            anyHold = true;
        }
        else
        {
            thirdHoldTime = 0;
        }
        if (Input.GetAxisRaw(PlayerControlTest.GetButtonOfControllerType("ItemTopDown")) == -1)
        {
            fourthHoldTime += Time.deltaTime;
            anyHold = true;
        }
        else
        {
            fourthHoldTime = 0;
        }

        if(!anyHold)
        {
            return -1;
        }
        if(firstHoldTime >= holdTime)
        {
            firstHoldTime = 0;
            return 1;
        }

        if (secondHoldTime >= holdTime)
        {
            secondHoldTime = 0;
            return 2;
        }

        if (thirdHoldTime >= holdTime)
        {
            thirdHoldTime = 0;
            return 3;
        }

        if (fourthHoldTime >= holdTime)
        {
            fourthHoldTime = 0;
            return 4;
        }
        return 0;
    }

    private void PickUpItem(GameObject obj, int holdButton)
    {
        bool found = false;
        obj.transform.Find("PickUpIcon").GetComponent<SpriteRenderer>().enabled = false;
        obj.GetComponent<SpriteRenderer>().material = defaultMaterial;
        Item pickedItem = obj.GetComponent<Item>();
        if (pickedItem.GetMaxAmount() > 0)
        {
            foreach (GameObject item in items)
            {
                if (item)
                {
                    if (item.name == obj.name)
                    {
                        found = true;
                        Item itemStat = item.GetComponent<Item>();
                        int objectAmount = pickedItem.GetAmount();
                        if (itemStat.GetAmount() + objectAmount < itemStat.GetMaxAmount())
                        {
                            itemStat.IncreaseAmount(pickedItem.GetAmount());
                            Destroy(obj);
                        }
                        else
                        {
                            int amount = itemStat.GetMaxAmount() - itemStat.GetAmount();
                            pickedItem.DecreaseAmount(amount);
                            itemStat.IncreaseAmount(amount);
                            if(holdButton > 0)
                            {
                                ThrowItem(true, holdButton);
                                items[holdButton - 1] = obj; 
                            }
                        }
                    }
                }
            }
        }
        if (!found)
        {
            if (itemsInInventory < itemSpace && holdButton <= 0)
            {
                int i = 0;
                bool picked = false;
                while (i < itemSpace && !picked)
                {
                    if (items[i] == null)
                    {
                        items[i] = obj;
                        items[i].transform.position = storePosition;
                        itemsInInventory++;
                        picked = true;
                    }
                    i++;
                }
            }
            else
            {
                if(holdButton > 0)
                {
                    if(items[holdButton - 1] != null)
                    {
                        ThrowItem(true, holdButton);
                    }
                    items[holdButton - 1] = obj;
                    items[holdButton - 1].transform.position = storePosition;
                }
                else
                {
                    if (items[0] != null)
                    {
                        ThrowItem(true, 1);
                    }
                    items[0] = obj;
                    items[0].transform.position = storePosition;
                }
            }
        }
    }

    private void PickUpWeapon(GameObject obj)
    {
        obj.transform.Find("PickUpIcon").GetComponent<SpriteRenderer>().enabled = false;
        obj.GetComponent<SpriteRenderer>().material = defaultMaterial;

        ThrowWeapon(true);
        obj.transform.parent = gameObject.transform;
        weapon = obj;
        weapon.transform.position = playerTrans.position;
        weapon.name = obj.name;
        weapon.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
        weapon.GetComponent<SpriteRenderer>().enabled = false;
        weapon.GetComponent<Collider2D>().enabled = false;
    }

    private void ThrowWeapon(bool isPickingUp)
    {
        if (weapon != null && weapon != emptyWeapon)
        {
            GameObject thrownItem = weapon;
            weapon = null;
            thrownItem.transform.parent = null;
            thrownItem.GetComponent<SpriteRenderer>().enabled = true;
            thrownItem.GetComponent<Collider2D>().enabled = true;
            thrownItem.transform.position = playerTrans.position;
            Rigidbody2D thrownRigid = thrownItem.GetComponent<Rigidbody2D>();
            if (thrownRigid.velocity != zero)
            {
                thrownRigid.velocity *= friction;

                if (thrownRigid.velocity.magnitude < stopVelocity)
                {
                    thrownRigid.velocity = zero;
                }
            }
            print(thrownItem.GetComponent<WeaponTest>().GetInventorySprite());
            thrownItem.GetComponent<SpriteRenderer>().sprite = thrownItem.GetComponent<WeaponTest>().GetInventorySprite();
        }
    }

    private void ThrowItem(bool isPickingUp, int holdButton)
    {
        
        GameObject thrownItem = null;
        if (holdButton > 0)
        {
            if(items[holdButton - 1])
            {
                thrownItem = items[holdButton - 1];
                items[holdButton - 1] = null;
            }
        }
        else
        {
            for(int i = 0; i < itemSpace; i++)
            {
                if (items[i] != null)
                {
                    thrownItem = items[i];
                    items[i] = null;
                    break;
                }
            }
        }
        if(thrownItem)
        {
            thrownItem.GetComponent<Item>().OnDrop();
            thrownItem.transform.parent = null;
            thrownItem.transform.position = playerTrans.position;
            Rigidbody2D thrownRigid = thrownItem.GetComponent<Rigidbody2D>();
            if (thrownRigid.velocity != zero)
            {
                thrownRigid.velocity *= friction;

                if (thrownRigid.velocity.magnitude < stopVelocity)
                {
                    thrownRigid.velocity = zero;
                }
            }
            if (!isPickingUp)
            {
                itemsInInventory--;
            }
        }
    }

    public void UseItem(int itemUsed)
    {
        if (items[itemUsed] != null)
        {
            Item item;
            bool remove;
            item = items[itemUsed].GetComponent<Item>();
            remove = item.UseItem(gameObject);
            if (remove)
            {
                items[itemUsed] = null;
                itemsInInventory--;
            }
            else
            {
                if (item.GetMaxAmount() > 0)
                {
                    item.DecreaseAmount();
                    if (item.GetAmount() == 0)
                    {
                        Destroy(items[itemUsed]);
                        itemsInInventory--;
                    }
                }
            }
        }
    }

    public GameObject GetItem(int index)
    {
        return items[index];
    }

    public GameObject GetCurrentWeapon()
    {
        return weapon;
    }

    public bool IsPickingUp()
    {
        return pickingUp;
    }
}
