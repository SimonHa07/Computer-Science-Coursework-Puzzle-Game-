using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weight : MonoBehaviour
{
    public bool activated = false;
    public Animator animator;
    [SerializeField] private bool isOrderedWeight;
    [SerializeField] private bool isPuzzle7;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.tag == "Player" || hitInfo.tag == "Block")
        {
            activated = true;
            animator.SetBool("Activate", true);
            if (isOrderedWeight && isPuzzle7)
            {
                FindObjectOfType<Puzzle_7>().EditOrder(gameObject.name);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D hitInfo)
    {
        if (hitInfo.tag == "Player" || hitInfo.tag == "Block")
        {
            activated = false;
            animator.SetBool("Activate", false);
        }
    }
}
