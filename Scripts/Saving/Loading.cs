using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Loading : MonoBehaviour
{ 
    public static Player_Temp LoadPlayerSave() // it returns a object of class Player_Temp
    {
        string filepath = Application.persistentDataPath + "/player_save.txt";
        if (File.Exists(filepath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream datastream = new FileStream(filepath, FileMode.Open); //create a file stream towards the filepath that is set to type Open, i.e, read the file
            Player_Temp saved_player = formatter.Deserialize(datastream) as Player_Temp; //decipher the data into a class of player_temp
            datastream.Close(); // close datastream
            return saved_player;
        }
        else
        {
            Debug.Log("The File was not found");  
            return null;
        }
    }
    public static Level_Temp LoadLevelSave() // returns an object of class Level_temp
    {
        string filepath = Application.persistentDataPath + "/level_save.txt"; // what the file is called
        if (File.Exists(filepath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream datastream = new FileStream(filepath, FileMode.Open);
            Level_Temp saved_level = formatter.Deserialize(datastream) as Level_Temp;
            datastream.Close();
            return saved_level;
        }
        else
        {
            Debug.Log("The File was not found");
            return null;
        }
    }
}
