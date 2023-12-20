using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    public CharacterScriptableObjects characterStats;
    private int currentMaxHP;
    [SerializeField] private int currentHp;
    private int currentRecovery;
    private int currentStrength;
    private int currentArmor;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float currentMagnet;

    #region Stat Property
    public int CurrentMaxHP
    {
        get => currentMaxHP;
        set
        {
            currentMaxHP = value;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.maxHPDisplay.text = "" + currentMaxHP;
            }
        }
    }

    public int CurrentHp
    {
        get => currentHp;
        set
        {
            currentHp = value;
            // if (GameManager.Instance != null)
            // {
            //     GameManager.Instance.maxHPDisplay.text = "" + currentHp;
            // }
        }
    }
    public int CurrentRecovery
    {
        get => currentRecovery;
        set
        {
            currentRecovery = value;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.recoveryDisplay.text = "" + currentRecovery;
            }
        }
    }
    public int CurrentStrength
    {
        get => currentStrength;
        set
        {
            currentStrength = value;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.strengthDisplay.text = "+" + currentStrength;
            }
        }
    }
    public int CurrentArmor
    {
        get => currentArmor;
        set
        {
            currentArmor = value;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.armorDisplay.text = "+" + currentArmor;
            }
        }
    }
    public float CurrentSpeed
    {
        get => currentSpeed;
        set
        {
            currentSpeed = value;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.moveSpeedDisplay.text = "" + currentSpeed.ToString("F2");
            }
        }
    }
    public float CurrentMagnet
    {
        get => currentMagnet;
        set
        {
            currentMagnet = value;
        }
    }

    #endregion


    [Header("Level/Exp")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    [Header("Inventory")]
    private InventoryManager inventory;
    private int weaponIndex;
    private int passiveItemIndex;

    [Header("Health Bar and Experience Bar")]
    public Image healthBar;
    public Image expBar;
    public TextMeshProUGUI levelText;

    [Header("Damage Feedback")]
    private Color dmgColor = Color.red;
    private Color originalColor;
    private SpriteRenderer playerSprite;

    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }
    public List<LevelRange> levelRanges;


    private void Awake()
    {
        characterStats = CharacterSelector.GetData();
        CharacterSelector.Instance.DestroySingleton();

        inventory = GetComponentInChildren<InventoryManager>();

        CurrentMaxHP = characterStats.MaxHp;
        CurrentHp = characterStats.MaxHp;
        CurrentRecovery = characterStats.Recovery;
        CurrentStrength = characterStats.Strength;
        CurrentArmor = characterStats.Armor;
        CurrentSpeed = characterStats.Speed;
        CurrentMagnet = characterStats.Magnet;

        //player Load
        LoadModel(characterStats.Character);
        SpawnWeapon(characterStats.StartingWeapon);

        playerSprite = transform.GetChild(3).GetComponent<SpriteRenderer>(); // model nam o vi tri thu 4 trong obj cha

    }

    private void Start()
    {
        //experienceCap = levelRanges[0].experienceCapIncrease; //khoi tao experienceCap tai lv dau tien

        //GameManager.Instance.maxHPDisplay.text = "" + currentHp;
        //show chi so
        GameManager.Instance.maxHPDisplay.text = "" + currentMaxHP;
        GameManager.Instance.recoveryDisplay.text = "" + currentRecovery;
        GameManager.Instance.strengthDisplay.text = "+" + currentStrength;
        GameManager.Instance.armorDisplay.text = "+" + currentArmor;
        GameManager.Instance.moveSpeedDisplay.text = "" + currentSpeed.ToString("F2");

        GameManager.Instance.ResultChosenCharacters(characterStats); //show player da choi khi chet

        InvokeRepeating("Recovery", 0f, 5f); // tu hoi phuc moi 5s

        UpdateHealthBar();
        UpdateExpBar();
        UpdateLevelText();

        originalColor = playerSprite.color;

    }

    public void IncreaseExperience(int amount)
    {
        experience += amount;
        LevelUpChecker();
        UpdateExpBar();
    }
    private void LevelUpChecker()
    {
        if (level == 70) //cap lv o 70
        {
            Debug.Log("Reached maximum level!");
            return;
        }
        if (experience >= experienceCap)
        {
            level++;
            //CurrentMaxHP += 2;
            Debug.Log("Level Up to " + level);
            experience -= experienceCap;

            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if (level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease;

            UpdateLevelText();

            GameManager.Instance.StartLevelUp();
        }
    }

    public void TakeDamage(int dmg)
    {
        //CurrentHp -= dmg;
        CurrentHp -= CalculateDamageTake(dmg);
        Debug.Log("take " + CalculateDamageTake(dmg) + " dmg");
        StartCoroutine(DamageFlash());

        if (CurrentHp <= 0)
        {
            CurrentHp = 0;
            PlayerDead();
        }

        UpdateHealthBar();
    }
    private int CalculateDamageTake(int dmg)
    {

        if (dmg > CurrentArmor)
        {
            dmg -= CurrentArmor;
        }
        else
        {
            dmg = 1; //dmg quai gay ra be hon giap cua ng choi thi van gay 1 dmg.
        }
        return dmg;
    }
    protected IEnumerator DamageFlash()
    {
        playerSprite.color = dmgColor;
        yield return new WaitForSeconds(0.2f);
        playerSprite.color = originalColor;

    }
    public void PlayerDead()
    {
        if (!GameManager.Instance.isGameOver)
        {
            GameManager.Instance.ChosenWeaponAndPassiveItemUI(inventory.weaponUISlots, inventory.passiveItemUiSlots);
            GameManager.Instance.GameOver();
        }
    }

    public void RestoreHealth(int amount)
    {
        if (CurrentHp < CurrentMaxHP)
        {
            CurrentHp += amount;

            if (CurrentHp > CurrentMaxHP)
            {
                CurrentHp = CurrentMaxHP;
            }
        }

        UpdateHealthBar();
    }
    private void Recovery()
    {
        if (CurrentHp < CurrentMaxHP)
        {
            CurrentHp += CurrentRecovery;

            if (CurrentHp > CurrentMaxHP)
            {
                CurrentHp = CurrentMaxHP;
            }
        }

        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        healthBar.fillAmount = (float)CurrentHp / CurrentMaxHP;
    }
    private void UpdateExpBar()
    {
        expBar.fillAmount = (float)experience / experienceCap;
    }
    private void UpdateLevelText()
    {
        levelText.text = "LV: " + level.ToString();
    }

    public void LoadModel(GameObject model)
    {
        GameObject spawnModel = Instantiate(model, transform.position, Quaternion.identity);
        spawnModel.transform.SetParent(transform);
    }

    public void SpawnWeapon(GameObject weapon)
    {
        if (weaponIndex >= inventory.listWeaponSlots.Count - 1)
        {
            Debug.LogWarning("Inventory is full");
            return;
        }
        GameObject spawnWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnWeapon.transform.SetParent(transform);
        inventory.AddWeapon(weaponIndex, spawnWeapon.GetComponent<WeaponController>());
        weaponIndex++;
    }
    public void SpawnPassiveItem(GameObject passiveItem)
    {
        if (passiveItemIndex >= inventory.listPassiveItemSlots.Count - 1)
        {
            Debug.LogWarning("Inventory is full");
            return;
        }
        GameObject spawnPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnPassiveItem.transform.SetParent(transform);
        inventory.AddPassiveItem(passiveItemIndex, spawnPassiveItem.GetComponent<PassiveItem>());
        passiveItemIndex++;
    }

    private void OnDestroy()
    {
        playerSprite.color = originalColor;
    }
}
