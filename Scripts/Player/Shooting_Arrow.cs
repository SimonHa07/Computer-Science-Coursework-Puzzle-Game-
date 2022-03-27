using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting_Arrow : MonoBehaviour
{
    public Animator animator;
    public GameObject arrow_prefab;
    public Transform FirePoint_left;
    public Transform FirePoint_right;
    public Transform FirePoint_up;
    public Transform FirePoint_down;
    public static bool can_shoot = true;
    float direction = 0;

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Player_Data.numberOfArrows);
        if (Input.GetKeyDown("p") && can_shoot && !Pause_Menu.isPaused)
        {
            Debug.Log("shoot");
            animator.SetBool("Shoot", true);
            can_shoot = false;
            Player_Data.numberOfArrows -= 1;
            FindObjectOfType<On_ScreenUI>().ChangeArrows();
        }
    }

    public void Shoot()
    {
        switch (direction)
        {
            case 1: // is left
                Instantiate(arrow_prefab, FirePoint_left.position, FirePoint_left.rotation);
                break;
            case 2: // is right
                Instantiate(arrow_prefab, FirePoint_right.position, FirePoint_right.rotation);
                break;
            case 3: // is up
                Instantiate(arrow_prefab, FirePoint_up.position, FirePoint_up.rotation);
                break;
            case 4: // is down
                Instantiate(arrow_prefab, FirePoint_down.position, FirePoint_down.rotation);
                break;
        }
    }
    public void EndShoot()
    {
        animator.SetBool("Shoot", false);
        if (Player_Data.numberOfArrows <= 0)
        {
            can_shoot = false;
        }
        else
        {
            can_shoot = true;
        }
    }
    public void SetLeft()
    {
        direction = 1;
    }
    public void SetRight()
    {
        direction = 2;
    }
    public void SetUp()
    {
        direction = 3;
    }
    public void SetDown()
    {
        direction = 4;
    }

}
