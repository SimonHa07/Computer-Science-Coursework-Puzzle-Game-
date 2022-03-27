using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Data : MonoBehaviour
{
    private int[] EasyLevels = { 1, 2, 3, 4 };
    private int[] MediumLevels = { 5, 6, 7};
    /*private int[] MediumLevels = { 5, 6, 7, 8, 9 };
    private int[] HardLevels = { 10, 11, 12, 13, 14 };
    private int[] VeryHardLevels = { 15, 16, 17, 18, 19 };
    
    public static int[] ListOfLevels = new int[12];
    */
    public static int[] ListOfLevels = new int[7];
    public static int LevelNo; // refers to the index within list of levels
    //public static bool CompletionOfPuzzle1;
    
    void Start()
    {
        string level = "";
        foreach (int lvl in ListOfLevels)
        {
            level += lvl.ToString() + " ";
        }
        Debug.Log(level);
    }
    public void SaveLevel_File()
    {
        Level_Temp level_temp = new Level_Temp(ListOfLevels, LevelNo);
        Saving.SaveLevel(level_temp);
    }
    public void LoadLevel()
    {
        Level_Temp level = Loading.LoadLevelSave();
        ListOfLevels = level.ListOfLevels;
        LevelNo = level.LevelNo;
        //CompletionOfPuzzle1 = level.CompletionOfPuzzle1;
    }
    public void Setup()
    {
        PopulateList_Levels(0, 4, EasyLevels);
        PopulateList_Levels(4, 7, MediumLevels);
        /*
        PopulateList_Levels(7, 10, HardLevels);
        PopulateList_Levels(10, 12, VeryHardLevels);
        */
        //total are 12 levels
    }
    public void PopulateList_Levels(int start, int end, int[] array)
    {
        for (int i = start; i < end; i++)
        {
            bool in_list = true;
            while (in_list)
            {
                in_list = false;
                int random_lvl = Random.Range(0, array.Length);
                if (ListOfLevels != null)
                {
                    foreach (int lvl in ListOfLevels)
                    {
                        if (array[random_lvl] == lvl)
                        {
                            in_list = true;
                            break;
                        }
                    }
                }
                if (!in_list)
                {
                    ListOfLevels[i] = array[random_lvl];
                }
            }
        }
    }
}
