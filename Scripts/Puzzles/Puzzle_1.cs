using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_1 : Puzzles
{
    // Start is called before the first frame update
    [SerializeField] private Target target1;
    [SerializeField] private Target target2;
    [SerializeField] private Target target3;
    [SerializeField] private Target target4;
    [SerializeField] private Target target5;
    [SerializeField] private Target target6;
    [SerializeField] private Transform[] locations; //enemy spawn locations
    [SerializeField] private Transform[] startPositions; //holds the two start positions for each player
    // Update is called once per frame
    void Start()
    {
        Player_Data.numberOfArrows = 10;
        Player_Data.numberOfArrows_2 = 10;
        Debug.Log(Player_Data.numberOfArrows);
        PositionPlayersStart(startPositions[0], startPositions[1]);
        FindObjectOfType<On_ScreenUI>().ChangeArrows();
        FindObjectOfType<On_ScreenUI>().ChangeArrows2();
    }
    void Update()
    {
        if (target1.activated && target2.activated && target3.activated && target4.activated && target5.activated && target6.activated)
        {
            sCriteria_Met = true;
        }
        else
        {
            sCriteria_Met = false;
        }
        bool coroutine_in_progress = FindObjectOfType<SpawnEnemies>().coroutine_in_progress;
        if (!coroutine_in_progress) //if spawn enemies is not running
        {
            FindObjectOfType<SpawnEnemies>().coroutine_in_progress = true;
            StartCoroutine(FindObjectOfType<SpawnEnemies>().Spawn_Enemies(5, 2, locations));
        }
    }
    private void PositionPlayersStart(Transform startpos1, Transform startpos2)
    {
        Player_Data[] players = (Player_Data[])FindObjectsOfType(typeof(Player_Data));
        players[0].gameObject.transform.position = startpos1.position;
        players[1].gameObject.transform.position = startpos2.position;
    }
}
