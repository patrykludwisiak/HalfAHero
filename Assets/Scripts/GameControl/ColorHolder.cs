using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorHolder : MonoBehaviour
{
    [SerializeField] Color color;
    [SerializeField] string soundName;

    public Color GetColor()
    {
        return color; 
    }

    public string GetSound()
    {
        return soundName;
    }
}
