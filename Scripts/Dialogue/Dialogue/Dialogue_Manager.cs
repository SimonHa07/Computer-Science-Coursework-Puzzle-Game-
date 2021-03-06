using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Dialogue_Manager : MonoBehaviour
{
    public Animator animator;
    public TextMeshProUGUI dialogueText;
    private Queue <string>sentences;
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }
    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("isOpen", true);
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();

    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(0.03f);

        }
    }
    void EndDialogue()
    {
        animator.SetBool("isOpen", false);
        Dialogue_Trigger.isDialogueTriggered = false;
        FindObjectOfType<Dialogue_Trigger>().dialoguecount = 0;
    }
}
