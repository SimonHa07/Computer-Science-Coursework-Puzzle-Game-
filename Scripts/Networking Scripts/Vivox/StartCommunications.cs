using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Android;
using UnityEngine.UI;
using TMPro;

public class StartCommunications : MonoBehaviour
{
    [SerializeField] private Button LoginButton;
    [SerializeField] private TMP_InputField name_Input;
    [SerializeField] private GameObject LoginScreenUI;
    private bool PermissionsDenied;
    private VivoxVoiceManager _VVManager;
    // Start is called before the first frame update
    private void Awake()
    {
        Setup();
    }
    private void LoginToVivoxService()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            // The user authorized use of the microphone.
            LoginToVivox();
        }
        else
        {
            // Check if the users has already denied permissions
            if (PermissionsDenied)
            {
                PermissionsDenied = false;
                LoginToVivox();
            }
            else
            {
                PermissionsDenied = true;
                // We do not have permission to use the microphone.
                // Ask for permission or proceed without the functionality enabled.
                Permission.RequestUserPermission(Permission.Microphone);
            }
        }
    }

    private void LoginToVivox()
    {
        LoginButton.interactable = false;
        _VVManager.Login(name_Input.text);
    }
    private void Setup()
    {
        _VVManager = VivoxVoiceManager.Instance;
        _VVManager.OnUserLoggedInEvent += OnUserLoggedIn;
        _VVManager.OnUserLoggedOutEvent += OnUserLoggedOut;
        LoginButton?.onClick.AddListener(() =>{ LoginToVivoxService();  });
        NameInputSetup();
        if (_VVManager.LoginState == VivoxUnity.LoginState.LoggedIn)
        {
            gameObject.SetActive(false);
        }
    }
    private void NameInputSetup()
    {
        name_Input.onEndEdit?.AddListener((string name) => { LoginToVivoxService(); Pause_Menu.isPaused = false; });
        name_Input.onSelect.AddListener((string text) => { Pause_Menu.isPaused = true; });
        name_Input.onDeselect.AddListener((string text) => { Pause_Menu.isPaused = false; });
    }
    private void OnUserLoggedIn()
    {
        LoginScreenUI.SetActive(false);
        Debug.Log("uhuhuhuhuhu");
    }
    private void OnUserLoggedOut()
    {
        LoginScreenUI.SetActive(true);
    }
}
