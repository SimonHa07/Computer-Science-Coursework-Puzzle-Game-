using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using SimonHa.Core.Singletons;

public class Switch : NetworkSingleton<Switch>
{
    private bool player_in_range = false;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject object_to_activate;
    [SerializeField] private bool isResetSwitch;
    [SerializeField] private bool isLevel6;
    [SerializeField] private bool isLevel7;
    public bool activated = false;
    /*
    [SerializeField] private NetworkVariable<bool> isActivated = new NetworkVariable<bool>();
    [SerializeField] private NetworkVariable<bool> changeState = new NetworkVariable<bool>();
    
    // Update is called once per frame
    void Update()
    {
        Debug.Log(isActivated.Value);
        if (changeState.Value == true && !IsOwner)
        {
            Debug.Log("boom");
            changeState.Value = false;
            ChangeClientState();
        }
    }
    */
    public void FlipSwitch()
    {
        if (!activated)
        {
            activated = true;
            ActivateObject(object_to_activate, activated);
            if (isLevel7)
            {
                FindObjectOfType<Puzzle_7>().EditOrder(gameObject.name);
            }
        }
        else
        {
            activated = false;
            ActivateObject(object_to_activate, activated);
        }
        if (isResetSwitch)
        {
            if (isLevel6)
            {
                Puzzle_6.counter++; //add one to the amount of times it has been pressed
            }
            FindObjectOfType<Level_Switch>().FadeToNextLevel(); //this might work
            //UpdateSwitchStateServerRpc(activated);  
        }
    }
    /*private void ChangeClientState()
    {
        if (isActivated.Value == true)
        {
            ActivateObject(object_to_activate, isActivated.Value);
        }
        else
        {
            ActivateObject(object_to_activate, isActivated.Value);
        }
    }
    */
    void ActivateObject(GameObject obj, bool activate)
    {
        animator.SetBool("Activate", activate);
        if (obj != null)
        {
            obj.SetActive(activate);
        }
    }    
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.tag == "Player" && hitInfo.gameObject.GetComponent<NetworkObject>().IsOwner)
        {
            player_in_range = true;
            GameObject player = hitInfo.gameObject;
            player.GetComponent<Player_Manager>().interactableObject = gameObject;
        }    
    }
    private void OnTriggerExit2D(Collider2D hitInfo)
    {
        if (hitInfo.tag == "Player" && hitInfo.gameObject.GetComponent<NetworkObject>().IsOwner)
        {
            player_in_range = false;
            GameObject player = hitInfo.gameObject;
            player.GetComponent<Player_Manager>().interactableObject = null;
        }
    }
    
    /*[ServerRpc]
    public void UpdateSwitchStateServerRpc(bool IsActivated)
    {
        Debug.Log("erw");
        isActivated.Value = IsActivated;
        changeState.Value = true;
    }
    */

}
