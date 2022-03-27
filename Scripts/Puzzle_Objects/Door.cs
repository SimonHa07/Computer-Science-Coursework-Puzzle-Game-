using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using SimonHa.Core.Singletons;
public class Door : NetworkBehaviour
{
    [SerializeField] private BoxCollider2D boxcollider;
    [SerializeField] private Animator animator;
    /*
    [SerializeField] private NetworkVariable<bool> isOpened = new NetworkVariable<bool>();

    private void Update()
    {
        if (isOpened.Value == true && !IsOwner)
        {
            Debug.Log("boom");
            OpenDoor();
            isOpened.Value = false;
        }
    }
    */
    public void OpenDoor()
    {
        boxcollider.isTrigger = true; // disable box collider
        animator.SetBool("Open", true);
        Debug.Log("hey");
        //UpdateDoorStateServerRpc(true);
    }
    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.tag == "Player")
        {
            Level_Data.LevelNo += 1;
            FindObjectOfType<Level_Switch>().FadeToNextLevel();
            /*if (NetworkManager.IsHost)
            {
                FindObjectOfType<Level_Switch>().FadeToNextLevel();
            }
            else
            {
                FindObjectOfType<Level_Switch>().ResetPlayerAnims();
                Level_Switch.transition = true;
            }
            */
        }
    }
    
    /*[ServerRpc]
    public void UpdateDoorStateServerRpc(bool isOpen)
    {
        isOpened.Value = isOpen;
    }
    */
}
