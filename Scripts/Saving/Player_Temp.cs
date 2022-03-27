using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Player_Temp
{
    public float numberOfArrows;
    public float health;
    public float x_pos;
    public float y_pos;
    public float z_pos;

public Player_Temp(float my_arrows, float my_health, float my_xpos, float my_ypos, float my_zpos)
    {
        numberOfArrows = my_health;
        health = my_health;
        x_pos = my_xpos;
        y_pos = my_ypos;
        z_pos = my_zpos;
    }
}
