using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class On_ScreenUI : MonoBehaviour
{
    public Heart[] hearts_p1;
    public Heart[] hearts_p2;
    public TextMeshProUGUI noArrows_p1;
    public TextMeshProUGUI noArrows_p2;
    // Start is called before the first frame update
    void Start()
    {
        Player_Data.health = 3;
        Player_Data.health_2 = 3;
        HeartsSetup(hearts_p1, true);
        HeartsSetup(hearts_p2, false);
        ChangeArrows();
        ChangeArrows2();
    }
    public void HeartsSetup(Heart[] listOfHearts, bool isPlayerOne)
    {
        float health_p = 0;
        if (isPlayerOne)
        {
            health_p = Player_Data.health; //the player's health stored in player data
        }
        else
        {
            health_p = Player_Data.health_2;
        }
        for (int i = 0; i < 3; i++)
        {
            float health = listOfHearts[i].health;
            if (health_p <= health)
            {
                listOfHearts[i].health = health_p;
                Debug.Log(hearts_p1[i].health);
                listOfHearts[i].GetComponent<Animator>().SetFloat("Health", listOfHearts[i].health);
                break;
            }
            else
            {
                health_p -= health;
                listOfHearts[i].health = 1;
                listOfHearts[i].GetComponent<Animator>().SetFloat("Health", 1);
            }
        }
    }

    // Update is called once per frame
    public void ChangeHearts(float dmg)
    {
        for (int i = 0; i < 3; i++)
        {
            float health = hearts_p1[i].health;
            if (dmg <= health)
            {
                hearts_p1[i].health -= dmg;
                hearts_p1[i].GetComponent<Animator>().SetFloat("Health", hearts_p1[i].health);
                break;
            }
            else
            {
                dmg -= health;
                hearts_p1[i].health = 0;
                hearts_p1[i].GetComponent<Animator>().SetFloat("Health", 0);
            }
        }
    }
    public void ChangeHeartsPlayer2(float dmg)
    {
        for (int i = 0; i < 3; i++)
        {
            float health = hearts_p2[i].health;
            if (dmg <= health)
            {
                hearts_p2[i].health -= dmg;
                hearts_p2[i].GetComponent<Animator>().SetFloat("Health", hearts_p2[i].health);
                break;
            }
            else
            {
                dmg -= health;
                hearts_p2[i].health = 0;
                hearts_p2[i].GetComponent<Animator>().SetFloat("Health", 0);
            }
        }
    }
    public void ChangeArrows()
    {
        float arrow_p1 = Player_Data.numberOfArrows;
        noArrows_p1.text = "X " + arrow_p1; 
    }
    public void ChangeArrows2()
    {
        float arrow_p2 = Player_Data.numberOfArrows_2;
        noArrows_p2.text = "X " + arrow_p2;
    }
}
