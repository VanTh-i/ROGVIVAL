using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;


public class GetPlayerData : MonoBehaviour
{
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference DBreference;

    private void Start()
    {
        StartCoroutine(CheckAndFixDependenciesAsync());
    }

    private IEnumerator CheckAndFixDependenciesAsync()
    {
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();

        yield return new WaitUntil(() => dependencyTask.IsCompleted);

        dependencyStatus = dependencyTask.Result;
        if (dependencyStatus == DependencyStatus.Available)
        {
            //If they are avalible Initialize Firebase
            InitializeFirebase();
            yield return new WaitForEndOfFrame();
            StartCoroutine(CheckForAutoLogin());
        }
        else
        {
            Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
        }

    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    private IEnumerator CheckForAutoLogin()
    {

        if (User != null)
        {
            Debug.Log("have user");
            var reloadUserTask = User.ReloadAsync();
            yield return new WaitUntil(() => reloadUserTask.IsCompleted);
        }
        else
        {
            Debug.Log("dont have user");
        }
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != User)
        {
            bool signedIn = User != auth.CurrentUser && auth.CurrentUser != null;

            if (!signedIn && User != null)
            {
                Debug.Log("Signed out " + User.UserId);

            }

            User = auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log("Signed in " + User.UserId);
            }
        }
    }

    public void SaveDataButton()
    {
        int stopwatchTimeSave = (int)GameManager.Instance.StopwatchTime;
        int currentCoin = GameManager.Instance.Coin;
        StartCoroutine(UpdateCoin(currentCoin));
        StartCoroutine(CheckAndUpdateTimeSurvive(stopwatchTimeSave));
    }

    private IEnumerator CheckAndUpdateTimeSurvive(int newTime)
    {
        if (User != null)
        {
            DatabaseReference userRef = DBreference.Child("users").Child(User.UserId);

            // Lấy điểm số hiện tại từ Firebase
            Task<DataSnapshot> getSurvivalTimeTask = userRef.Child("Survival time").GetValueAsync();
            yield return new WaitUntil(() => getSurvivalTimeTask.IsCompleted);

            if (getSurvivalTimeTask.Exception != null)
            {
                Debug.LogWarning($"Failed to get Survival time with {getSurvivalTimeTask.Exception}");
                yield break; // Thoát nếu không lấy được điểm số từ Firebase
            }

            // Lấy điểm số hiện tại từ Firebase
            int currentSurvivalTime = 0;
            DataSnapshot snapshot = getSurvivalTimeTask.Result;
            if (snapshot != null && snapshot.Value != null)
            {
                currentSurvivalTime = int.Parse(snapshot.Value.ToString());
            }

            // Kiểm tra điểm số mới và cập nhật nếu nó cao hơn
            if (newTime > currentSurvivalTime)
            {
                yield return StartCoroutine(UpdateTimeSurvive(newTime));
                Debug.Log("cap nhat diem."); // Kiểm tra xem điều kiện này được thực hiện không
            }
            else
            {
                Debug.Log("Không cập nhật điểm.");
            }
            yield return new WaitForEndOfFrame();
            LoadToMenu();

        }
        else
        {
            yield return new WaitForEndOfFrame();
            LoadToMenu();
        }
    }

    private IEnumerator UpdateTimeSurvive(int time)
    {
        //Set the currently logged in user
        Task DBTask = DBreference.Child("users").Child(User.UserId).Child("Survival time").SetValueAsync(time);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Xp is now updated
        }
        yield return new WaitForEndOfFrame();
        LoadToMenu();
    }

    private IEnumerator UpdateCoin(int value)
    {
        if (User != null)
        {
            Task<DataSnapshot> getCoinTask = DBreference.Child("users").Child(User.UserId).Child("Coin").GetValueAsync();
            yield return new WaitUntil(() => getCoinTask.IsCompleted);

            if (getCoinTask.Exception != null)
            {
                Debug.LogWarning($"Failed to get Coin with {getCoinTask.Exception}");
                yield break;
            }

            int currentCoin = 0;
            DataSnapshot snapshot = getCoinTask.Result;
            if (snapshot != null && snapshot.Value != null)
            {
                currentCoin = int.Parse(snapshot.Value.ToString());
            }

            int newCoinValue = currentCoin + value;

            //Set the currently logged in user
            Task DBTask = DBreference.Child("users").Child(User.UserId).Child("Coin").SetValueAsync(newCoinValue);

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else
            {
                //Xp is now updated
            }
        }
        else
        {
            Debug.Log("ko dang nhap user, ko luu tien");
        }
    }

    private void LoadToMenu()
    {
        SceneManager.LoadScene(0);
        SoundManager.Instance.PlaySFX("Button");
        Time.timeScale = 1f;
        SoundManager.Instance.musicSource.Stop();
    }

}
