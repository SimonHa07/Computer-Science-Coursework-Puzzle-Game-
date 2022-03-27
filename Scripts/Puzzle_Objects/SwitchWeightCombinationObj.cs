using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWeightCombinationObj : MonoBehaviour
{
    [SerializeField] private Switch switchObj;
    [SerializeField] private Weight weightObj;
    [SerializeField] private GameObject object_to_activate;

    void Update()
    {
        if (switchObj.activated == true && weightObj.activated)
        {
            object_to_activate.SetActive(true);
        }
    }

}
