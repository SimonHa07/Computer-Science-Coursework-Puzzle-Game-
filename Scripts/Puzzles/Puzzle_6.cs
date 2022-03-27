using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_6 : Puzzles
{
    // Start is called before the first frame update
    [SerializeField] private Target target1;
    [SerializeField] private Transform[] locations; //enemy spawn locations
    [SerializeField] private Transform[] startPositions; //holds the two start positions for each player
    [SerializeField] private float enemy_spawntime = 20;
    [SerializeField] private int enemy_number = 0;
    public static int counter = 0;
    // Update is called once per frame
    void Start()
    {
        Player_Data.numberOfArrows = 5;
        Player_Data.numberOfArrows_2 = 5;
        PositionPlayersStart(startPositions[0], startPositions[1]);
        FindObjectOfType<On_ScreenUI>().ChangeArrows();
        FindObjectOfType<On_ScreenUI>().ChangeArrows2();
    }
    void Update()
    {
        if (target1.activated && counter >= 1) //the target has been hit and the second switch has been activated more than once
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
