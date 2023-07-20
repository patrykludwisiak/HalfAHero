using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class GameControl : MonoBehaviour
{
    public static GameControl gameControl;

    private void Start()
    {
        gameControl = this;
    }

    void Update()
    {
        foreach (FadeAway footStep in GameData.footSteps)
        {
            if (footStep != null)
            { 
                footStep.Fade();
            }
            else
            {
                GameData.footStepsToRemove.Add(footStep);
            }
        }
        foreach(FadeAway footStep in GameData.footStepsToRemove)
        {
            GameData.footSteps.Remove(footStep);
        }
        GameData.footStepsToRemove.Clear();
    }

    public static void ExitGame()
    {
        Application.Quit();
    }

    public void Stop()
    {
        if (GameData.hitStopped)
            return;

        Time.timeScale = 0.0f;
        StartCoroutine(Wait(GameData.hitStopTime));
    }
    IEnumerator Wait(float duration)
    {
        GameData.hitStopped = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
        GameData.hitStopped = false;
    }

    public void ChangeLanguage(string locale)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(locale);
    }
}
