using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct DialogCharacter
{
    public string characterName;
    public DialogComponent dialogComponent;
}

public class DialogController : MonoBehaviour
{
    [SerializeField] GameObject dialogCanvas;
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] Image dialogCharacterImage;
    [SerializeField] List<DialogCharacter> dialogCharacters;
    float timer;
    float timerCap;
    List<DialogLine> dialogQueue;
    DialogComponent currentDialogComponent;

    public delegate void OnDialogAddHandler();
    public event OnDialogAddHandler OnDialogAdd;
    public delegate void OnDialogLineEndHandler();
    public event OnDialogLineEndHandler OnDialogLineEnd;
    public delegate void OnDialogEndHandler();
    public event OnDialogEndHandler OnDialogEnd;

    // Start is called before the first frame update
    void Start()
    {
        dialogQueue = new List<DialogLine>();
        timer = 0;
        timerCap = 0;
        OnDialogAdd += ShowDialogScreen;
        OnDialogLineEnd += SetNextDialog;
        OnDialogEnd += HideDialogScreen;
        LocalizationSettings.SelectedLocaleChanged += Reload;
    }

    // Update is called once per frame
    void Update()
    {
        if(dialogQueue.Count > 0)
        {
            if(timer < timerCap)
            {
                timer += Time.deltaTime;
            }
            else
            {
                OnDialogLineEnd.Invoke();
            }
        }
    }

    void ShowDialogScreen()
    {
        dialogCanvas.SetActive(true);
        SetNewDialog();
    }

    void HideDialogScreen()
    {
        dialogCanvas.SetActive(false);
        currentDialogComponent = null;
    }

    void SetNewDialog()
    {
        if (currentDialogComponent)
        {
            currentDialogComponent.HideDialog();
        }
        if (dialogQueue.Count > 0)
        {
            SetDialogValues(dialogQueue[0]);
        }
        else
        {
            timerCap = 0;
            OnDialogEnd.Invoke();
        }
    }

    void SetNextDialog()
    {
        dialogQueue.RemoveAt(0);
        timer = 0;
        SetNewDialog(); 
    }

    void SetDialogValues(DialogLine line)
    {
        timerCap = line.time;
        dialogText.text = line.text.GetLocalizedString();
        Sprite sprite = line.characterDialogSprite;
        if (sprite)
        {
            dialogCharacterImage.sprite = sprite;
        }
        int index = dialogCharacters.FindIndex(character => character.characterName.Equals(line.dialogCharacterName));
        if(index != -1)
        {
            currentDialogComponent = dialogCharacters[index].dialogComponent;
            if(currentDialogComponent)
            {
                currentDialogComponent.ShowDialog();
            }
        }
    }

    void Reload(Locale locale)
    {
        dialogText.text = dialogQueue[0].text.GetLocalizedString();
    }

    public void AddDialogToQueue(DialogObject dialogObject)
    {
        if(dialogObject.isForced)
        {
            ClearQueue();
        }
        dialogQueue.AddRange(dialogObject.dialog);
        OnDialogAdd.Invoke();
    }

    public void ClearQueue()
    {
        dialogQueue.Clear();
        timer = 0;
        timerCap = 0;
    }

    public bool IsEmpty()
    {
        return dialogQueue.Count == 0;
    }

    public DialogLine GetDialog()
    {
        return dialogQueue[0];
    }

    public DialogLine GetDialog(int index)
    {
        return dialogQueue[index];
    }

}
