using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerControlTest : MonoBehaviour
{
    private Vector2 controller;
    private Vector2 mouse;
    private List<StatusEffect> statusEffects;
    private List<EnemyStatistics> attackedEnemies;
    Vector2 lookVector; //normalized vector of looking direction
    Inventory inventory;
    WeaponTest weapon;
    PlayerStatistics stats;
    SpriteRenderer playerRenderer;
    [SerializeField] private Animator animator;
    private string[] directions =
        {"S", "SE", "E", "NE", "N", "NW", "W", "SW"};
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private GameObject arrow;

    private void Awake()
    {
        attackedEnemies = new List<EnemyStatistics>();
        controller = new Vector2(Input.GetAxis("JoystickHorizontal"), Input.GetAxis("JoystickVertical"));
        mouse = Input.mousePosition;
        statusEffects = new List<StatusEffect>();
        stats = GetComponent<PlayerStatistics>();
        inventory = gameObject.GetComponentInChildren<Inventory>();
        playerRenderer = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        StatusEffect.DoStatusEffects(statusEffects, false);
        if (weapon)
        {
            weapon.WeaponDecide();
        }
    }

    private void FixedUpdate()
    {
        if (inventory.GetCurrentWeapon() != weapon)
        {
            weapon = inventory.GetCurrentWeapon().GetComponent<WeaponTest>();
        }

        //walking controll for pad

        lookVector = DetermineLookVector();
        
        if (weapon)
        {
            weapon.WeaponAction();
            if (!weapon.IsUsingAbilities())
            {
                PlayAnimation("PlayerIdle");
                arrow.transform.position = rigidBody.position + lookVector * 0.3f;
            }
        }
    }
    
    public static int GetAngleIndexOfEight(Vector3 normalizedVector)
    {
        int value = Mathf.FloorToInt((Vector2.SignedAngle(Vector2.up, normalizedVector) + 202.5f) / 45f);
        if (value == 8)
        {
            return 0;
        }
        return value;
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
        if (lookVector == Vector2.zero && tempController == Vector2.zero)
        {
            return new Vector2(0, -0.01f);
        }
        else if (lookVector == Vector2.zero && tempController != Vector2.zero)
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

    public GameObject GetArrowObject()
    {
        return arrow;
    }

    public List<StatusEffect> GetStatusEffects()
    {
        return statusEffects;
    }

    public static string GetButtonOfControllerType(string name)
    {
        if (Input.GetJoystickNames().Length != 0)
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
        foreach (EnemyStatistics enemy in attackedEnemies)
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

    public void PlayAnimation(Animator animator, string animationStateName)
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
