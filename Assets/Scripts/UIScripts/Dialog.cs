using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    Inventory playerInventory;
    GameControl gameControl;
    GUIStyle style;
    Rect rect;
    string text;
    string textProperty;
    bool normalDialog;
    [SerializeField] bool npc;
    string language;
    Collider2D player;

    // Start is called before the first frame update
    void Start()
    {
        text = "";
        textProperty = "";
        gameControl = Camera.main.GetComponent<GameControl>();
        playerInventory = GameObject.Find("Ifer").GetComponentInChildren<Inventory>();
        normalDialog = false;
        style = new GUIStyle
        {
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.LowerCenter,
            fontSize = 20,
            wordWrap = true
        };
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = Camera.main.WorldToScreenPoint(transform.position);
        rect = new Rect(position.x - 125f, Screen.height - position.y - 200f, 250f, 100f);
        /*if(player && !normalDialog)
        {
            GameObject item = playerInventory.GetCurrentItem();
            if (item)
            {
                string description = item.GetComponent<Item>().GetDescription();
                if (text != description)
                {
                    text = description;
                }
            }
        }
        else*/ if (!normalDialog)
        {
            text = "";
        }
    }

    private void OnGUI()
    {
        if (text != "")
        {
            GUIFunctions.DrawOutline(rect, text, style, Color.black, Color.white);
        }
    }

    public void SetText(string text)
    {
        if(text != "")
        {
            textProperty = text;
            this.text = ReadLanguageFile.ReadText(text, language);
        }
        else
        {
            this.text = "";
            textProperty = "";
        }
    }
    
    public string GetText()
    {
        return text;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(npc && collision.tag == "Player" && !normalDialog)
        {
            player = collision;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (npc && collision.tag == "Player")
        {
            player = null;
        }
    }

    public void SetNormalDialog(bool isNormalDialog)
    {
        normalDialog = isNormalDialog;
    }

    public void Reload()
    {
        if (textProperty != "")
        {
            text = ReadLanguageFile.ReadText(textProperty, language);
        }
    }
}
