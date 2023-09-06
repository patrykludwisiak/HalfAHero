using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogComponent : MonoBehaviour
{
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
        normalDialog = false;
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

    public void ShowDialog()
    {

    }

    public void HideDialog()
    {

    }
}
