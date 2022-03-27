using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    // Start is called before the first frame update
    public bool activated = false;
    public Animator animator;
    [SerializeField] private bool isLevel7;
    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.tag == "Arrow")
        {
            ActivateTarget(); // activates the target
            if (isLevel7)
            {
                FindObjectOfType<Puzzle_7>().EditOrder(gameObject.name);
            }
        }
        
    }
    public void ActivateTarget()
    {
        if (!activated)
        {
            activated = true;
            animator.SetBool("Activated", true);
        }
        else
        {
            activated = false;
            animator.SetBool("Activated", false);
        }
    }
}
