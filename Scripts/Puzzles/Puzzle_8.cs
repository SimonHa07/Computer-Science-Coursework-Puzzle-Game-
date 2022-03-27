using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_8 : Puzzles
{
    [SerializeField] private Weight weight1;
    [SerializeField] private Weight weight2;
    [SerializeField] private Weight weight3;
    [SerializeField] private Transform[] locations;
    [SerializeField] private float enemy_spawntime = 20;
    [SerializeField] private int enemy_number = 1;
    [SerializeField] private Transform[] startPositions;
    // Update is called once per frame
    void Start()
    {
        Player_Data.numberOfArrows = 5;
        Player_Data.numberOfArrows_2 = 5;
        Debug.Log(Player_Data.numberOfArrows);
        PositionPlayersStart(startPositions[0], startPositions[1]);
        FindObjectOfType<On_ScreenUI>().ChangeArrows();
        FindObjectOfType<On_ScreenUI>().ChangeArrows2();
    }
    void Update()
    {
        if (weight1.activated && weight2.activated && weight3.activated)
        {
            sCriteria_Met = true;
        }
        else
        {
            sCriteria_Met = false;
        }
        bool coroutine_in_progress = FindObjectOfType<SpawnEnemies>().coroutine_in_progress;
        if (!coroutine_in_progress)
        {
            FindObjectOfType<SpawnEnemies>().coroutine_in_progress = true;
            StartCoroutine(FindObjectOfType<SpawnEnemies>().Spawn_Enemies(enemy_spawntime, enemy_number, locations));
        }
    }
    private void PositionPlayersStart(Transform startpos1, Transform startpos2)
    {
        Player_Data[] players = (Player_Data[])FindObjectsOfType(typeof(Player_Data));
        players[0].gameObject.transform.position = startpos1.position;
        players[1].gameObject.transform.position = startpos2.position;
    }
}
