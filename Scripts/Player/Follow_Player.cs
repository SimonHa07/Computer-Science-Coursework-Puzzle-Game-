using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Player : MonoBehaviour
{
    public GameObject target;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if (!Player_Manager.GameOver)
        {
            transform.position = target.transform.position;
        }
    }
}
