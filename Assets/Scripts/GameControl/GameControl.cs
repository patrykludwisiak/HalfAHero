using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
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

}
