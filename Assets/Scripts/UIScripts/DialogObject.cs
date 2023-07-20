using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[System.Serializable]
public struct DialogLine
{
    public Sprite characterDialogSprite;
    public LocalizedString text;
    public float time;
}

[CreateAssetMenu(fileName = "DialogObject", menuName = "DialogObject")]
public class DialogObject : ScriptableObject
{
    public List<DialogLine> dialog;
    public bool isForced;
}
