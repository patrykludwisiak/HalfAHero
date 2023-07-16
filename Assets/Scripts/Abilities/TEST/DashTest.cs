using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DashTest : AbilityTest
{
    [SerializeField] protected GameObject Footprint;
    [SerializeField] protected bool bounceFromBlockade;
    protected ParticleSystem dust;
    protected bool isDashing;
    protected Vector2 chargeVector;
    protected Vector2 dashVelocity;
    protected Vector2 endPoint;
    protected Vector2 footPosition;
    protected float distance;
    protected float speed;
    protected float intendedPassedDistance;
    protected float spritePosY;
    protected Transform spritePosition;
    private bool blockadeHit;
    ParticleSystem.MainModule particleSetting;
    [SerializeField] protected float dashSpeed;
    [SerializeField] protected bool changeHeight;
    private void Start()
    {
        dashVelocity = Vector2.zero;
        speed = 0f;
        distance = 0f;
        isDashing = false;
        spritePosition = null;
        blockadeHit = false;
    }
    protected override AbilityReturn AbilityScript(WeaponTest weapon)
    {
        if(!dust)
        {
            dust = weapon.GetPlayerDust();
            particleSetting = dust.main;
        }
        if (!isDashing)
        {
            if (weapon.GetChargeVector() != Vector2.zero)
            {
                spritePosition = weapon.GetPlayerSpriteObject().transform;
                weapon.ModifyArrow(true);
                weapon.SetLookVector(Vector3.Normalize(weapon.GetChargeVector()));
                chargeVector = weapon.GetChargeVector();
                endPoint = weapon.GetPlayerRigidbody().position + chargeVector;
                distance = Vector2.Distance(weapon.GetPlayerRigidbody().position, endPoint);
                spritePosY = spritePosition.localPosition.y;
                isDashing = true;
                footPosition = new Vector2(spritePosition.position.x - 0.01f, spritePosition.position.y - 0.365f);
                CreateFootPrint(footPosition, weapon);
                CreateDust();
                FindObjectOfType<AudioManager>().Play("DashSound");
                transform.eulerAngles = new Vector3(0, 0, Vector2.Angle(weapon.GetChargeVector(), Vector2.up));
            }
            else
            {
                return AbilityReturn.True;
            }
        }
        else
        {
            weapon.SetPosition((Vector2)weapon.GetPlayerTransform().position + (Vector2)Vector3.Normalize(chargeVector) * 0.3f + new Vector2(0.0f, 0.2f));
            weapon.GetPlayerControl().PlayAnimation("PlayerCharging", Vector3.Normalize(chargeVector));
            intendedPassedDistance += dashVelocity.magnitude * Time.fixedDeltaTime;
            speed = dashSpeed;
            dashVelocity = chargeVector * speed * CountMultiplier(StatusEffect.GetListOfEffectsValues<Slow>(weapon.GetStatusEffects()));
            if (!blockadeHit)
            {
                weapon.GetPlayerRigidbody().velocity = dashVelocity;
            }
            else
            {
                weapon.GetPlayerRigidbody().velocity = -0.2f * dashVelocity;
            }
            if (changeHeight)
            {
                if (distance / 2 >= intendedPassedDistance)
                {
                    spritePosition.localPosition = new Vector3(spritePosition.localPosition.x, spritePosition.localPosition.y + (0.025f * CountMultiplier(StatusEffect.GetListOfEffectsValues<Slow>(weapon.GetStatusEffects()))), spritePosition.localPosition.z);
                }
                else if (intendedPassedDistance <= distance)
                {
                    spritePosition.localPosition = new Vector3(spritePosition.localPosition.x, spritePosition.localPosition.y - (0.025f * CountMultiplier(StatusEffect.GetListOfEffectsValues<Slow>(weapon.GetStatusEffects()))), spritePosition.localPosition.z);
                }
            }
            if (speed <= 0 || intendedPassedDistance > distance)
            {
                dashVelocity = Vector2.zero;
                weapon.GetPlayerRigidbody().velocity = Vector3.zero;
                endPoint = Vector2.zero;
                speed = 0;
                distance = 0;
                intendedPassedDistance = 0;
                spritePosition.localPosition = new Vector3(spritePosition.localPosition.x, spritePosY, spritePosition.localPosition.z);
                CreateDust();
                footPosition = new Vector2(spritePosition.position.x - 0.01f, spritePosition.position.y - 0.365f);
                CreateFootPrint(footPosition, weapon);
                spritePosition = null;
                isDashing = false;
                blockadeHit = false;
                weapon.ModifyArrow(false);
                return AbilityReturn.True;
            }
        }
        return AbilityReturn.False;
    }
    void CreateDust()
    {
        dust.Play();
    }
    void CreateFootPrint(Vector2 vec, WeaponTest weapon)
    {
        GameObject fp = Instantiate(Footprint);
        fp.transform.position = vec;
        fp.transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, weapon.GetChargeVector()));
        fp.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (bounceFromBlockade && collision.CompareTag("Blockade"))
        {
            blockadeHit = true;
        }
        if (collision.CompareTag("Color"))
        {
            if(dust)
            {
            particleSetting.startColor = collision.GetComponent<ColorHolder>().GetColor();
            }
        }
    }
    protected float CountMultiplier(List<float> dashMultipliers)
    {
        float multiplier = 1;
        foreach (float multi in dashMultipliers)
        {
            multiplier *= multi;
        }
        return multiplier;
    }
}
