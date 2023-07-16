using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RaiseShield : AbilityTest
{
    [SerializeField] private string mainButton;
    [SerializeField] private float staminaDecreasedPerSecond;
    protected override AbilityReturn AbilityScript(WeaponTest weapon)
    {
        if (weapon.CheckIfHold(mainButton) == Holding.hold && weapon.GetCurrentStamina() > 0)
        {
            weapon.ChangeStaminaRegening(false);
            weapon.GetPlayerControl().PlayAnimation("PlayerCharging", weapon.GetLookVector());
            weapon.DecreaseCurrentStamina(staminaDecreasedPerSecond * Time.fixedDeltaTime);
            weapon.SetPosition((Vector2)weapon.GetPlayerTransform().position + weapon.GetLookVector() * 0.3f + new Vector2(0.0f, 0.2f));
            weapon.ModifyArrow(true);
            weapon.GetArrow().transform.position = weapon.GetPlayerRigidbody().position + weapon.GetLookVector();
            weapon.EnableCollision(true);
            weapon.EnableRender(true);
        }
        else
        {
            weapon.ResetEnergy();
            weapon.ModifyArrow(false);
            weapon.EnableCollision(false);
            weapon.EnableRender(false);
            weapon.ChangeStaminaRegening(true);
            return AbilityReturn.True;
        }
        return AbilityReturn.False;
    }
}
