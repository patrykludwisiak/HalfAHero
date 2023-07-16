using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerControl : MonoBehaviour
{
    private Vector2 controller;
    private Vector2 mouse;
    private int movementCombo;
    [SerializeField] private bool isWalk;
    [SerializeField] private bool isMovingByWalk;
    private bool isDashing;
    private bool isCharging;
    private bool chargingCondition;
    private bool chargingEndCondition;
    private bool chargingBreakCondition;
    private List<StatusEffect> statusEffects;
    private List<EnemyStatistics> attackedEnemies;
    Vector2 lookVector; //normalized vector of looking direction
    Inventory inventory;
    Weapons weapon;
    PlayerStatistics stats;
    SpriteRenderer playerRenderer;
    [SerializeField] private float staminaRegeneration;
    [SerializeField] private List<GameObject> abilities;
    [SerializeField] private Animator animator;
    private string[] directions =
        {"S", "SE", "E", "NE", "N", "NW", "W", "SW"};
    [SerializeField] private int maxMovementCombo;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private GameObject arrow;

    private void Awake()
    {
        attackedEnemies = new List<EnemyStatistics>();
        controller = new Vector2(Input.GetAxis("JoystickHorizontal"), Input.GetAxis("JoystickVertical"));
        mouse = Input.mousePosition;
        abilities = Ability.LoadAbilities(gameObject, abilities);
        statusEffects = new List<StatusEffect>();
        stats = GetComponent<PlayerStatistics>();
        inventory = gameObject.GetComponentInChildren<Inventory>();
        playerRenderer = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        chargingCondition = false;
        chargingEndCondition = false;
        chargingBreakCondition = false;
    }

    private void Update()
    {
        StaminaRegeneration(staminaRegeneration);
        StatusEffect.DoStatusEffects(statusEffects, false);
        chargingCondition = Input.GetMouseButton(1) || Input.GetButton(GetButtonOfControllerType("Jump"));
        if (!chargingEndCondition)
        {
            chargingEndCondition = Input.GetMouseButtonUp(1) || Input.GetButtonUp(GetButtonOfControllerType("Jump"));
        }
        if(!chargingBreakCondition)
        {
            chargingBreakCondition = Input.GetMouseButton(0) || Input.GetButton(GetButtonOfControllerType("Attack"));
        }
    }
    
    private void FixedUpdate()
    {
        if (inventory.GetCurrentWeapon() != weapon)
        {
            weapon = inventory.GetCurrentWeapon().GetComponent<Weapons>();
        }

        //walking controll for pad
        isWalk = Input.GetButton(GetButtonOfControllerType("Walk"));

        lookVector = DetermineLookVector();
        bool move = Move(), attack = Attack();
        if (!move && !attack)
        {
            PlayAnimation("PlayerIdle");
            arrow.transform.position = rigidBody.position + lookVector * 0.3f;

            //slow move
            Vector2 slowMove = new Vector2(0, 0);
            //controlls for gamepad
            if (isWalk)
            {
                slowMove = Vector3.Normalize(new Vector2(Input.GetAxis("JoystickHorizontal"), Input.GetAxis("JoystickVertical")));
            }

            //controlls for keboard
            if (Input.GetButton("SlowWalkUp"))
            {
                slowMove += new Vector2(0, 1);
            }
            if (Input.GetButton("SlowWalkDown"))
            {
                slowMove += new Vector2(0, -1);
            }
            if (Input.GetButton("SlowWalkLeft"))
            {
                slowMove += new Vector2(-1, 0);
            }
            if (Input.GetButton("SlowWalkRight"))
            {
                slowMove += new Vector2(1, 0);
            }

            transform.position += stats.GetSlowMovementSpeed() * Time.deltaTime * (Vector3)slowMove.normalized;

            //sets if player is moving only by slow walk to sink if over water
            isMovingByWalk = !slowMove.normalized.Equals(new Vector2(0, 0));

        }
        else if (isCharging || Is_Attack_charging())
        {
            isMovingByWalk = false;
            chargingBreakCondition = false;
        }
    }

    private void StaminaRegeneration(float staminaRegeneration)
    {
        float stam = stats.GetStamina(), maxStam = stats.GetMaxStamina();
        if(stam + staminaRegeneration * Time.deltaTime > maxStam)    
        {
            stats.IncreaseStamina(maxStam - stam);
        }
        else
        {
            stats.IncreaseStamina(staminaRegeneration * Time.deltaTime);
        }
    }

    public bool Attack()
    {
        bool result = false;
        if (weapon)
        {
            result = weapon.Attack();
        }
        return result;
    }

    public bool Is_Attack_charging()
    {
        bool result = false;
        if (weapon)
        {
            result = weapon.IsCharging();
        }
        return result;
    }

    public bool Is_moving_by_walk()
    {
        return isMovingByWalk;
    }

    public bool Move()
    {
        if ((chargingBreakCondition && !chargingCondition && chargingEndCondition) || (chargingBreakCondition && isDashing) ||
            (chargingBreakCondition && !chargingEndCondition && !chargingCondition))
        {
            chargingEndCondition = false;
            chargingBreakCondition = false;
        }
        if (!weapon || (!weapon.IsAttacking() && !weapon.Move()))
        {
            Charge charge = Ability.GetAbilityOfType<Charge>(abilities);
            isCharging = charge.Cast(chargingCondition, chargingEndCondition, chargingBreakCondition,
                StatusEffect.GetListOfEffectsValues<Slow>(statusEffects), rigidBody.position, ref lookVector, arrow);
            if (isCharging)
            {
                PlayAnimation("PlayerCharging");
            }
            if (charge.IsCharged())
            {
                chargingEndCondition = false;
                isDashing = true;
                bool perfect = charge.IsPerfect();
                if (perfect)
                {
                    IncreaseMovementCombo();
                    StaminaRegeneration(40);
                }
                else
                {
                    ResetMovementCombo();
                }

            }
            if(isDashing)
            {
                Dash dash = Ability.GetAbilityOfType<Dash>(abilities);
                if (dash.Cast(stats.GetMovementSpeed(), CombineSlowAndMovementCombo(), rigidBody, lookVector, transform.GetChild(0).gameObject))
                {
                    isDashing = false;
                }
            }
            if(isCharging || isDashing)
            {
                return true;
            }
        }
        return false;
    }

    public static int GetAngleIndexOfEight(Vector3 normalizedVector)
    {
        int value = Mathf.FloorToInt((Vector2.SignedAngle(Vector2.up, normalizedVector) + 202.5f) / 45f);
        if(value == 8)
        {
            return 0;
        }
        return value;
    }

    public int GetMovementCombo()
    {
        return movementCombo;
    }

    public void IncreaseMovementCombo()
    {
        if(movementCombo < maxMovementCombo)
        {
            movementCombo++;
        }
    }

    public void ResetMovementCombo()
    {
        movementCombo = 0;
    }

    public Vector2 DetermineLookVector()
    {
        Vector2 tempController = Vector3.Normalize(new Vector2(Input.GetAxis("JoystickHorizontal"), Input.GetAxis("JoystickVertical")));
        Vector2 tempMouse = Input.mousePosition;
        if (tempMouse != mouse)
        {
            mouse = tempMouse;
            return Vector3.Normalize((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - rigidBody.position);
        }
        else if (tempController != controller)
        {
            controller = tempController;
            return tempController;
        }
        if(lookVector == Vector2.zero && tempController == Vector2.zero)
        {
            return new Vector2(0, -0.01f);
        }
        else if(lookVector == Vector2.zero && tempController != Vector2.zero)
        {
            controller = tempController;
            return tempController;
        }

        return Vector3.Normalize(lookVector);
    }

    public Vector2 GetLookVector()
    {
        return lookVector;
    }

    public ref Vector2 GetLookVectorRef()
    {
        return ref lookVector;
    }

    public void SetLookVector(Vector2 lookVector)
    {
        this.lookVector = lookVector;
    }

    public Rigidbody2D GetRigidbody()
    {
        return rigidBody;
    }

    public Vector2 GetPlayerPosition()
    {
        return transform.position;
    }

    public PlayerStatistics GetPlayerStats()
    {
        return stats;
    }

    public SpriteRenderer GetPlayerRenderer()
    {
        return playerRenderer;
    }

    public void DecreaseStamina(float stamina)
    {
        stats.IncreaseStamina(-stamina);
    }

    public GameObject GetArrowObject()
    {
        return arrow;
    }

    public List<StatusEffect> GetStatusEffects()
    {
        return statusEffects;
    }

    public List<float> CombineSlowAndMovementCombo()
    {
        List<float> toReturn = StatusEffect.GetListOfEffectsValues<Slow>(statusEffects);
        toReturn.Add(1 / (1 + Mathf.Pow(movementCombo + 0.5f, -0.5f * (movementCombo + 1))));
        return toReturn;

    }

    public static string GetButtonOfControllerType(string name)
    {
        if (Input.GetJoystickNames().Length !=0)
        {
           if (!Input.GetJoystickNames()[0].ToLower().Contains("xbox"))
           {
               return "PS" + name;
           }
        }
        return name;
    }

    public void AddEnemyToAttackedEnemies(GameObject enemy)
    {
        attackedEnemies.Add(enemy.GetComponent<EnemyStatistics>());
    }

    public void AddEnemyToAttackedEnemies(EnemyStatistics enemy)
    {
        attackedEnemies.Add(enemy);
    }

    public void ClearAttackedEnemies()
    {
        foreach(EnemyStatistics enemy in attackedEnemies)
        {
            if (enemy)
            {
                enemy.ChangeAttacked(false);
            }
        }
        attackedEnemies.Clear();
    }

    public void PlayAnimation(string animationStateName)
    {
        animator.Play(animationStateName + directions[GetAngleIndexOfEight(lookVector)]);
    }

    public void PlayAnimation( Animator animator, string animationStateName)
    {
        animator.Play(animationStateName + directions[GetAngleIndexOfEight(lookVector)]);
    }

    public void PlayAnimation(string animationStateName, Vector2 lookingVector)
    {
        animator.Play(animationStateName + directions[GetAngleIndexOfEight(lookingVector)]);
    }

    public void PlayAnimation(Animator animator, string animationStateName, Vector2 lookingVector)
    {
        animator.Play(animationStateName + directions[GetAngleIndexOfEight(lookingVector)]);
    }
}
