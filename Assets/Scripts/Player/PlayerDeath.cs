using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using TMPro;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] GameObject deathScreen;
    [SerializeField] TextMeshPro text;
    [SerializeField] LocalizedString localString;
    private void Start()
    {
        GameData.playerStats.onDeath += ShowDeathScreen;
        localString.StringChanged += Reload;
    }

    public void ShowDeathScreen()
    {
        deathScreen.SetActive(true);
        GameData.player.SetActive(false);
    }

    public void Reload(string newString)
    {
        text.text = newString;
    }
}
