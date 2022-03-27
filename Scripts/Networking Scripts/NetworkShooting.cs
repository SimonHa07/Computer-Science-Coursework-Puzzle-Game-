using Unity.Netcode;
using UnityEngine;
using SimonHa.Core.Singletons;

public class NetworkShooting : NetworkSingleton<NetworkShooting>
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject arrow_prefab;
    [SerializeField] private Transform FirePoint_left;
    [SerializeField] private Transform FirePoint_right;
    [SerializeField] private Transform FirePoint_up;
    [SerializeField] private Transform FirePoint_down;
    //arrow variables
    /*[SerializeField] private NetworkVariable<Vector3> arrow_shot_position = new NetworkVariable<Vector3>();
    [SerializeField] private NetworkVariable<Quaternion> arrow_shot_rotation = new NetworkVariable<Quaternion>();
    [SerializeField] private NetworkVariable<bool> arrow_beenShot = new NetworkVariable<bool>(); //for the client to shoot
    */
    [SerializeField] private NetworkVariable<bool> player_startShotAnim = new NetworkVariable<bool>();
    public static bool can_shoot = true;
    float direction = 0;
    private bool old_shotAnim;

    // Update is called once per frame
    private void Update()
    {
        //Debug.Log(Player_Data.numberOfArrows);

        if (IsClient && IsOwner)
        {
            ClientShootInput();
        }
        if (!IsOwner && player_startShotAnim.Value == true)
        {
            if (old_shotAnim != player_startShotAnim.Value)
            {
                Debug.Log("shoting");
                ClientShotAnimation(player_startShotAnim.Value);
                Player_Data.numberOfArrows_2--;
                FindObjectOfType<On_ScreenUI>().ChangeArrows2();
            }
        }
        if (old_shotAnim != player_startShotAnim.Value)
        {
            old_shotAnim = player_startShotAnim.Value;
        }
    }
    private void ClientShootInput()
    {
        if (Input.GetKeyDown("p") && can_shoot && !Pause_Menu.isPaused && !Dialogue_Trigger.isDialogueTriggered)
        {
            Debug.Log("shoot");
            can_shoot = false;
            Player_Data.numberOfArrows -= 1;
            animator.SetBool("Shoot", true);
            UpdateShotAnimServerRpc(true);
            FindObjectOfType<On_ScreenUI>().ChangeArrows();
        }
    }
    private void ClientShotAnimation(bool animshoot)
    {
        animator.SetBool("Shoot", animshoot);
    }
    public void Shoot()
    {
        Debug.Log("Shot");
        switch (direction)
        {
            case 1: // is left
                InstantiateArrow(FirePoint_left.position, FirePoint_left.rotation);
                break;
            case 2: // is right
                InstantiateArrow(FirePoint_right.position, FirePoint_right.rotation);
                break;
            case 3: // is up
                InstantiateArrow(FirePoint_up.position, FirePoint_up.rotation);
                break;
            case 4: // is down
                InstantiateArrow(FirePoint_down.position, FirePoint_down.rotation);
                break;
        }
    }
    private void InstantiateArrow(Vector3 position, Quaternion rotation)
    {
        Instantiate(arrow_prefab, position, rotation);
    }
    public void EndShoot()
    {
        animator.SetBool("Shoot", false);
        if (IsOwner)
        {
            UpdateShotAnimServerRpc(false);
        }
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
    /*[ServerRpc]
    public void ShootArrowServerRpc(Vector3 position, Quaternion rotation)
    {
        arrow_shot_position.Value = position;
        arrow_shot_rotation.Value = rotation;
        arrow_beenShot.Value = true;
    }
    */
    [ServerRpc]
    public void UpdateShotAnimServerRpc(bool boolean)
    {
        player_startShotAnim.Value = boolean;
    }
}
