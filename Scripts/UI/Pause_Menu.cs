using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class Pause_Menu : NetworkBehaviour
{
    [SerializeField] private GameObject pause_menu;
    [SerializeField] private GameObject options_menu;
    [SerializeField] private GameObject background;
    [SerializeField] private Button resumeButton;
    public static bool isPaused = false;
    bool is_open = false;
    // Start is called before the first frame update
    // Update is called once per frame
    private void Start()
    {
        //gameObject.GetComponent<NetworkObject>().Spawn();
    }
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (is_open)
            {
                HideMenus();
            }
            else
            {
                ShowPauseMenu();
            }
        }
    }
    public void ShowPauseMenu()
    {
        is_open = true;
        isPaused = true;
        pause_menu.SetActive(true);
        background.SetActive(true);
        resumeButton.Select();
       // Time.timeScale = 0;
    }
    public void HideMenus()
    {
        is_open = false;
        isPaused = false;
        pause_menu.SetActive(false);
        options_menu.SetActive(false);
        background.SetActive(false);
        //Time.timeScale = 1;
    }
}
