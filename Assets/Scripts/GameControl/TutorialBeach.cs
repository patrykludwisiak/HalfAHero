using UnityEngine;


public class TutorialBeach : MonoBehaviour
{
    Teleport caveInTeleport;
    Teleport caveExitTeleport;
    GameObject player;
    GameObject nivek;
    GameObject enemy1;
    GameObject enemy2;
    GameObject gauntletWall;
    Inventory playerInventory;
    Patrol nivekPatrol;
    Prompt prompt;
    DialogController dialogController;
    bool pickedUp, treeUp, dialog1, dialog2, dialog3, dialog4, dialog5, dialog6, dialog7, dialog8, prompt1, prompt2, prompt3, prompt4;

    private void Start()
    {
        dialog1 = dialog2 = dialog3 = dialog4 = dialog5 = dialog6 = dialog7 = dialog8 = prompt1 = prompt2 = prompt3 = prompt4 = false;
        caveInTeleport = GameObject.Find("CaveIn").GetComponent<Teleport>();
        caveExitTeleport = GameObject.Find("CaveExit").GetComponent<Teleport>();
        nivek = GameObject.Find("Nivek");
        player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = player.GetComponentInChildren<Inventory>();
        enemy1 = GameObject.Find("MeleeEnemyBeforeGauntlet");
        enemy2 = GameObject.Find("RangedEnemyBeforeGauntlet");
        gauntletWall = GameObject.Find("GauntletWall");
        nivekPatrol = nivek.GetComponent<Patrol>();
        dialogController = Camera.main.GetComponent<DialogController>();
        dialogController.AddDialogObject("Nivek");
        dialogController.AddDialogObject("Ifer");
        prompt = Camera.main.GetComponent<Prompt>();
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
            dialogController.AddTextToQueue("Nivek", "nivekTutorial1", 3f);
            dialogController.AddTextToQueue("Ifer", "playerTutorial1", 1f);
            dialogController.AddTextToQueue("Nivek", "nivekTutorial2", 0.8f);
            dialogController.AddTextToQueue("Nivek", "nivekTutorial3", 2f);
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
            dialogController.AddTextToQueue("Nivek", "nivekTutorial4", 1f);
            dialogController.AddTextToQueue("Nivek", "nivekTutorial5", 3f);
            dialogController.AddTextToQueue("Ifer", "playerTutorial2", 2f);
            dialogController.AddTextToQueue("Nivek", "nivekTutorial6", 2f);
            dialogController.AddTextToQueue("Ifer", "playerTutorial3", 2f);
            dialogController.AddTextToQueue("Nivek", "nivekTutorial7", 1.5f);
            dialogController.AddTextToQueue("Ifer", "playerTutorial6", 1f);

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
            dialogController.AddTextToQueue("Nivek", "nivekTutorial4", 1f);
            dialogController.AddTextToQueue("Nivek", "nivekTutorial8", 2f);
            dialogController.AddTextToQueue("Nivek", "nivekTutorial9", 3f);
            dialog3 = true;
        }

        if (!dialog4 && nivekPatrol.GetPatrolIndex() == 4 && nivekPatrol.IsStopped())
        {
            dialogController.ForceAddTextToQueue("Nivek", "nivekTutorial10", 2.5f);
            dialogController.AddTextToQueue("Ifer", "playerTutorial4", 2f);
            dialogController.AddTextToQueue("Nivek", "nivekTutorial11", 3.5f);
            dialogController.AddTextToQueue("Ifer", "playerTutorial5", 1.5f);
            dialog4 = true;
        }

        if(nivekPatrol.GetPatrolIndex() == 4 && dialogController.IsEmpty())
        {
            nivekPatrol.SetBreakPoint(6);
        }

        if(!dialog5 && treeUp && nivekPatrol.GetPatrolIndex() == 6 && nivekPatrol.IsStopped())
        {
            dialogController.AddTextToQueue("Nivek", "nivekTutorial12", 2.5f);
            dialogController.AddTextToQueue("Nivek", "nivekTutorial13", 2f);
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
            dialogController.AddTextToQueue("Nivek", "nivekTutorial14", 2.5f);
            dialog6 = true;
        }
        if (!dialog7 && nivekPatrol.GetPatrolIndex() == 8 && !enemy1 && !enemy2)
        {
            nivekPatrol.SetBreakPoint(9);
            dialogController.ForceAddTextToQueue("Nivek", "nivekTutorial15", 4f);
            dialogController.AddTextToQueue("Nivek", "nivekTutorial16", 3f);
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
            dialogController.AddTextToQueue("Nivek", "nivekTutorial17", 2f);
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
