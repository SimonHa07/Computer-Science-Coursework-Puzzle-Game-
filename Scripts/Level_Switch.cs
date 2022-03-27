using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class Level_Switch : MonoBehaviour
{
    public Animator animator;
    public static bool transition = false;
    public static bool transition_anim = false;
    // Start is called before the first frame update
    public void ResetLevel()
    {
        animator.SetTrigger("FadeOut");
        Pause_Menu.isPaused = true;
        //find players and reset their animation
        ResetPlayerAnims();
        transition_anim = true;
    }
    public void OnResetFadeComplete()
    {
        Pause_Menu.isPaused = false;
        if (!Player_Manager.GameOver)
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Debug.Log(Level_Data.ListOfLevels[Level_Data.LevelNo]);

                string path = SceneUtility.GetScenePathByBuildIndex(Level_Data.ListOfLevels[Level_Data.LevelNo]);
                string scene_name = System.IO.Path.GetFileNameWithoutExtension(path);

                NetworkManager.Singleton.SceneManager.LoadScene(scene_name, LoadSceneMode.Single);
                //SceneManager.LoadScene(Level_Data.ListOfLevels[Level_Data.LevelNo]);
            }
        }
        else
        {
            SceneManager.LoadScene("Game Over");
        }
        transition = true;
    }
    public void FadeToNextLevel()
    {
        Debug.Log("fade");
        animator.SetTrigger("FadeOut");
        Pause_Menu.isPaused = true;
        //find players and reset their animation
        ResetPlayerAnims();
        transition_anim = true;
        //FindObjectOfType<Movement>().gameObject.GetComponent<Animator>().SetFloat("Speed", 0);
    }
    public void OnFadeComplete()
    {
        Debug.Log(Level_Data.LevelNo);
        Pause_Menu.isPaused = false;
        if (!Player_Manager.GameOver)
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Debug.Log(Level_Data.ListOfLevels[Level_Data.LevelNo]);

                string path = SceneUtility.GetScenePathByBuildIndex(Level_Data.ListOfLevels[Level_Data.LevelNo]);
                string scene_name = System.IO.Path.GetFileNameWithoutExtension(path);

                NetworkManager.Singleton.SceneManager.LoadScene(scene_name, LoadSceneMode.Single);
                //SceneManager.LoadScene(Level_Data.ListOfLevels[Level_Data.LevelNo]);
            }
        }
        else
        {
            SceneManager.LoadScene("Game Over");
        }
        transition = true;
    }
    public void AllowInput()
    {
        transition = false;
    }
    public void AllowAnimation()
    {
        transition_anim = false;
    }
    public void ResetPlayerAnims()
    {
        Player_Data[] players = (Player_Data[])FindObjectsOfType(typeof(Player_Data));
        players[0].gameObject.GetComponent<Animator>().SetFloat("Speed", 0);
        players[1].gameObject.GetComponent<Animator>().SetFloat("Speed", 0);
    }
    public void PositionPlayersStart(Transform startpos1, Transform startpos2)
    {
        Player_Data[] players = (Player_Data[])FindObjectsOfType(typeof(Player_Data));
        players[0].gameObject.transform.position = startpos1.position;
        players[1].gameObject.transform.position = startpos2.position;
    }
}
