using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    Dictionary<string,Dialog> dialogObjects;
    public List<string> dialogs;
    public List<string> texts;
    public List<float> timers;
    float timer;
    bool empty, realEmpty;
    string language;
    Collider2D player;

    // Start is called before the first frame update
    void Start()
    {
        dialogObjects = new Dictionary<string, Dialog>();
        empty = true;
        realEmpty = true;
        dialogs = new List<string>();
        texts = new List<string>();
        timers = new List<float>();
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (!empty)
            {
                Dialog dialog = dialogObjects[dialogs[0]];
                dialog.SetNormalDialog(false);
                dialog.SetText("");
                dialogs.RemoveAt(0);
                texts.RemoveAt(0);
                timers.RemoveAt(0);
            }
            if (texts.Count > 0)
            {
                Dialog dialog = dialogObjects[dialogs[0]];
                timer = timers[0];
                dialog.SetNormalDialog(true);
                dialog.SetText(texts[0]);
                empty = false;
                realEmpty = false;
            }
            else if (texts.Count == 0 && empty == false)
            {
                empty = true;
                realEmpty = true;
            }
        }
    }

    public void AddDialogObject(string gameObjectName)
    {
        dialogObjects.Add(gameObjectName, GameObject.Find(gameObjectName).transform.Find("Dialog").GetComponent<Dialog>());
    }

    public void AddTextToQueue(string gameObjectName, string textProperty, float timer)
    {
        dialogs.Add(gameObjectName);
        texts.Add(textProperty);
        timers.Add(timer);
    }

    public void ForceAddTextToQueue(string gameObjectName, string textProperty, float timer)
    {
        ClearQueue();
        AddTextToQueue(gameObjectName, textProperty, timer);
    }

    public void ClearQueue()
    {
        if (dialogs.Count > 0)
        {
            Dialog dialog = dialogObjects[dialogs[0]];
            dialog.SetNormalDialog(false);
            dialog.SetText("");
        }
        dialogs.Clear();
        texts.Clear();
        timers.Clear();
        timer = 0;
        empty = true;
        realEmpty = false;
    }

    public string GetText()
    {
        return texts[0];
    }

    public string GetText(int index)
    {
        return texts[index];
    }

    public void SetTimer(float timer)
    {
        this.timer = timer;
    }

    public float GetTimer(int index)
    {
        return timers[index];
    }

    public float GetTimer()
    {
        return timer;
    }

    public bool IsEmpty()
    {
        if(realEmpty && dialogs.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public string GetDialog()
    {
        return dialogs[0];
    }

    public string GetDialog(int index)
    {
        return dialogs[index];
    }
}
