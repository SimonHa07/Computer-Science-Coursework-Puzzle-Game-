using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using UnityEngine.UI;
using System.Threading.Tasks;
public class UI_Manager : MonoBehaviour
{
    public static bool continu = false;
    //public static bool quitMain = false;
    // Start is called before the first frame update
    private void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            QuitMain();
        };
    }
    public void NewGame()
    {
        FindObjectOfType<Level_Data>().Setup();
        FindObjectOfType<Level_Switch>().FadeToNextLevel(); 
    }
    public void Continue()
    {
        //FindObjectOfType<Player_Data>().LoadPlayer();
        FindObjectOfType<Level_Data>().LoadLevel();
        continu = true;
        //string current_level = Level_Data.ListOfLevels[Level_Data.LevelNo];
        //SceneManager.LoadScene(current_level, LoadSceneMode.Single); // loads the stage that is in the save data
        FindObjectOfType<Level_Switch>().FadeToNextLevel();
    }
    public void QuitMain() // quit the main game, and enter the main menu
    {
        Pause_Menu.isPaused = false;
        Time.timeScale = 1;
        if (NetworkManager.Singleton.IsHost)
        {
            //FindObjectOfType<Player_Data>().SavePlayer_File();
            FindObjectOfType<Level_Data>().SaveLevel_File();
            PlayerNetworkManager.Instance.PlayersinGame = 0; // this is needed for the start game button to work
        }
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
    public void Quit() // quit the game
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.Shutdown();
        }
        Application.Quit();
    }
}
