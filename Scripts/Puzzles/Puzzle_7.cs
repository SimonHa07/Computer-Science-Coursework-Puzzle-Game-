using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Puzzle_7 : Puzzles
{
    public string[] orderList1 = new string[3]; //stores the first order
    private int list1Counter = 0;
    private string[] rightOrder1 = new string[] {"Weight1", "Completion Button", "Weight2" }; //correct order
    public string[] orderList2 = new string[2];
    private int list2Counter = 0;
    private string[] rightOrder2 = new string[] { "Target1", "Switch" }; //other correct order

    [SerializeField] private Transform[] locations;
    [SerializeField] private float enemy_spawntime = 20;
    [SerializeField] private int enemy_number = 1;
    [SerializeField] private Transform[] startPositions;
    // Update is called once per frame
    void Start()
    {
        Setup();
    }
    void Update()
    {
        if (Enumerable.SequenceEqual(orderList1, rightOrder1) && Enumerable.SequenceEqual(orderList2, rightOrder2))
        {
            //if orderlist1 is equal to the right order, and orderlist2 is right too
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
    public void EditOrder(string name)
    {
        if (Array.Exists(rightOrder1, x => x== name)) //if name of obj is in rightOrder1
        {
            if (list1Counter != orderList1.Length) //the list isnt full
            {
                orderList1[list1Counter] = name; //add to list
                list1Counter++;
            }
            else
            {
                Array.Clear(orderList1, 0, orderList1.Length); // clears the list if it is full
                list1Counter = 0; //resets counter
            }
        }
        if (Array.Exists(rightOrder2, x => x == name))
        {
            if (list2Counter != orderList2.Length)
            {
                orderList2[list2Counter] = name;
                list2Counter++;
            }
            else
            {
                Array.Clear(orderList2, 0, orderList2.Length); // clears the list if it is full
                list2Counter = 0;
            }
        }
    }
    private void PositionPlayersStart(Transform startpos1, Transform startpos2)
    {
        Player_Data[] players = (Player_Data[])FindObjectsOfType(typeof(Player_Data));
        players[0].gameObject.transform.position = startpos1.position;
        players[1].gameObject.transform.position = startpos2.position;
    }
    private void Setup()
    {
        Player_Data.numberOfArrows = 5;
        Player_Data.numberOfArrows_2 = 5;
        PositionPlayersStart(startPositions[0], startPositions[1]);
        FindObjectOfType<On_ScreenUI>().ChangeArrows();
        FindObjectOfType<On_ScreenUI>().ChangeArrows2();

    }
}
