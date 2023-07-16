using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prompt : MonoBehaviour
{
    GUIStyle style;
    GameControl gameControl;
    Rect rect;
    string text, textProperty, language;
    // Start is called before the first frame update
    void Start()
    {
        text = "";
        textProperty = "";
        gameControl = Camera.main.GetComponent<GameControl>();
        rect = new Rect(9, Screen.height/3, Screen.width, Screen.height);
        style = new GUIStyle
        {
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            fontSize = 22
        };

    }

    private void OnGUI()
    {
        if (textProperty != "")
        {
            GUIFunctions.DrawOutline(rect, text, style, Color.red, Color.yellow);
        }
    }
    

    public void SetText(string textProperty)
    {
        if(textProperty != "")
        {
            this.textProperty = textProperty;
            text = ReadLanguageFile.ReadText(textProperty, language);
        }
        else
        {
            ClearText();
        }
    }

    public void ClearText()
    {
        text = "";
        textProperty = "";
    }

    public string GetText()
    {
        return text;
    }

    public void Reload()
    {
        if (textProperty != "")
        {
            text = ReadLanguageFile.ReadText(textProperty, language);
        }
    }
}
