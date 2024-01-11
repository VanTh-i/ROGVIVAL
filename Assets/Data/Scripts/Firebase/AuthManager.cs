using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.Linq;

public class AuthManager : MonoBehaviour
{
    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference DBreference;

    //Login variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    //Register variables
    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Text warningRegisterText;

    [Header("UI")]
    public Button loginBtn;
    public Button logoutBtn;
    public GameObject profile;

    [Header("UserData")]
    public TMP_Text usernameText;
    public TMP_Text userCoin;
    public TMP_Text timeField;
    public GameObject scoreElement;
    public Transform scoreboardContent;

    private void Start()
    {
        StartCoroutine(CheckAndFixDependenciesAsync());
    }

    void Update()
    {
        if (auth != null && User != null)
        {
            loginBtn.gameObject.SetActive(false);
            logoutBtn.gameObject.SetActive(true);
            profile.gameObject.SetActive(true);
        }
        else
        {
            loginBtn.gameObject.SetActive(true);
            logoutBtn.gameObject.SetActive(false);
            profile.gameObject.SetActive(false);
        }
    }

    public void LoadUserDataBtn()
    {
        if (User == null) return; //todo: sua lai khong cho an vao khi chua co nguoi dung
        StartCoroutine(LoadUserData());
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
            Debug.Log("auto log");
            var reloadUserTask = User.ReloadAsync();
            yield return new WaitUntil(() => reloadUserTask.IsCompleted);
            AutoLogin();
        }
        else
        {
            Debug.Log("not auto log");
            //LoginUIManager.Instance.LoginScreen();
        }
    }

    private void AutoLogin()
    {
        if (User != null)
        {
            LogToTitleScreen();
        }
        else
        {
            //LoginUIManager.Instance.LoginScreen();
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
                usernameText.text = User.DisplayName;
            }
        }
    }

    public void ClearLoginFeilds()
    {
        emailLoginField.text = "";
        passwordLoginField.text = "";
        warningLoginText.text = "";
        confirmLoginText.text = "";
    }
    public void ClearRegisterFeilds()
    {
        usernameRegisterField.text = "";
        emailRegisterField.text = "";
        passwordRegisterField.text = "";
        passwordRegisterVerifyField.text = "";
        warningRegisterText.text = "";
    }

    // public void SaveDataButton()
    // {
    //     int stopwatchTimeSave = (int)GameManager.Instance.StopwatchTime;
    //     StartCoroutine(UpdateTimeSurvive(stopwatchTimeSave));
    // }

    public void ScoreboardButton()
    {
        StartCoroutine(LoadScoreboardData());
    }

    public void LogoutButton()
    {
        if (auth != null && User != null)
        {
            auth.SignOut();
            ClearRegisterFeilds();
            ClearLoginFeilds();
        }
    }

    //Function for the login button
    public void LoginButton()
    {
        //Call the login coroutine passing the email and password
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }
    //Function for the register button
    public void RegisterButton()
    {
        //Call the register coroutine passing the email, password, and username
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
        LoadUserData();
    }

    private IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        Task<AuthResult> LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            warningLoginText.text = message;
        }
        else
        {
            //User is now logged in
            //Now get the result
            User = LoginTask.Result.User;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            warningLoginText.text = "";
            confirmLoginText.text = "Logged In";
            LoadUserDataBtn();
            LogToTitleScreen();
        }
    }

    private void LogToTitleScreen()
    {
        LoginUIManager.Instance.loginUI.gameObject.SetActive(false);
        LoginUIManager.Instance.registerUI.gameObject.SetActive(false);
        StartCoroutine(UpdateUsernameAuth(usernameText.text));
        StartCoroutine(UpdateUsernameDatabase(usernameText.text));
        StartCoroutine(LoadUserData());
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            //If the username field is blank show a warning
            warningRegisterText.text = "Missing Username";
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            //If the password does not match show a warning
            warningRegisterText.text = "Password Does Not Match!";
        }
        else
        {
            //Call the Firebase auth signin function passing the email and password
            Task<AuthResult> RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                warningRegisterText.text = message;
            }
            else
            {
                //User has now been created
                //Now get the result
                User = RegisterTask.Result.User;

                if (User != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    //Call the Firebase auth update user profile function passing the profile with the username
                    Task ProfileTask = User.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningRegisterText.text = "Username Set Failed!";
                    }
                    else
                    {
                        //Username is now set
                        //Now return to login screen
                        warningRegisterText.text = "Successful";
                        usernameText.text = User.DisplayName;
                        LogToTitleScreen();
                    }
                }
            }
        }
    }

    private IEnumerator UpdateUsernameAuth(string _username)
    {
        //Create a user profile and set the username
        UserProfile profile = new UserProfile { DisplayName = _username };

        //Call the Firebase auth update user profile function passing the profile with the username
        Task ProfileTask = User.UpdateUserProfileAsync(profile);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
        }
        else
        {
            //Auth username is now updated
        }
    }

    private IEnumerator UpdateUsernameDatabase(string _username)
    {
        //Set the currently logged in user username in the database
        Task DBTask = DBreference.Child("users").Child(User.UserId).Child("username").SetValueAsync(_username);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Database username is now updated
        }
    }

    private IEnumerator LoadUserData()
    {
        //Get the currently logged in user data
        Task<DataSnapshot> DBTask = DBreference.Child("users").Child(User.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        //else if (DBTask.Result.Value == null)
        else if (DBTask.Result.Child("Coin").Value == null && DBTask.Result.Child("Survival time").Value == null)
        {
            //DataSnapshot snapshot = DBTask.Result;
            //No data exists yet
            timeField.text = "00:00";
            userCoin.text = "0";
            //userCoin.text = snapshot.Child("Coin").Value.ToString();
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            string timeSurvFormat = snapshot.Child("Survival time").Value.ToString();
            int min = Mathf.FloorToInt(Convert.ToSingle(timeSurvFormat) / 60);
            int sec = Mathf.FloorToInt(Convert.ToSingle(timeSurvFormat) % 60);
            timeField.text = string.Format("{0:00}:{1:00}", min, sec);
            userCoin.text = snapshot.Child("Coin").Value.ToString();
        }
    }

    private IEnumerator LoadScoreboardData()
    {
        //Get all the users data ordered by kills amount
        Task<DataSnapshot> DBTask = DBreference.Child("users").OrderByChild("Survival time").LimitToFirst(11).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            //Destroy any existing scoreboard elements
            foreach (Transform child in scoreboardContent.transform)
            {
                Destroy(child.gameObject);
            }

            //Loop through every users UID
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                if (childSnapshot.HasChild("Survival time") && childSnapshot.Child("Survival time").Exists)
                {
                    string username = childSnapshot.Child("username").Value.ToString();
                    float survivalTime = int.Parse(childSnapshot.Child("Survival time").Value.ToString());

                    //Instantiate new scoreboard elements
                    GameObject scoreboardElement = Instantiate(scoreElement, scoreboardContent);
                    scoreboardElement.GetComponent<LeaderboardElement>().NewScoreElement(username, survivalTime);
                }
            }
        }
    }
}
