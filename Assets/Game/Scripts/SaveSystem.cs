using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    private static PlayerData playerData = null;

    public static PlayerData PlayerData { get => playerData; }

    public static void SavePlayerData()
    {
        if (playerData == null)
        {
            Debug.LogError("Cannot save null data!");
            return;
        }
        Debug.Log("Saving...");
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, playerData);
        stream.Close();
    }

    public static void LoadPlayerData(Action Callback)
    {
        string path = Application.persistentDataPath + "/player.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            stream.Position = 0;
            playerData = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
        }
        else
        {
            Debug.Log("Save file not found. Created new.");
            playerData = new PlayerData();
            SavePlayerData();
        }
        Callback();
    }
}
