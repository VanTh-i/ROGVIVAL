using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get => instance; }

    public enum GameState
    {
        GamePlay,
        Paused,
        GameOver,
        LevelUp
    };

    [SerializeField] private int coin;
    public int Coin { get => coin; set => coin = value; }
    public bool isGameOver = false;
    [HideInInspector] public bool choosingUpgrade;
    public GameObject playerObject;
    public CanvasGroup joystickFade;

    public GameState currentState;
    [HideInInspector] public GameState previousState;

    [Header("UI")]
    public TMP_Text coinText;
    public GameObject pauseScreen;
    public GameObject resultsScreen;
    public GameObject levelUpScreen;
    public GameObject joystick;
    public Button pauseBtn;


    [Header("Stats UI")]
    public TextMeshProUGUI maxHPDisplay;
    public TextMeshProUGUI recoveryDisplay;
    public TextMeshProUGUI strengthDisplay;
    public TextMeshProUGUI armorDisplay;
    public TextMeshProUGUI moveSpeedDisplay;

    [Header("Result UI")]
    public TextMeshProUGUI resultTitle;
    public TextMeshProUGUI timeSurvivalDisplay;
    public Image chosenCharacters;
    public TextMeshProUGUI charactersName;
    public List<Image> chosenWeaponUI = new List<Image>(6);
    public List<Image> chosenPassiveItemUI = new List<Image>(6);

    [Header("Stopwatch")]
    public float timeLimit;
    private float stopwatchTime;
    public float StopwatchTime { get => stopwatchTime; private set => stopwatchTime = value; }
    public TextMeshProUGUI stopwatchDisplay;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        BeginGame();
    }

    private void BeginGame()
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
        levelUpScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    private void Start()
    {
        SoundManager.Instance.musicSource.Stop();
        SoundManager.Instance.PlayMusic("Theme");
        coinText.text = Coin.ToString();
    }

    private void Update()
    {
        coinText.text = Coin.ToString();

        switch (currentState)
        {
            case GameState.GamePlay:
                CheckPauseAndResume();
                UpdateStopwatch();
                break;

            case GameState.Paused:
                CheckPauseAndResume();
                break;

            case GameState.GameOver:
                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f;
                    Debug.Log("GAME OVER");
                    DisplayResults();
                }
                break;

            case GameState.LevelUp:
                if (!choosingUpgrade)
                {
                    choosingUpgrade = true;
                    Time.timeScale = 0f;
                    levelUpScreen.SetActive(true);
                }
                break;

            default:
                Debug.LogWarning("STATE DOES NOT EXIST");
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void PausedGame()
    {
        SoundManager.Instance.PlaySFX("Button");
        if (currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
            joystick.SetActive(false);
        }
    }
    public void ResumeGame()
    {
        SoundManager.Instance.PlaySFX("Button");
        if (currentState == GameState.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
            joystick.SetActive(true);
        }
    }
    private void CheckPauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PausedGame();
            }
        }
    }
    public void GameOver()
    {
        SoundManager.Instance.PlaySFX("Game Over");
        SoundManager.Instance.musicSource.Stop();
        timeSurvivalDisplay.text = stopwatchDisplay.text;
        resultTitle.text = "Game Over";
        ChangeState(GameState.GameOver);
    }
    private void DisplayResults()
    {
        resultsScreen.SetActive(true);
    }
    public void ResultChosenCharacters(CharacterScriptableObjects characterData)
    {
        chosenCharacters.sprite = characterData.CharacterIcon;
        charactersName.text = characterData.Name;
    }
    public void ChosenWeaponAndPassiveItemUI(List<Image> chosenWeaponData, List<Image> chosenPassiveItemData)
    {
        if (chosenWeaponData.Count != chosenWeaponUI.Count || chosenPassiveItemData.Count != chosenPassiveItemUI.Count)
        {
            Debug.Log("different lengths");
            return;
        }

        for (int i = 0; i < chosenWeaponUI.Count; i++)
        {
            if (chosenWeaponData[i].sprite)
            {
                chosenWeaponUI[i].enabled = true;
                chosenWeaponUI[i].sprite = chosenWeaponData[i].sprite;
            }
            else
            {
                chosenWeaponUI[i].enabled = false;
            }
        }

        for (int i = 0; i < chosenPassiveItemUI.Count; i++)
        {
            if (chosenPassiveItemData[i].sprite)
            {
                chosenPassiveItemUI[i].enabled = true;
                chosenPassiveItemUI[i].sprite = chosenPassiveItemData[i].sprite;
            }
            else
            {
                chosenPassiveItemUI[i].enabled = false;
            }
        }
    }

    public void Victory()
    {
        SoundManager.Instance.PlaySFX("Victory");
        SoundManager.Instance.musicSource.Stop();
        timeSurvivalDisplay.text = stopwatchDisplay.text;
        resultTitle.text = "Victory";
        ChangeState(GameState.GameOver);
    }

    private void UpdateStopwatch()
    {
        stopwatchTime += Time.deltaTime;
        UpdateStopwatchTimeDisplay();

        if (stopwatchTime >= timeLimit)
        {
            playerObject.SendMessage("PlayerVictory");
        }
    }

    private void UpdateStopwatchTimeDisplay()
    {
        int min = Mathf.FloorToInt(stopwatchTime / 60);
        int sec = Mathf.FloorToInt(stopwatchTime % 60);
        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", min, sec);
    }

    public void StartLevelUp()
    {
        SoundManager.Instance.PlaySFX("Level Up");

        ChangeState(GameState.LevelUp);

        pauseBtn.gameObject.SetActive(false);
        joystickFade.alpha = 0f;
        joystickFade.interactable = false;

        playerObject.transform.GetChild(2).SendMessage("RemoveAndApplyUpgrades");
    }
    public void EndLevelUp()
    {
        SoundManager.Instance.PlaySFX("Weapon Up");

        choosingUpgrade = false;
        Time.timeScale = 1f;

        levelUpScreen.SetActive(false);
        pauseBtn.gameObject.SetActive(true);
        joystickFade.alpha = 1f;
        joystickFade.interactable = true;

        ChangeState(GameState.GamePlay);

    }
}
