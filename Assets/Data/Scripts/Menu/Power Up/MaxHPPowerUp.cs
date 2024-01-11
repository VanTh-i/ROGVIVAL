using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Threading.Tasks;

public class MaxHPPowerUp : MonoBehaviour
{
    public DependencyStatus dependencyStatus;
    public FirebaseUser User;
    public DatabaseReference DBreference;

    [Header("UI")]
    public ShopScriptableObject upgradeData;
    public TMP_Text upgradeCoin;
    public TMP_Text coin;
    public TMP_Text level;
    public TMP_Text description;
    public TMP_Text upgradeCost;
    public TMP_Text upgradeText;
    public Button upgradeBTN;

    private void Start()
    {
        InitializeFirebase();
    }
    private void InitializeFirebase()
    {
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        if (auth.CurrentUser != null)
        {
            User = auth.CurrentUser;
            LoadPowerUpData();
        }
        else
        {
            Debug.LogWarning("Không có người dùng hiện tại. Yêu cầu đăng nhập.");
        }
    }

    public void UpgradeButton()
    {
        StartCoroutine(GetCurrentAbilitiesLevel());
    }

    public void LoadPowerUpDataButton()
    {
        InitializeFirebase();
        if (User == null)
        {
            Debug.Log("no user");
            return;
        }
        StartCoroutine(LoadPowerUpData());

    }

    private IEnumerator GetCurrentAbilitiesLevel()
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("Max HP Level").GetValueAsync();

        yield return new WaitUntil(() => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning($"Failed to retrieve Abilities with {DBTask.Exception}");
            yield break;
        }

        DataSnapshot snapshot = DBTask.Result;
        if (snapshot != null && snapshot.Value != null)
        {
            int currentLevelAbilities = int.Parse(snapshot.Value.ToString());
            if (currentLevelAbilities < 5)
            {
                StartCoroutine(UpdateAbilities(currentLevelAbilities + 1));
            }
            else
            {
                Debug.LogWarning("Abilities đã đạt giá trị tối đa.");
                upgradeText.text = "Max";
                upgradeCost.gameObject.SetActive(false);
                upgradeBTN.interactable = false;
                // lam mo button
            }
        }
        else
        {
            //StartCoroutine(UpdateAbilities(0));
        }
    }

    private IEnumerator UpdateAbilities(int level)
    {
        if (User != null)
        {
            // Lấy giá trị coins từ Firebase
            var getCoinsTask = DBreference.Child("users").Child(User.UserId).Child("Coin").GetValueAsync();

            yield return new WaitUntil(() => getCoinsTask.IsCompleted);

            if (getCoinsTask.Exception != null)
            {
                Debug.LogWarning($"Failed to get Coins with {getCoinsTask.Exception}");
                yield break;
            }

            DataSnapshot coinSnapshot = getCoinsTask.Result;

            if (coinSnapshot != null && coinSnapshot.Value != null)
            {
                int currentCoins = int.Parse(coinSnapshot.Value.ToString());
                int curupgradeCost = upgradeData.upgradeItems.upgradelevels[level - 1].upgradeCost;

                if (currentCoins >= curupgradeCost)
                {
                    currentCoins -= curupgradeCost;

                    // Cập nhật coins mới vào Firebase
                    Task updateCoinsTask = DBreference.Child("users").Child(User.UserId).Child("Coin").SetValueAsync(currentCoins);

                    yield return new WaitUntil(() => updateCoinsTask.IsCompleted);

                    if (updateCoinsTask.Exception != null)
                    {
                        Debug.LogWarning($"Failed to update Coins with {updateCoinsTask.Exception}");
                        yield break;
                    }

                    //Set the currently logged in user
                    Task DBTask = DBreference.Child("users").Child(User.UserId).Child("Max HP Level").SetValueAsync(level);

                    yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

                    if (DBTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to Abilities task with {DBTask.Exception}");
                    }
                    else
                    {
                        upgradeCoin.text = currentCoins.ToString();
                        coin.text = currentCoins.ToString();
                        // updated
                        this.level.text = "Level: " + level.ToString();
                        description.text = "Player Max Hp: " + upgradeData.upgradeItems.upgradelevels[level].upgradeValue.ToString();
                        upgradeCost.text = upgradeData.upgradeItems.upgradelevels[level].upgradeCost.ToString();

                        float maxHPValue = upgradeData.upgradeItems.upgradelevels[level].upgradeValue;
                        Task updateMaxHpValueTask = DBreference.Child("users").Child(User.UserId).Child("Max HP Value").SetValueAsync(maxHPValue);

                    }
                    yield return new WaitForEndOfFrame();
                }
                else
                {
                    Debug.LogWarning("Không đủ Coins để nâng cấp.");
                }
            }
            else
            {
                Debug.LogWarning("Không có dữ liệu Coins.");
            }


        }
    }

    private IEnumerator LoadPowerUpData()
    {
        //Get the currently logged in user data
        Task<DataSnapshot> DBTask = DBreference.Child("users").Child(User.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        //else if (DBTask.Result.Value == null)
        else if (DBTask.Result.Child("Max HP Level").Value == null)
        {
            DBreference.Child("users").Child(User.UserId).Child("Max HP Level").SetValueAsync(0);
            DBreference.Child("users").Child(User.UserId).Child("Max HP Value").SetValueAsync(0);

            upgradeCoin.text = coin.text;
            level.text = "Level: 0";
            description.text = "Player Max Hp: " + upgradeData.upgradeItems.upgradelevels[0].upgradeValue.ToString();
            upgradeCost.text = upgradeData.upgradeItems.upgradelevels[0].upgradeCost.ToString();
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            upgradeCoin.text = coin.text;

            //level.text = snapshot.Child("Abilities").Value.ToString();
            description.text = snapshot.Child("Max HP Value").Value.ToString();
            string abilitiesValue = snapshot.Child("Max HP Level").Value.ToString();

            level.text = "Level:" + abilitiesValue;

            int abilitiesLevel = int.Parse(abilitiesValue);
            description.text = "Player Max Hp: " + upgradeData.upgradeItems.upgradelevels[abilitiesLevel].upgradeValue.ToString();
            upgradeCost.text = upgradeData.upgradeItems.upgradelevels[abilitiesLevel].upgradeCost.ToString();
        }
    }

}
