using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Level_Temp
{
    public int[] ListOfLevels;
    public int LevelNo;
    //public bool CompletionOfPuzzle1;

    public Level_Temp(int[] my_levelList, int my_levelno)
    {
        ListOfLevels = my_levelList;
        LevelNo = my_levelno;
        //CompletionOfPuzzle1 = my_puzzle1;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
