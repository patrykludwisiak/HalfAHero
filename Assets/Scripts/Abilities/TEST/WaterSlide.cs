using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaterSlide : AbilityTest
{
    [SerializeField] float slideTime;
    [SerializeField] float slideSpeed;
    [SerializeField] Sprite iferSprite;
    [SerializeField] private string mainButton;
    float slideTimer;
    // Start is called before the first frame update
    void Start()
    {
        slideTimer = 0;
    }

    protected override AbilityReturn AbilityScript(WeaponTest weapon)
    {
        Vector2 dashVelocity = weapon.GetLookVector() * slideSpeed;
        if (weapon.CheckIfHold(mainButton) == Holding.hold && slideTimer < slideTime)
        {
            weapon.GetPlayerControl().PlayAnimation("PlayerSlide");
            weapon.GetPlayerRigidbody().velocity = dashVelocity;
            slideTimer += Time.fixedDeltaTime;
            return AbilityReturn.False;
        }
        weapon.GetPlayerRigidbody().velocity = Vector2.zero;
        slideTimer = 0;
        return AbilityReturn.True;
    }
}
