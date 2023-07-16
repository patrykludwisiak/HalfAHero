using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnd : MonoBehaviour
{
    GameObject nivek;
    GameObject endScreen;
    Teleport templeOutTeleport;
    Patrol nivekPatrol;
    DialogController dialogController;
    Rect informationPosition;
    GUIStyle style;
    bool gotToEnd, dialog1, dialog2;
    string endInfo, language;

    void Start()
    {
        endScreen = GameObject.Find("EndScreen");
        nivek = GameObject.Find("Nivek");
        nivekPatrol = nivek.GetComponent<Patrol>();
        dialogController = Camera.main.GetComponent<DialogController>();
        templeOutTeleport = GameObject.Find("TempleOut").GetComponent<Teleport>();
        gotToEnd = dialog1 = dialog2 = false;
        informationPosition = new Rect(0, 0, Screen.width, Screen.height);
        style = new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 40
        };
        endInfo = ReadLanguageFile.ReadText("endInfo", language);
    }

    private void Update()
    {
        if (!dialog1 && nivekPatrol.GetPatrolIndex() == 13 && templeOutTeleport.GetTeleportTimes() > 0)
        {
            nivekPatrol.SetBreakPoint(15);
            dialogController.AddTextToQueue("Nivek", "nivekTutorial22", 3f);
            dialogController.AddTextToQueue("Nivek", "nivekTutorial23", 3f);
            dialog1 = true;
        }
        if (!dialog2 && gotToEnd)
        {
            dialogController.ForceAddTextToQueue("Ifer", "playerTutorial7", 1f);
            dialogController.AddTextToQueue("Nivek", "nivekTutorial24", 2f);
            dialog2 = true;
        }
        if(gotToEnd && dialogController.IsEmpty())
        {
            GameData.SetEnd(true);
            endScreen.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private void OnGUI()
    {
        if (GameData.IsEnd())
        {
            GUIFunctions.DrawOutline(informationPosition, endInfo, style, Color.black, Color.white);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            gotToEnd = true;
        }
    }

    public void Reload()
    {
        endInfo = ReadLanguageFile.ReadText("endInfo", language);
    }
}
