using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Charge : Ability
{
    private float charge;
    private float maxChargeUsed;
    private float minPerfectChargeUsed;
    private float maxPerfectChargeUsed;
    private bool perfect;
    public bool isCharged;
    public bool breakCharge;
    private Vector2 normalizedChargeVector;
    private PlayerControl control;
    private Transform arrowTransform;
    private SpriteRenderer arrowRenderer;
    [SerializeField] private float chargingValue;
    [SerializeField] private float maxCharge;
    [SerializeField] private float chargeMultiplier; //additional multiplier for charge length
    [SerializeField] private float minPerfectCharge;
    [SerializeField] private float maxPerfectCharge;
    [SerializeField] private bool distanceIncreasedByCombo;
    [SerializeField] private float comboMultiplier;
    [SerializeField] private GameObject perfectChargeWindup;

    private void Awake()
    {
        charge = 0;
        perfect = false;
        control = GameObject.Find("Ifer").GetComponent<PlayerControl>();
    }

    public bool Cast(float chargingValue, float maxCharge, float minPerfect, float maxPerfect, bool chargingCondition,
        bool chargeEndCondition, bool breakChargeCondition, List<float> chargeMultipliers, Vector2 position, ref Vector2 chargeVector, GameObject arrow)
    {
        if (!breakCharge)
        {
            if (chargeEndCondition && charge != 0)
            {
                chargeVector = normalizedChargeVector * charge * CountMultiplier(chargeMultipliers);
                arrow.GetComponent<SpriteRenderer>().color = Color.black;
                isCharged = true;
                perfect = DecideIfPerfect();
                charge = 0;
            }
        }
        SetBreak(chargingCondition, breakChargeCondition);
        if (!breakCharge) {
            if(charge > 0) {
                arrow.GetComponent<SpriteRenderer>().color = Color.black;
            }
            normalizedChargeVector = Vector3.Normalize(chargeVector);
            if (chargingCondition)
            {
                if (charge == 0)
                {
                    maxChargeUsed = maxCharge;
                    minPerfectChargeUsed = minPerfect;
                    maxPerfectChargeUsed = maxPerfect;
                    if (distanceIncreasedByCombo)
                    {
                        maxChargeUsed *= control.GetMovementCombo() * comboMultiplier + 1;
                        minPerfectChargeUsed *= control.GetMovementCombo() * comboMultiplier + 1;
                        maxPerfectChargeUsed *= control.GetMovementCombo() * comboMultiplier + 1;
                    }
                    Instantiate(perfectChargeWindup, transform);
                }
                isCharged = false;
                IncreaseCharge(chargingValue);
                chargeVector = charge * CountMultiplier(chargeMultipliers) * normalizedChargeVector;
                arrow.transform.position = position + chargeVector;
                if (DecideIfPerfect())
                {
                    arrow.GetComponent<SpriteRenderer>().color = Color.white;
                }
                return true;
            }
        }
        else
        {
            charge = 0;
        }
        return false;
    }

    public bool Cast(float minPerfect, float maxPerfect, bool chargingCondition, bool chargeEndCondition, bool breakChargeCondition,
        List<float> chargeMultipliers, Vector2 position, ref Vector2 chargeVector, GameObject arrow)
    {
        return Cast(chargingValue, maxCharge, minPerfect, maxPerfect, chargingCondition, chargeEndCondition, breakChargeCondition,
            chargeMultipliers, position, ref chargeVector, arrow);
    }

    public bool Cast(bool chargingCondition, bool chargeEndCondition, bool breakChargeCondition, List<float> chargeMultipliers,
        Vector2 position, ref Vector2 chargeVector, GameObject arrow)
    {
        return Cast(chargingValue, maxCharge, minPerfectCharge, maxPerfectCharge, chargingCondition, chargeEndCondition, breakChargeCondition,
            chargeMultipliers, position, ref chargeVector, arrow);
    }

    private void IncreaseCharge(float charge)
    {
        if (distanceIncreasedByCombo)
        {
            this.charge += charge*(control.GetMovementCombo() * comboMultiplier + 1);
        }
        else
        {
            this.charge += charge;
        }
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

    public bool IsPerfect()
    {
        return perfect;
    }

    public bool IsCharged()
    {
        bool temp = isCharged;
        isCharged = false;
        return temp;
    }

    private float CountMultiplier(List<float> chargeMultipliers)
    {
        float multiplier = chargeMultiplier;
        foreach(float multi in chargeMultipliers)
        {
            multiplier *= multi;
        }
        return multiplier;
    }

    private void SetBreak(bool chargingCondition, bool breakCondition)
    {
        if(breakCondition)
        {
            breakCharge = true;
        }
        if(!chargingCondition && !breakCondition)
        {
            breakCharge = false;
        }
    }
}
