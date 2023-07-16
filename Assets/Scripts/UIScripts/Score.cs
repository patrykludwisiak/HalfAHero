using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] GameObject player;
    GameControl gameControl;
    Rect comboPosition;
    GUIStyle style;
    string language, comboInfo;

    void Start()
    {
        gameControl = Camera.main.GetComponent<GameControl>();
        comboPosition = new Rect(Screen.width / 2 + 300, 20, 100, 50);
        style = new GUIStyle
        {
            fontSize = 40
        };
        comboInfo = ReadLanguageFile.ReadText("comboInfo", language);
    }

    public void Reload()
    {
        comboInfo = ReadLanguageFile.ReadText("comboInfo", language);
    }
}
