using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Special_Target : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private bool isActivated;
    [SerializeField] private Target targetToActivate;
    void Start()
    {
        if (isActivated) //if the target should start out activated
        {
            GetComponent<Target>().ActivateTarget(); //activate the target
        }
    }
    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.tag == "Arrow")
        {
            targetToActivate.ActivateTarget(); //activate the other target
        }

    }
}
