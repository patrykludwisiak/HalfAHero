using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Holding
{
    none, click, hold
}
public class WeaponTest : MonoBehaviour
{
    [SerializeField] protected Sprite inventorySprite;
    [SerializeField] protected AttackTypes attackType;
    [SerializeField] float maxEnergy;
    [SerializeField] float energyKeepTime;
    [SerializeField] float clickToHoldTime;
    [SerializeField] float maxStamina;
    [SerializeField] float staminaPerSecond;
    [SerializeField] Transform energyBar;
    [SerializeField] Transform staminaBar;
    [SerializeField] GameObject staminaBarRoot;
    [Space]
    [SerializeField] List<AbilityTest> leftHold;
    [SerializeField] List<AbilityTest> leftClick;
    [SerializeField] List<AbilityTest> rightHold;
    [SerializeField] List<AbilityTest> rightClick;
    [SerializeField] List<AbilityTest> qButtonHold;
    [SerializeField] List<AbilityTest> qButtonClick;
    [SerializeField] List<AbilityTest> eButtonHold;
    [SerializeField] List<AbilityTest> eButtonClick;
    [Space]
    [SerializeField] private GameObject arrow;

    Vector2 lookVector;
    Vector2 chargeVector;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected Collider2D collision;
    protected PlayerControlTest playerControl;
    protected bool isModifyingArrow;

    protected bool usingAbilities;
    protected bool leftSetAbility;
    protected bool rightSetAbility;
    protected bool qSetAbility;
    protected bool eSetAbility;
    protected List<GameObject> objectHit;
    private List<GameObject> sharedObjects;
    protected int energy;
    protected int newEnergy;
    protected float currentStamina;
    protected bool isStaminaRegening;
    private float rightHoldingTime;
    private float leftHoldingTime;
    private float qHoldingTime;
    private float eHoldingTime;
    protected GameObject player;
    protected Transform playerTransform;
    protected Rigidbody2D playerRigidbody;
    protected ParticleSystem playerDust;
    protected float energyTimer;
    protected AbilityTest castedLeftAbility;
    protected AbilityTest castedRightAbility;
    protected AbilityTest castedQAbility;
    protected AbilityTest castedEAbility;
    protected AbilityTest leftSkippedAbility;
    protected AbilityTest rightSkippedAbility;
    protected AbilityTest qSkippedAbility;
    protected AbilityTest eSkippedAbility;
    // Start is called before the first frame update
    void Awake()
    {
        objectHit = new List<GameObject>();
        sharedObjects = new List<GameObject>();
        attackType = AttackTypes.Normal;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collision = GetComponent<Collider2D>();
        usingAbilities = false;
        energy = 0;
        newEnergy = 0;
        leftHoldingTime = 0f;
        rightHoldingTime = 0f;
        qHoldingTime = 0f;
        eHoldingTime = 0f;
        energyTimer = 0f;
        currentStamina = maxStamina;
        InstantiateAbilities();
    }

    private void Start()
    {
        player = GameData.player;
        playerControl = player.GetComponent<PlayerControlTest>();
        playerTransform = player.transform;
        playerRigidbody = player.GetComponent<Rigidbody2D>();
        playerDust = player.transform.GetChild(6).GetComponent<ParticleSystem>();
    }

    public void WeaponDecide()
    {
        if (spriteRenderer.enabled)
        {
            playerControl.PlayAnimation(animator, "Shield");
        }
        if (CheckIfHold(PlayerControlTest.GetButtonOfControllerType("Attack")) == Holding.none || CheckIfHold(PlayerControlTest.GetButtonOfControllerType("Jump")) == Holding.none || CheckIfHold(PlayerControlTest.GetButtonOfControllerType("SpecialSkill1")) == Holding.none || CheckIfHold(PlayerControlTest.GetButtonOfControllerType("SpecialSkill2")) == Holding.none)
        {
            if (castedLeftAbility == null)
            {
                switch (CheckIfHold(PlayerControlTest.GetButtonOfControllerType("Attack")))
                {
                    case Holding.hold:
                        if (leftHold.Count >= energy + 1)
                        {
                            leftSetAbility = true;
                            castedLeftAbility = leftHold[energy];
                        }
                        break;
                    case Holding.click:
                        if (leftClick.Count >= energy + 1)
                        {
                            leftSetAbility = true;
                            castedLeftAbility = leftClick[energy];
                        }
                        break;
                }
            }
            if (castedRightAbility == null)
            {
                switch (CheckIfHold(PlayerControlTest.GetButtonOfControllerType("Jump")))
                {
                    case Holding.hold:
                        if (rightHold.Count >= energy + 1)
                        {
                            rightSetAbility = true;
                            castedRightAbility = rightHold[energy];
                        }
                        break;
                    case Holding.click:
                        if (rightClick.Count >= energy + 1)
                        {
                            rightSetAbility = true;
                            castedRightAbility = rightClick[energy];
                        }
                        break;
                }
            }
            if (castedQAbility == null)
            {
                switch (CheckIfHold(PlayerControlTest.GetButtonOfControllerType("SpecialSkill1")))
                {
                    case Holding.hold:
                        if (qButtonHold.Count >= energy + 1)
                        {
                            qSetAbility = true;
                            castedQAbility = qButtonHold[energy];
                        }
                        break;
                    case Holding.click:
                        if (qButtonClick.Count >= energy + 1)
                        {
                            qSetAbility = true;
                            castedQAbility = qButtonClick[energy];
                        }
                        break;
                }
            }
            if (castedEAbility == null)
            {
                switch (CheckIfHold(PlayerControlTest.GetButtonOfControllerType("SpecialSkill2")))
                {
                    case Holding.hold:
                        if (eButtonHold.Count >= energy + 1)
                        {
                            eSetAbility = true;
                            castedEAbility = eButtonHold[energy];
                        }
                        break;
                    case Holding.click:
                        if (eButtonClick.Count >= energy + 1)
                        {
                            eSetAbility = true;
                            castedEAbility = eButtonClick[energy];
                        }
                        break;
                }
            }
        }
        else
        {
            chargeVector = Vector2.zero;
        }
    }

    public void WeaponAction()
    {
        if(currentStamina == maxStamina)
        {
            staminaBarRoot.SetActive(false);
        }
        else
        {
            staminaBarRoot.SetActive(true);
        }
        if (currentStamina >= 0)
        {
            staminaBar.localScale = new Vector3(currentStamina / maxStamina, 1f, 1f);
        }
        else
        {
            staminaBar.localScale = new Vector3(0f, 1f, 1f);
        }
        if (isStaminaRegening)
        {
            IncreaseCurrentStamina(staminaPerSecond * Time.fixedDeltaTime);
        }
        lookVector = playerControl.GetLookVector();
        if(!isModifyingArrow)
        {
            GetArrow().transform.position = GetPlayerRigidbody().position + lookVector * 0.2f;
        }
        transform.position = playerControl.GetPlayerPosition() + (Vector2)Vector3.Normalize(lookVector) * 0.3f + new Vector2(0.0f, 0.2f);
        if (castedLeftAbility != null)
        {
            usingAbilities = true;
            AbilityReturn ability = castedLeftAbility.Cast(this);
            if (ability == AbilityReturn.True)
            {
                usingAbilities = false;
                castedLeftAbility = null;
            }
            else if (ability == AbilityReturn.Skip)
            {
                leftSkippedAbility = castedLeftAbility;
                castedLeftAbility = null;
            }
        }
        if (leftSetAbility && castedLeftAbility == null)
        {
            leftSetAbility = false;
            leftHoldingTime = 0;
        }
        if (castedRightAbility != null)
        {
            usingAbilities = true;
            AbilityReturn ability = castedRightAbility.Cast(this);
            if (ability == AbilityReturn.True)
            {
                usingAbilities = false;
                castedRightAbility = null;
                ResetEnergy();
            }
            else if(ability == AbilityReturn.Skip)
            {
                rightSkippedAbility = castedRightAbility;
                castedRightAbility = null;
            }
        }
        if (rightSetAbility && castedRightAbility == null)
        {
            rightSetAbility = false;
            rightHoldingTime = 0;
        }
        if (castedQAbility != null)
        {
            usingAbilities = true;
            AbilityReturn ability = castedQAbility.Cast(this);
            if (ability == AbilityReturn.True)
            {
                usingAbilities = false;
                castedQAbility = null;
                ResetEnergy();
            }
            else if (ability == AbilityReturn.Skip)
            {
                qSkippedAbility = castedQAbility;
                castedQAbility = null;
            }
        }
        if (qSetAbility && castedQAbility == null)
        {
            qSetAbility = false;
            qHoldingTime = 0;
        }
        if (castedEAbility != null)
        {
            usingAbilities = true;
            AbilityReturn ability = castedEAbility.Cast(this);
            if (ability == AbilityReturn.True)
            {
                usingAbilities = false;
                castedEAbility = null;
                ResetEnergy();
            }
            else if (ability == AbilityReturn.Skip)
            {
                eSkippedAbility = castedEAbility;
                castedEAbility = null;
            }
        }
        if (eSetAbility && castedEAbility == null)
        {
            eSetAbility = false;
            eHoldingTime = 0;
        }
        if (newEnergy > 0)
        {
            if(newEnergy != energy)
            {
                energy = newEnergy;
                energyTimer = 0f;
                for (int i = 0; i < energy; i++)
                {
                    energyBar.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;
                }
            }
            energyTimer += Time.deltaTime;
            if(energyTimer >= energyKeepTime)
            {
                ResetEnergy();
            }
        }
        if (leftSkippedAbility != null)
        {
            if (leftSkippedAbility.CastSkippedEnd(this) == AbilityReturn.True)
            {
                usingAbilities = false;
                leftSkippedAbility = null;
            }
        }
        if (rightSkippedAbility != null)
        {
            if(rightSkippedAbility.CastSkippedEnd(this) == AbilityReturn.True)
            {
                usingAbilities = false;
                rightSkippedAbility = null;
            }
        }
        if (qSkippedAbility != null)
        {
            if (qSkippedAbility.CastSkippedEnd(this) == AbilityReturn.True)
            {
                usingAbilities = false;
                qSkippedAbility = null;
            }
        }
        if (eSkippedAbility != null)
        {
            if (eSkippedAbility.CastSkippedEnd(this) == AbilityReturn.True)
            {
                usingAbilities = false;
                eSkippedAbility = null;
            }
        }
    }

    public Holding CheckIfHold(string inputName)
    {
        if (inputName == PlayerControlTest.GetButtonOfControllerType("Attack"))
        {
            if (Input.GetButtonUp(inputName) && leftHoldingTime > 0 && leftHoldingTime < clickToHoldTime && leftClick.Count > 0)
            {
                return Holding.click;
            }
            if (Input.GetButton(inputName) && (leftHoldingTime >= clickToHoldTime || leftClick.Count == 0))
            {
                return Holding.hold;
            }
            if (Input.GetButton(inputName))
            {
                leftHoldingTime += Time.deltaTime;
            }
        }
        else if (inputName == PlayerControlTest.GetButtonOfControllerType("Jump"))
        {
            if (Input.GetButtonUp(inputName) && rightHoldingTime > 0 && rightHoldingTime < clickToHoldTime && rightClick.Count > 0)
            {
                return Holding.click;
            }
            if (Input.GetButton(inputName) && (rightHoldingTime >= clickToHoldTime || rightClick.Count == 0))
            {
                return Holding.hold;
            }
            if (Input.GetButton(inputName))
            {
                rightHoldingTime += Time.deltaTime;
            }
        }
        else if (inputName == PlayerControlTest.GetButtonOfControllerType("SpecialSkill2"))
        {
            if (Input.GetButtonUp(inputName) && eHoldingTime > 0 && eHoldingTime < clickToHoldTime && eButtonClick.Count > 0)
            {
                return Holding.click;
            }
            if (Input.GetButton(inputName) && (eHoldingTime >= clickToHoldTime || eButtonClick.Count == 0))
            {
                return Holding.hold;
            }
            if (Input.GetButton(inputName))
            {
                eHoldingTime += Time.deltaTime;
            }
        }
        else if (inputName == PlayerControlTest.GetButtonOfControllerType("SpecialSkill1"))
        {
            if (Input.GetButtonUp(inputName) && qHoldingTime > 0 && qHoldingTime < clickToHoldTime && qButtonClick.Count > 0)
            {
                return Holding.click;
            }
            if (Input.GetButton(inputName) && (qHoldingTime >= clickToHoldTime || qButtonClick.Count == 0))
            {
                return Holding.hold;
            }
            if (Input.GetButton(inputName))
            {
                qHoldingTime += Time.deltaTime;
            }
        }
        return Holding.none;
    }

    public bool IsUsingAbilities()
    {
        return usingAbilities;
    }
    public int GetEnergy()
    {
        return energy;
    }

    public AttackTypes GetAttackType()
    {
        return attackType;
    }

    public void SetAttackType(AttackTypes attackType)
    {
        this.attackType = attackType;
    }

    public void IncreaseEnergy()
    {
        if(newEnergy + 1 <= maxEnergy)
        {
            newEnergy++;
        }
    }

    public void ResetEnergy()
    {
        newEnergy = 0;
        energy = 0;
        energyTimer = 0;

        for (int i = 0; i < maxEnergy; i++)
        {
            energyBar.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public Sprite GetInventorySprite()
    {
        return inventorySprite;
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }
    public Vector2 GetLookVector()
    {
        return lookVector;
    }
    public void SetLookVector(Vector2 lookVector)
    {
        this.lookVector = lookVector;
    }

    public Vector2 GetChargeVector()
    {
        return chargeVector;
    }

    public void SetChargeVector(Vector2 chargeVector)
    {
        this.chargeVector = chargeVector;
    }

    public void EnableCollision(bool enable)
    {
        collision.enabled = enable;
    }

    public void EnableRender(bool enable)
    {
        spriteRenderer.enabled = enable;
    }

    public Animator GetAnimator()
    {
        return animator;
    }

    public GameObject GetPlayer()
    {
        return player;
    }
    
    public PlayerControlTest GetPlayerControl()
    {
        return playerControl;
    }
    public Transform GetPlayerTransform()
    {
        return playerTransform;
    }

    public Rigidbody2D GetPlayerRigidbody()
    {
        return playerRigidbody;
    }

    public GameObject GetPlayerSpriteObject()
    {
        return player.transform.GetChild(0).gameObject;
    }

    public List<StatusEffect> GetStatusEffects()
    {
        return player.GetComponent<PlayerControl>().GetStatusEffects();
    }

    public GameObject GetArrow()
    {
        return arrow;
    }

    public void ModifyArrow(bool modify)
    {
        isModifyingArrow = modify;
    }

    public ParticleSystem GetPlayerDust()
    {
        return playerDust;
    }

    public List<GameObject> GetObjectHit()
    {
        return objectHit;
    }

    public void AddToSharedObjects(GameObject sharedObject)
    {
        sharedObjects.Add(sharedObject);
    }

    public void RemoveFromSharedObjects(GameObject sharedObject)
    {
        for(int i = 0; i < sharedObjects.Count; i++)
        {
            if(sharedObjects[i] == sharedObject)
            {
                sharedObjects.RemoveAt(i);
                break;
            }
        }
    }

    public GameObject GetSharedObject(string tag)
    {
        foreach(GameObject sharedObject in sharedObjects)
        {
            if(sharedObject.CompareTag(tag))
            {
                return sharedObject;
            }
        }
        return null;
    }

    public List<GameObject> GetSharedObjects()
    {
        return sharedObjects;
    }

    public float GetCurrentStamina()
    {
        return currentStamina;
    }

    public void DecreaseCurrentStamina(float stamina)
    {
        currentStamina -= stamina;
        if (currentStamina < 0)
        {
            currentStamina = 0;
        }
    }

    public void IncreaseCurrentStamina(float stamina)
    {
        currentStamina += stamina;
        if(currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }
    }

    public bool IsStaminaRegening()
    {
        return isStaminaRegening;
    }

    public void ChangeStaminaRegening(bool change)
    {
        isStaminaRegening = change;
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }

    public void ResetColor()
    {
        spriteRenderer.color = Color.white;
    }

    public List<AbilityTest> GetLeftClickAbilities()
    {
        return leftClick;
    }

    public List<AbilityTest> GetLeftHoldAbilities()
    {
        return leftHold;
    }

    public List<AbilityTest> GetRightClickAbilities()
    {
        return rightClick;
    }

    public List<AbilityTest> GetRightHoldAbilities()
    {
        return rightHold;
        
    }

    public List<AbilityTest> GetQClickAbilities()
    {
        return qButtonClick;
    }

    public List<AbilityTest> GetQHoldAbilities()
    {
        return qButtonHold;
    }

    public List<AbilityTest> GetEClickAbilities()
    {
        return eButtonClick;
    }

    public List<AbilityTest> GetEHoldAbilities()
    {
        return eButtonHold;

    }

    private void InstantiateAbilities()
    {
        for (int i = 0; i < leftHold.Count; i++)
        {
            leftHold[i] = Instantiate(leftHold[i], transform);
            leftHold[i].InstantiateAbilities();
        }
        for (int i = 0; i < leftClick.Count; i++)
        {
            leftClick[i] = Instantiate(leftClick[i], transform);
            leftClick[i].InstantiateAbilities();
        }
        for (int i = 0; i < rightHold.Count; i++)
        {
            rightHold[i] = Instantiate(rightHold[i], transform);
            rightHold[i].InstantiateAbilities();
        }
        for (int i = 0; i < rightClick.Count; i++)
        {
            rightClick[i] = Instantiate(rightClick[i], transform);
            rightClick[i].InstantiateAbilities();
        }
        for (int i = 0; i < qButtonHold.Count; i++)
        {
            qButtonHold[i] = Instantiate(qButtonHold[i], transform);
            qButtonHold[i].InstantiateAbilities();
        }
        for (int i = 0; i < qButtonClick.Count; i++)
        {
            qButtonClick[i] = Instantiate(qButtonClick[i], transform);
            qButtonClick[i].InstantiateAbilities();
        }
        for (int i = 0; i < eButtonHold.Count; i++)
        {
            eButtonHold[i] = Instantiate(eButtonHold[i], transform);
            eButtonHold[i].InstantiateAbilities();
        }
        for (int i = 0; i < eButtonClick.Count; i++)
        {
            eButtonClick[i] = Instantiate(eButtonClick[i], transform);
            eButtonClick[i].InstantiateAbilities();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(objectHit.IndexOf(collision.gameObject) == -1)
        {
            objectHit.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (objectHit.IndexOf(collision.gameObject) != -1)
        {
            objectHit.Remove(collision.gameObject);
        }
    }
}
