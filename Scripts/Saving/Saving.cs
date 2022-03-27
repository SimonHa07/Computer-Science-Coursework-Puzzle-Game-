using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Saving : MonoBehaviour
{
    public static void SavePlayer(Player_Temp player_temp)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string filepath = Application.persistentDataPath + "/player_save.txt";
        FileStream datastream = new FileStream(filepath, FileMode.Create);
        formatter.Serialize(datastream, player_temp);
        datastream.Close();
    }
    public static void SaveLevel(Level_Temp level_temp)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string filepath = Application.persistentDataPath + "/level_save.txt";
        FileStream datastream = new FileStream(filepath, FileMode.Create);
        formatter.Serialize(datastream, level_temp);
        datastream.Close();
    }
}
