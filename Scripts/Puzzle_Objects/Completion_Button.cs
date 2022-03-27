using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Completion_Button : NetworkBehaviour
{
    private bool player_in_range = false;
    private bool pressed = false;
    public Animator animator;
    public Puzzles puzzle_script;
    [SerializeField] private bool isOrderedButton;
    [SerializeField] private bool isPuzzle7;

    void Update()
    {
    }
    public void PressButton()
    {
        if (!pressed)
        {
            if (puzzle_script.sCriteria_Met)
            {
                pressed = true;
                animator.SetBool("Pressed", true);
                FindObjectOfType<Door>().OpenDoor();
            }
            else
            {
                animator.SetBool("Pressed", true);
                StartCoroutine(WaitButton());
            }
            if (isOrderedButton && isPuzzle7)
            {
                FindObjectOfType<Puzzle_7>().EditOrder(gameObject.name);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D hitinfo)
    {
        if (hitinfo.tag == "Player" && hitinfo.gameObject.GetComponent<NetworkObject>().IsOwner)
        {
            player_in_range = true;
            GameObject player = hitinfo.gameObject;
            player.GetComponent<Player_Manager>().interactableObject = gameObject;
        }
    }
    void OnTriggerExit2D(Collider2D hitinfo)
    {
        if (hitinfo.tag == "Player" && hitinfo.gameObject.GetComponent<NetworkObject>().IsOwner)
        {
            player_in_range = false;
            GameObject player = hitinfo.gameObject;
            player.GetComponent<Player_Manager>().interactableObject = null;
        }
    }
    IEnumerator WaitButton()
    {
        yield return new WaitForSeconds(1);
        animator.SetBool("Pressed", false);
    }
}
