using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAway : MonoBehaviour
{
    [SerializeField] float tillFadeAwayTime;
    float tillFadeAwayTimer;
    [SerializeField] float fadeAwayTime;
    float fadeAwayTimer;
    float initialAlpha;
    SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialAlpha = spriteRenderer.color.a;
        tillFadeAwayTimer = tillFadeAwayTime;
        fadeAwayTimer = fadeAwayTime;
        GameData.AddFootstep(this);
    }
    public void Fade()
    {
        if(tillFadeAwayTimer > 0)
        {
            tillFadeAwayTimer -= Time.deltaTime;
        }
        if (tillFadeAwayTimer <= 0)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, initialAlpha*fadeAwayTimer / fadeAwayTime);
            fadeAwayTimer -= Time.deltaTime;
        }
        if(fadeAwayTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
