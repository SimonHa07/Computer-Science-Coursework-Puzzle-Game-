using Unity.Netcode;
using Unity.Netcode.Samples;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(ClientNetworkTransform))]
public class NetworkMovement : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private float current_speed = 5f;
    public Animator animator;

    [SerializeField] private NetworkVariable<Vector2> movementVector = new NetworkVariable<Vector2>();

    [SerializeField] private Rigidbody2D rb;

    private Vector2 position;
    private Vector2 movement;
    private Vector2 old_movement;
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
        if (IsClient && IsOwner)
        {
            ClientMovementInput();

            NetworkCamera.Instance.FollowPlayer(transform);
            ClientAnimation(movement);
        }
        if (!IsOwner)
        {
            ClientAnimation(movementVector.Value);
        }
    }
    private void FixedUpdate()
    {
        if (!Pause_Menu.isPaused && !Level_Switch.transition && !Dialogue_Trigger.isDialogueTriggered)
        {
            rb.MovePosition(rb.position + movement * current_speed * Time.deltaTime);
        }
    }
    private void ClientMovementInput()
    {
        if (!Pause_Menu.isPaused && !Level_Switch.transition && !Dialogue_Trigger.isDialogueTriggered)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (Mathf.Abs(movement.x) > 0.9 && Mathf.Abs(movement.y) > 0.9)
            {
                current_speed = moveSpeed / 1.5f;
            }
            else
            {
                current_speed = moveSpeed;
            }
            if (movement != old_movement)
            {
                old_movement = movement;
                UpdateClientMovementVectorServerRpc(movement);
            }
        }
        if (Pause_Menu.isPaused || Dialogue_Trigger.isDialogueTriggered)
        {
            Debug.Log("dialogue");
            animator.SetFloat("Speed", 0);
        }
    }
    private void ClientAnimation(Vector2 movement)
    {
        if (!Level_Switch.transition_anim && !Dialogue_Trigger.isDialogueTriggered)
        {
            if (!Level_Switch.transition)
            {
                animator.SetFloat("Speed", movement.sqrMagnitude);
            }

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
        }
    }  
    /*private void FixedUpdate()
    {
        if (!Pause_Menu.isPaused && !Level_Switch.transition)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
        }
        [SerializeField] private NetworkVariable<float> playerPosx = new NetworkVariable<float>();
        [SerializeField] private NetworkVariable<float> playerPosy = new NetworkVariable<float>();
            playerPosx.Value = movement.x;
        playerPosy.Value = movement.y;

            if (playerPosx.Value != 0 || playerPosy.Value != 0)
        {
            transform.position = new Vector3(transform.position.x + playerPosx.Value * current_speed * Time.deltaTime, 
                transform.position.y + playerPosy.Value * current_speed * Time.deltaTime, -1);
        }
    }
    private void ClientUpdatePosition()
    {
        if (playerPosx.Value != 0 || playerPosy.Value != 0)
        {
            transform.position = new Vector3(transform.position.x + playerPosx.Value * current_speed * Time.deltaTime,
                transform.position.y + playerPosy.Value * current_speed * Time.deltaTime, -1);
        }
    }
    */
    [ServerRpc]
    public void UpdateClientMovementVectorServerRpc(Vector2 movement)
    {
        movementVector.Value = movement;
    }
}
