using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : MonoBehaviour
{
    [SerializeField] Sprite inventorySprite;
    [SerializeField] string descriptionProperty;
    //if maxAmount is lower than 0 it can be used infinitely
    [SerializeField] int maxAmount;
    GameControl gameControl;
    protected PlayerControlTest playerControlTest;
    string description;
    int amount;

    protected virtual void Start()
    {
        if (maxAmount > 0)
        {
            amount = 1;
        }
        else
        {
            amount = maxAmount;
        }
        gameControl = Camera.main.GetComponent<GameControl>();
        playerControlTest = FindObjectOfType<PlayerControlTest>();
    }
    public void Reload()
    {
        description = "";
    }

    public virtual bool UseItem(GameObject inventory)
    {
        return false;
    }

    public virtual void OnDrop()
    {
    }

    public Sprite GetInventorySprite()
    {
        return inventorySprite;
    }
    
    public void IncreaseAmount()
    {
        amount++;
    }

    public void IncreaseAmount(int amount)
    {
        this.amount += amount;
    }
    public void DecreaseAmount()
    {
        amount--;
    }

    public void DecreaseAmount(int amount)
    {
        this.amount -= amount;
    }
    public int GetAmount()
    {
        return amount;
    }
    public int GetMaxAmount()
    {
        return maxAmount;
    }
    public string GetDescription()
    {
        return description;
    }
}
