using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death_Trigger : MonoBehaviour
{
    public void Death()
    {
        Destroy(gameObject);
        Debug.Log("GameOver");
        Player_Manager.GameOver = true;
        FindObjectOfType<Level_Switch>().FadeToNextLevel();
    }
}
