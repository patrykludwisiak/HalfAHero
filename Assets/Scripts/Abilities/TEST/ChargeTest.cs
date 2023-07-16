using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChargeTest : AbilityTest
{
    private float charge;
    private float maxChargeUsed;
    private float minPerfectChargeUsed;
    private float maxPerfectChargeUsed;
    private bool breakCharge;
    private Vector2 normalizedChargeVector;
    [SerializeField] private string mainButton;
    [SerializeField] private string breakButton;
    [SerializeField] private bool showWeapon;
    [SerializeField] private float chargingValue;
    [SerializeField] private float maxCharge;
    [SerializeField] private float chargeMultiplier; //additional multiplier for charge length
    [SerializeField] private float minPerfectCharge;
    [SerializeField] private float maxPerfectCharge;
    [SerializeField] private GameObject perfectChargeWindup;
    // Start is called before the first frame update
    void Start()
    {
        charge = 0;
    }

    // Update is called once per frame
    protected override AbilityReturn AbilityScript(WeaponTest weapon)
    {
        RemoveBreak(weapon);
        SetBreak(weapon);
        if (!breakCharge)
        {
            if (weapon.CheckIfHold(mainButton) == Holding.none)
            {
                if(showWeapon)
                {
                    weapon.EnableCollision(false);
                    weapon.EnableRender(false);
                }
                weapon.SetChargeVector(charge * CountMultiplier(StatusEffect.GetListOfEffectsValues<Slow>(weapon.GetStatusEffects())) * normalizedChargeVector);
                weapon.GetArrow().GetComponent<SpriteRenderer>().color = Color.black;
                if(DecideIfPerfect())
                {
                    weapon.IncreaseEnergy();
                }
                else
                {
                    weapon.ResetEnergy();
                }
                weapon.ModifyArrow(false);
                charge = 0;
            }
        }
        if (!breakCharge)
        {
            if (charge > 0)
            {
                weapon.GetArrow().GetComponent<SpriteRenderer>().color = Color.black;
                weapon.SetPosition((Vector2)weapon.GetPlayerTransform().position + (Vector2)Vector3.Normalize(weapon.GetChargeVector()) * 0.3f + new Vector2(0.0f, 0.2f));
            }
            normalizedChargeVector = Vector3.Normalize(weapon.GetLookVector());
            if (weapon.CheckIfHold(mainButton) == Holding.hold)
            {
                if (charge == 0)
                {
                    weapon.ModifyArrow(true);
                    maxChargeUsed = maxCharge;
                    minPerfectChargeUsed = minPerfectCharge;
                    maxPerfectChargeUsed = maxPerfectCharge;
                    GameObject windUp = Instantiate(perfectChargeWindup, transform);
                    windUp.transform.eulerAngles = Vector3.zero;
                    if (showWeapon)
                    {
                        weapon.EnableCollision(true);
                        weapon.EnableRender(true);
                    }
                }
                IncreaseCharge(chargingValue);
                weapon.SetChargeVector( charge * CountMultiplier(StatusEffect.GetListOfEffectsValues<Slow>(weapon.GetStatusEffects())) * normalizedChargeVector);
                weapon.GetPlayerControl().PlayAnimation("PlayerCharging", Vector3.Normalize(weapon.GetChargeVector()));
                weapon.GetArrow().transform.position = weapon.GetPlayerRigidbody().position + weapon.GetChargeVector();
                if (DecideIfPerfect())
                {
                    weapon.GetArrow().GetComponent<SpriteRenderer>().color = Color.white;
                }
                return AbilityReturn.False;
            }
        }
        else
        {
            if (showWeapon)
            {
                weapon.EnableCollision(false);
                weapon.EnableRender(false);
            }
            weapon.SetChargeVector(Vector2.zero);
            weapon.GetArrow().GetComponent<SpriteRenderer>().color = Color.black;
            charge = 0;
            weapon.ModifyArrow(false);
        }
        return AbilityReturn.True;
    }

    private void IncreaseCharge(float charge)
    {
        
        this.charge += charge;
        if (this.charge > maxChargeUsed)
        {
            this.charge = maxChargeUsed;
        }
    }

    private bool DecideIfPerfect()
    {
        if (charge >= minPerfectChargeUsed && charge <= maxPerfectChargeUsed)
        {
            return true;
        }
        return false;
    }

    private float CountMultiplier(List<float> chargeMultipliers)
    {
        float multiplier = chargeMultiplier;
        foreach (float multi in chargeMultipliers)
        {
            multiplier *= multi;
        }
        return multiplier;
    }

    private void SetBreak(WeaponTest weapon)
    {
        Holding hold = weapon.CheckIfHold(breakButton);
        if (hold == Holding.click || hold == Holding.hold)
        {
            breakCharge = true;
        }
    }
    private void RemoveBreak(WeaponTest weapon)
    {
        if (weapon.CheckIfHold(mainButton) == Holding.none && weapon.CheckIfHold(breakButton) == Holding.none)
        {
            breakCharge = false;
        }
    }
}
