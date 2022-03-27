using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter_Switch : MonoBehaviour
{
    [SerializeField] private Puzzles puzzle_script;
    private Switch switchObj;
    // Start is called before the first frame update
    void Start()
    {
        switchObj = GetComponent<Switch>();
    }

    // Update is called once per frame
    void Update()
    {
        if (switchObj.activated)
        {
        }
    }
}
