using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[System.Serializable]

public class TutorialBeach : MonoBehaviour
{
    [SerializeField] Teleport caveInTeleport;
    [SerializeField] Teleport caveExitTeleport;
    [SerializeField] GameObject nivek;
    [SerializeField] GameObject enemy1;
    [SerializeField] GameObject enemy2;
    [SerializeField] GameObject gauntletWall;
    [SerializeField] DialogController dialogController;
    [SerializeField] Patrol nivekPatrol;
    [SerializeField] Prompt prompt;
    [SerializeField] DialogObject startDialog;
    [SerializeField] DialogObject stoneJumpingDialog;
    [SerializeField] DialogObject shieldDialog;
    [SerializeField] DialogObject windDialog;
    [SerializeField] DialogObject treeDialog;
    [SerializeField] DialogObject seeGhostsDialog;
    [SerializeField] DialogObject killedGhostsDialog;
    [SerializeField] DialogObject wallDialog;
    Inventory playerInventory;
    bool pickedUp, treeUp, dialog1, dialog2, dialog3, dialog4, dialog5, dialog6, dialog7, dialog8, prompt1, prompt2, prompt3, prompt4;

    private void Start()
    {
        dialog1 = dialog2 = dialog3 = dialog4 = dialog5 = dialog6 = dialog7 = dialog8 = prompt1 = prompt2 = prompt3 = prompt4 = false;
        playerInventory = GameData.inventory;
        pickedUp = false;
        treeUp = true;
    }

    private void Update()
    {

        if (treeUp && !GameObject.Find("BreakableTree"))
        {
            treeUp = false;
        }
        
        if (!dialog1)
        {
            nivekPatrol.SetBreakPoint(1);
            dialogController.AddDialogToQueue(startDialog);
            dialog1 = true;
        }

        if(dialog1 && !prompt1 && !Input.GetMouseButtonUp(1))
        {
            prompt.SetText("tutorialBMP");
        }
        else
        {
            if (!prompt1)
            {
                prompt.ClearText();
                prompt1 = true;
            }
        }

        if (!dialog2 && nivekPatrol.GetPatrolIndex() == 1 && nivekPatrol.IsStopped() && dialogController.IsEmpty())
        {
            nivekPatrol.SetBreakPoint(2);
            dialogController.AddDialogToQueue(stoneJumpingDialog);

            dialog2 = true;
        }

        if(dialog2 && !prompt2 && playerInventory.IsPickingUp())
        {
            prompt.SetText("tutorialPickingUp");
        }
        else
        {
            if(!prompt2 && playerInventory.GetCurrentWeapon().CompareTag("Shield"))
            {
                prompt.ClearText();
                prompt2 = true;
            }
        }

        if (!dialog3 && nivekPatrol.GetPatrolIndex() == 2 && pickedUp)
        {
            nivekPatrol.SetBreakPoint(4);
            dialogController.AddDialogToQueue(shieldDialog);
            dialog3 = true;
        }

        if (!dialog4 && nivekPatrol.GetPatrolIndex() == 4 && nivekPatrol.IsStopped())
        {
            dialogController.AddDialogToQueue(windDialog);
            dialog4 = true;
        }

        if(nivekPatrol.GetPatrolIndex() == 4 && dialogController.IsEmpty())
        {
            nivekPatrol.SetBreakPoint(6);
        }

        if(!dialog5 && treeUp && nivekPatrol.GetPatrolIndex() == 6 && nivekPatrol.IsStopped())
        {
            dialogController.AddDialogToQueue(treeDialog);
            dialog5 = true;
        }
        
        if(dialog5 && !prompt3 && treeUp && dialogController.IsEmpty())
        {
            prompt.SetText("tutorialLMP");
        }
        else
        {
            if (!prompt3 && !treeUp)
            {
                prompt.ClearText();
                prompt3 = true;
            }
        }

        if(!treeUp && nivekPatrol.GetPatrolIndex() == 6)
        {
            dialogController.ClearQueue();
            nivekPatrol.SetBreakPoint(8);
        }
        if(!dialog6 && nivekPatrol.GetPatrolIndex() == 8 && caveInTeleport.GetTeleportTimes() > 0)
        {
            dialogController.AddDialogToQueue(seeGhostsDialog);
            dialog6 = true;
        }
        if (!dialog7 && nivekPatrol.GetPatrolIndex() == 8 && !enemy1 && !enemy2)
        {
            nivekPatrol.SetBreakPoint(9);
            dialogController.AddDialogToQueue(killedGhostsDialog);
            dialog7 = true;
        }

        if (dialog7 && !prompt4 && playerInventory.GetItem(0) && !Input.GetKeyDown("r") && !Input.GetMouseButtonDown(3) && !Input.GetMouseButtonDown(2) && dialogController.IsEmpty())
        {
            prompt.SetText("tutorialUsingItems");
        }
        else
        {
            if (!prompt4 && prompt3 && prompt2 && prompt1 && (Input.GetKeyDown("r") || Input.GetMouseButtonDown(3) || Input.GetMouseButtonDown(2)))
            {
                prompt.ClearText();
                prompt4 = true;
            }
        }

        if (!dialog8 && nivekPatrol.GetPatrolIndex() == 9 && !gauntletWall)
        {
            nivekPatrol.SetBreakPoint(10);
            dialogController.AddDialogToQueue(wallDialog);
            dialog8 = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameObject.Find("Shield"))
        {
            caveInTeleport.enabled = true;
            caveExitTeleport.enabled = true;
        }
        else
        {
            caveInTeleport.enabled = false;
            caveExitTeleport.enabled = false;
        }
        if (collision.tag == "Player" && (playerInventory.GetCurrentWeapon() == GameObject.Find("Shield")))
        {
            pickedUp = true;
        }
    }
}
