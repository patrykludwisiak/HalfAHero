using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Shield : Weapons
{
    private bool chargingCondition;
    private bool chargingEndCondition;
    private bool chargingBreakCondition;
    private bool runeCrash;
    private bool shieldBashAvailable;
    private float attackMulitplier;
    private Vector2 dashVector;
    private Deflect deflect;
    private float deflectTimer;
    [SerializeField] float dashSpeed;
    [SerializeField] float attackDashDamage;
    [SerializeField] AttackTypes attackDashType;
    [SerializeField] float attackDashStaminaDrained;
    [SerializeField] float blockStaminaDrained;
    [SerializeField] float deflectTiming;
    [SerializeField] float deflectSpeed;
    [SerializeField] float shieldYoffset = 0.2f;
    [SerializeField] float swipeDistance = 0.8f;
    [SerializeField] GameObject shieldSwipePrefab;
    [SerializeField] float shieldSwipeMaxDamage;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        deflect = Ability.GetAbilityOfType<Deflect>(abilities);
        chargingCondition = false;
        chargingEndCondition = false;
        chargingBreakCondition = false;
        runeCrash = false;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collision = GetComponent<Collider2D>();
        attackMulitplier = 1f;
        deflectTimer = deflectTiming;
    }

    // Update is called once per frame
    void Update()
    {
        chargingCondition = Input.GetMouseButton(0) || Input.GetButton(PlayerControl.GetButtonOfControllerType("Attack"));
        if (!chargingEndCondition)
        {
            chargingEndCondition = Input.GetMouseButtonUp(0) || Input.GetButtonUp(PlayerControl.GetButtonOfControllerType("Attack"));
        }
        if (!chargingBreakCondition)
        {
            chargingBreakCondition = Input.GetMouseButton(1) || Input.GetButton(PlayerControl.GetButtonOfControllerType("Jump"));
        }
        if(!shieldBashAvailable && Input.GetButtonDown(PlayerControl.GetButtonOfControllerType("SpecialAttack")))
        {
            shieldBashAvailable = true;
        }
        if (shieldBashAvailable && Input.GetButtonUp(PlayerControl.GetButtonOfControllerType("SpecialAttack")) && playerStats.GetAbilityCharge() == 0)
        {
            shieldBashAvailable = false;
        }
    }

    public override bool Attack()
    {
        if ((chargingBreakCondition && !chargingCondition && chargingEndCondition) || (chargingBreakCondition && isAttacking) ||
               (chargingBreakCondition && !chargingEndCondition && !chargingCondition))
        {
            chargingEndCondition = false;
            chargingBreakCondition = false;
        }
        if (playerStats.GetStamina() >= attackDashStaminaDrained) {
            Charge charge = Ability.GetAbilityOfType<Charge>(abilities);
            isCharging = charge.Cast(chargingCondition, chargingEndCondition, chargingBreakCondition,
                StatusEffect.GetListOfEffectsValues<Slow>(playerControl.GetStatusEffects()), playerControl.GetRigidbody().position,
                ref playerControl.GetLookVectorRef(), playerControl.GetArrowObject());
            dashVector = playerControl.GetLookVector();
            if (isCharging)
            {
                spriteRenderer.enabled = true;
                collision.enabled = true;
                playerControl.PlayAnimation(animator, "Shield", dashVector);

                //shieldBash

                deflectTimer -= Time.fixedDeltaTime;

            }
            if ((chargingBreakCondition && !chargingCondition && chargingEndCondition) || (chargingBreakCondition && isAttacking))
            {
                chargingEndCondition = false;
                chargingBreakCondition = false;
            }
            if (charge.IsCharged())
            {
                chargingEndCondition = false;
                isAttacking = true;
                bool perfect = charge.IsPerfect();
                if (perfect)
                {
                    attackMulitplier = 1.5f;
                    playerControl.DecreaseStamina(attackDashStaminaDrained/2);
                }
                else
                {
                    playerControl.DecreaseStamina(attackDashStaminaDrained);
                }
            }
        }
        if (isAttacking)
        {
            AttackDash dash = Ability.GetAbilityOfType<AttackDash>(abilities);
            if (dash.Cast(dashSpeed, StatusEffect.GetListOfEffectsValues<Slow>(playerControl.GetStatusEffects()),
                playerControl.GetRigidbody(), dashVector, enemiesToAttack, attackDashDamage * attackMulitplier,
                DecideAttackType(attackDashType), playerControl.gameObject.transform.GetChild(0).gameObject))
            {
                isAttacking = false;
                attackMulitplier = 1f;
                playerControl.ClearAttackedEnemies();
                runeCrash = true;
                deflectTimer = deflectTiming;
                playerControl.ResetMovementCombo();
            }
        }
        /*Crash crash = Ability.GetAbilityOfType<Crash>(abilities);
        if (crash)
        {
            crash.Cast(runeCrash);
        }*/
        runeCrash = false;
        if (isCharging || isAttacking)
        {
            transform.position = playerControl.GetPlayerPosition() + (Vector2)Vector3.Normalize(dashVector)*0.3f + new Vector2(0.0f, shieldYoffset);
            playerControl.PlayAnimation("PlayerCharging", dashVector);
            return true;
        }
        if(!isCharging && !isAttacking)
        {
            spriteRenderer.enabled = false;
            collision.enabled = false;
        }
        chargingEndCondition = false;

        float abilityCharge = playerStats.GetAbilityCharge();
        if (abilityCharge > 0 && shieldBashAvailable)
        {
            GameObject shieldSwipe = Instantiate(shieldSwipePrefab);
            shieldSwipe.GetComponent<ShieldSwipe>().SetDamage(shieldSwipeMaxDamage * abilityCharge / playerStats.GetMaxAbilityCharge());
            playerStats.SetAbilityCharge(0);
            shieldSwipe.transform.parent = gameObject.transform;
            shieldSwipe.transform.position = playerControl.GetPlayerPosition() + (Vector2)Vector3.Normalize(dashVector) * swipeDistance + new Vector2(0.0f, shieldYoffset);
            shieldSwipe.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.Normalize(dashVector));
            shieldBashAvailable = false;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Bullet") && (isCharging || isAttacking) && !collision.GetComponent<Bullet>().IsPenetrating() && playerStats.GetStamina() >= blockStaminaDrained)
        {
            if(deflectTimer>0)
            {
                deflect.Cast(collision.gameObject, Vector3.Normalize(dashVector), deflectSpeed, "Enemy");
            }
            else
            {
                Destroy(collision.gameObject);
            }
            playerControl.DecreaseStamina(blockStaminaDrained);
        }
    }
}
