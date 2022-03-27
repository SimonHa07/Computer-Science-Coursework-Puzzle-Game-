using UnityEngine;
using Unity.Netcode;

public class Dialogue_Trigger : MonoBehaviour
{
    public float dialoguecount = 0f;
    private bool playerinrange = false;
    public Dialogue dialogue;
    public static bool isDialogueTriggered = false;
    void Update()
    {
        if (Input.GetKeyDown("o") && playerinrange == true && !Pause_Menu.isPaused) //if the player is in the range and the game isn't paused
        {
            isDialogueTriggered = true; //the player can't move
            if (dialoguecount == 0)
            {
                TriggerDialogue();
            }
            else
            {
                FindObjectOfType<Dialogue_Manager>().DisplayNextSentence();
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && other.GetComponent<NetworkObject>().IsOwner)
        {
            playerinrange = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        playerinrange = false;
    }
    public void TriggerDialogue()
    {
        FindObjectOfType<Dialogue_Manager>().StartDialogue(dialogue);    // start the dialogue
        dialoguecount = 1;  // sets dialogue count to 1, so that we display the next sentence when the user presses 'o'
    }

}
