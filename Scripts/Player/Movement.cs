using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Movement : NetworkBehaviour
{
    public float moveSpeed = 5f;
    private float current_speed = 5f; 
    public Animator animator;
    private Rigidbody2D rb;
    private Vector2 position;
    private Vector2 movement;
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (UI_Manager.continu)
        {
            transform.position = new Vector3(Player_Data.x_pos, Player_Data.y_pos, Player_Data.z_pos);
        }
        UI_Manager.continu = false;
    }

    // Update is called once per frame
    private void Update()
    {
        //transform.position = new Vector3(player_posx.Value, player_posy.Value, -2)
        if (!Pause_Menu.isPaused && !Level_Switch.transition)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            animator.SetFloat("Speed", movement.sqrMagnitude);

            if (Mathf.Abs(movement.y) < 0.9) // if you are not holding up or down
            {
                animator.SetFloat("Horizontal", movement.x);
                animator.SetFloat("Position_y", 0);
                animator.SetFloat("Vertical", 0);
            }
            else if (Mathf.Abs(movement.x) < 0.9) //if you are not holding left or right, then the vertical parameter can be changed
            {
                animator.SetFloat("Vertical", movement.y);
                animator.SetFloat("Position_x", 0);
                animator.SetFloat("Horizontal", 0);
            }
            animator.SetFloat("Position_x", position.x);
            animator.SetFloat("Position_y", position.y);

            //Therefore, if two directions are being held down, only the first one will be recorded (the rest will not update until one is let go)
            if (movement.x == 0 && movement.y == 0) //if no movement then return
            {
                animator.SetFloat("Shoot_x", animator.GetFloat("Position_x"));
                animator.SetFloat("Shoot_y", animator.GetFloat("Position_y"));
            }
            else
            {
                position.x = movement.x;
                position.y = movement.y;
                animator.SetFloat("Shoot_x", animator.GetFloat("Horizontal"));
                animator.SetFloat("Shoot_y", animator.GetFloat("Vertical"));

            }
            if (Mathf.Abs(movement.x) > 0.9 && Mathf.Abs(movement.y) > 0.9)
            {
                current_speed = moveSpeed / 1.5f;
            }
            else
            {
                current_speed = moveSpeed;
            }
        }
    }
    private void FixedUpdate()
    {
        if (!Pause_Menu.isPaused && !Level_Switch.transition)
        {
            rb.MovePosition(rb.position + movement * current_speed * Time.deltaTime);
        }
    }
}
