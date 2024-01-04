using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginUIManager : MonoBehaviour
{
    private static LoginUIManager instance;
    public static LoginUIManager Instance { get => instance; }

    public GameObject loginUI;
    public GameObject registerUI;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        loginUI.SetActive(false);
        registerUI.SetActive(false);
    }

    public void LoginBtn()
    {
        loginUI.SetActive(!loginUI.activeSelf);
    }

    public void LoginScreen()
    {
        loginUI.SetActive(true);
        registerUI.SetActive(false);
    }
    public void RegisterScreen()
    {
        loginUI.SetActive(false);
        registerUI.SetActive(true);
    }
}
