using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Data : MonoBehaviour
{
    public static float numberOfArrows;
    public static float numberOfArrows_2;
    public static float health;
    public static float health_2;
    public static float x_pos;
    public static float y_pos;
    public static float z_pos;

    public void SavePlayer_File()
    {
        x_pos = transform.position.x;
        y_pos = transform.position.y;
        z_pos = transform.position.z;
        Player_Temp player_temp = new Player_Temp(numberOfArrows, health, x_pos, y_pos, z_pos);
        Debug.Log("Saving");
        Saving.SavePlayer(player_temp);
    }
    public void LoadPlayer()
    {
        Debug.Log("hello");
        Player_Temp player = Loading.LoadPlayerSave();
        health = player.health;
        numberOfArrows = player.numberOfArrows;
        x_pos = player.x_pos;
        y_pos = player.y_pos;
        z_pos = player.z_pos;
    }
}
