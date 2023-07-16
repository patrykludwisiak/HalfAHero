using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIFunctions : MonoBehaviour
{
    public static void DrawOutline(Rect position, string text, GUIStyle style, Color outColor, Color inColor)
    {
        GUIStyle backupStyle = style;
        style.normal.textColor = outColor;
        position.x -= 2;
        GUI.Label(position, text, style);
        position.x += 4;
        GUI.Label(position, text, style);
        position.x -= 2;
        position.y -= 2;
        GUI.Label(position, text, style);
        position.y += 4;
        GUI.Label(position, text, style);
        position.y -= 2;
        style.normal.textColor = inColor;
        GUI.Label(position, text, style);
        style = backupStyle;
    }
}
