using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] GameObject deathScreen;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] LocalizedString localString;
    private void Start()
    {
        GameData.playerStats.OnDeath += ShowDeathScreen;
        LocalizationSettings.SelectedLocaleChanged += Reload;
    }

    public void ShowDeathScreen()
    {
        deathScreen.SetActive(true);
        GameData.player.SetActive(false);
    }

    public void Reload(Locale locale)
    {
        text.text = localString.GetLocalizedString();
    }
}
