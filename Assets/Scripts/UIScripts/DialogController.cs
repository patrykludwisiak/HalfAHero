using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using TMPro;

public class DialogController : MonoBehaviour
{
    [SerializeField] GameObject dialogCanvas;
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] Image dialogCharacterImage;
    float timer;
    float timerCap;
    List<DialogLine> dialogQueue;

    public delegate void OnDialogAdd();
    public OnDialogAdd onDialogAdd;
    public delegate void OnDialogLineEnd();
    public OnDialogLineEnd onDialogLineEnd;
    public delegate void OnDialogEnd();
    public OnDialogEnd onDialogEnd;

    // Start is called before the first frame update
    void Start()
    {
        dialogQueue = new List<DialogLine>();
        timer = 0;
        timerCap = 0;
        onDialogAdd += ShowDialogScreen;
        onDialogLineEnd += SetNewDialog;
        onDialogEnd += HideDialogScreen;
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
                onDialogLineEnd.Invoke();
            }
        }
    }

    void ShowDialogScreen()
    {
        dialogCanvas.SetActive(true);
    }

    void HideDialogScreen()
    {
        dialogCanvas.SetActive(false);
    }

    void SetNewDialog()
    {
        dialogQueue.RemoveAt(0);
        timer = 0;
        if (dialogQueue.Count > 0)
        {
            SetDialogValues(dialogQueue[0]);
        }
        else
        {
            timerCap = 0;
            onDialogEnd.Invoke();
        }
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
        onDialogAdd.Invoke();
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
