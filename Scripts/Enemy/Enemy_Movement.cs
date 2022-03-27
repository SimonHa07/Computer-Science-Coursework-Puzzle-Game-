using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public Animator animator;
    Vector2 movement;
    Vector2 position;
    Transform target;
    float move_speed = 2.5f;
    // Update is called once per frame
    void Start()
    {
        Player_Data[] players = (Player_Data[])FindObjectsOfType(typeof(Player_Data));
        player1 = players[0].gameObject.transform;
        player2 = players[1].gameObject.transform;
    }
    void Update()
    {
        if (!Player_Manager.GameOver)
        {
            if (Vector2.Distance(player1.position, gameObject.transform.position) < Vector2.Distance(player2.position, gameObject.transform.position))
            //which player is closest to the enemy
            {
                target = player1;
            }
            else
            {
                target = player2;
            }
            Follow(target);
            //animation code
            movement.x = target.position.x - transform.position.x;
            movement.y = target.position.y - transform.position.y;
            movement = movement.normalized; // I only need the direction the player is in within -1 to 1

            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);

            animator.SetFloat("Position_x", position.x);
            animator.SetFloat("Position_y", position.y);

            if (movement.x == 0 && movement.y == 0) //if no movement then return
            {
                return;
            }
            else
            {
                position.x = movement.x;
                position.y = movement.y;
            }
        }
    }
    public void Follow(Transform target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, move_speed * Time.deltaTime);
    }
}
