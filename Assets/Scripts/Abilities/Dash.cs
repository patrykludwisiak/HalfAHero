using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Dash : Ability
{
    [SerializeField]protected GameObject Footprint;
    [SerializeField]protected bool bounceFromBlockade;
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
    string soundTypeSetting;
    [SerializeField] protected float dashSpeed;
    [SerializeField] protected bool changeHeight;

    private void Start()
    {
		dust = GameObject.Find("Ifer").transform.GetChild(6).GetComponent<ParticleSystem>();
        particleSetting = dust.main;
        soundTypeSetting = "GrassLandingSound";
        dashVelocity = Vector2.zero;
        speed = 0f;
        distance = 0f;
        isDashing = false;
        spritePosition = null;
        blockadeHit = false;
    }

    public override bool Cast()
    {
        return false;
    }

    public virtual bool Cast(float dashSpeed, List<float> dashMultipliers, Rigidbody2D rigidBody, Vector2 chargeVector, GameObject spriteObject)
    {
        
        if (!isDashing)
        {
            spritePosition = spriteObject.transform;
            this.chargeVector = Vector3.Normalize(chargeVector);
            endPoint = rigidBody.position + chargeVector;
            distance = Vector2.Distance(rigidBody.position, endPoint);
            spritePosY = spritePosition.localPosition.y;
            isDashing = true;
            footPosition = new Vector2(spritePosition.position.x - 0.01f, spritePosition.position.y -0.365f);
            CreateFootPrint(footPosition);
            CreateDust();
            FindObjectOfType<AudioManager>().Play("DashSound");
            transform.eulerAngles = new Vector3(0,0,Vector2.Angle(chargeVector, Vector2.up));
        }
        else
        {
            intendedPassedDistance += dashVelocity.magnitude * Time.fixedDeltaTime;
            speed = dashSpeed;
            dashVelocity = this.chargeVector * speed * CountMultiplier(dashMultipliers);
            if(!blockadeHit)
            {
                rigidBody.velocity = dashVelocity;
            }
            else
            {
                rigidBody.velocity = -0.2f * dashVelocity;
            }
            if(changeHeight)
            {
                if (distance / 2 >= intendedPassedDistance)
                {
                    spritePosition.localPosition = new Vector3(spritePosition.localPosition.x, spritePosition.localPosition.y + (0.025f * CountMultiplier(dashMultipliers)), spritePosition.localPosition.z);
                }
                else if (intendedPassedDistance <= distance)
                {
                    spritePosition.localPosition = new Vector3(spritePosition.localPosition.x, spritePosition.localPosition.y - (0.025f * CountMultiplier(dashMultipliers)), spritePosition.localPosition.z);
                }
            }
            if (speed <= 0 || intendedPassedDistance > distance)
            {
                dashVelocity = Vector2.zero;
                rigidBody.velocity = Vector3.zero;
                endPoint = Vector2.zero;
                speed = 0;
                distance = 0;
                intendedPassedDistance = 0;
                spritePosition.localPosition = new Vector3(spritePosition.localPosition.x, spritePosY, spritePosition.localPosition.z);
				CreateDust();
                FindObjectOfType<AudioManager>().Play(soundTypeSetting);
                footPosition = new Vector2(spritePosition.position.x - 0.01f, spritePosition.position.y - 0.365f);
                CreateFootPrint(footPosition);
                spritePosition = null;
                isDashing = false;
                blockadeHit = false;
                return true;
            }
        }
        
        return false;
    }

    public virtual bool Cast(List<float> dashMultipliers, Rigidbody2D rigidBody, Vector2 chargeVector, GameObject spriteObject)
    {
        return Cast(dashSpeed, dashMultipliers, rigidBody, chargeVector, spriteObject);
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
	
	void CreateDust(){
		dust.Play();
    }

    void CreateFootPrint(Vector2 vec)
    {
        GameObject fp = Instantiate(Footprint);
        fp.transform.position = vec;
        fp.transform.eulerAngles = new Vector3(0,0,Vector2.SignedAngle(Vector2.up, chargeVector));
        fp.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(bounceFromBlockade && collision.CompareTag("Blockade"))
        {
            blockadeHit = true;
        }
        if(collision.CompareTag("Color"))
        {
            particleSetting.startColor = collision.GetComponent<ColorHolder>().GetColor();
            soundTypeSetting = collision.GetComponent<ColorHolder>().GetSound();
        }
    }
}
