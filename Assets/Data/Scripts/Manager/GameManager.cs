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

    public bool isGameOver = false;
    public bool choosingUpgrade;

    public GameObject playerObject;

    public GameState currentState;
    public GameState previousState;

    [Header("UI")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;
    public GameObject levelUpScreen;


    [Header("Stats UI")]
    public TextMeshProUGUI maxHPDisplay;
    public TextMeshProUGUI recoveryDisplay;
    public TextMeshProUGUI strengthDisplay;
    public TextMeshProUGUI armorDisplay;
    public TextMeshProUGUI moveSpeedDisplay;

    [Header("Result UI")]
    public TextMeshProUGUI timeSurvivalDisplay;
    public Image chosenCharacters;
    public TextMeshProUGUI charactersName;
    public List<Image> chosenWeaponUI = new List<Image>(6);
    public List<Image> chosenPassiveItemUI = new List<Image>(6);

    [Header("Stopwatch")]
    public float timeLimit;
    private float stopwatchTime;
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

    private void Update()
    {
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
        if (currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
        }
    }
    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
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
        timeSurvivalDisplay.text = stopwatchDisplay.text;
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

    private void UpdateStopwatch()
    {
        stopwatchTime += Time.deltaTime;
        UpdateStopwatchTimeDisplay();

        if (stopwatchTime >= timeLimit)
        {
            //GameOver();
            // victory, fix late
            playerObject.SendMessage("PlayerDead");
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
        ChangeState(GameState.LevelUp);
        playerObject.transform.GetChild(2).SendMessage("RemoveAndApplyUpgrades");
    }
    public void EndLevelUp()
    {
        choosingUpgrade = false;
        Time.timeScale = 1f;
        levelUpScreen.SetActive(false);
        ChangeState(GameState.GamePlay);
    }


    // private void StatsResult()
    // {
    //     if (Input.GetKey(KeyCode.Tab))
    //     {
    //         if (currentState == GameState.Paused || currentState == GameState.GameOver)
    //         {
    //             return;
    //         }
    //         else
    //         {
    //             statsScreen.SetActive(true);
    //         }
    //     }
    //     else
    //     {
    //         statsScreen.SetActive(false);
    //     }
    // }
}
