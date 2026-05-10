
using System;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
public class SaveSystem
{
    private static string SavePath => Application.persistentDataPath + "/save.dat";
    
    public static void SaveGame(PlayerHealth playerHealth, LevelManager levelManager)
    {
        try
        {
            SaveData data = new SaveData
            {
                playerHealth = playerHealth.CurrentHealth,
                currentLevel = levelManager.Level,
                entryPoint = levelManager.EntryPoint,
                killedPersistentEnemyIds = levelManager.KilledPersistentEnemyIds.ToArray()
            };

            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(SavePath, FileMode.Create))
            {
                formatter.Serialize(stream, data);
            }
            
            Debug.Log("Game saved successfully!");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save game: {e.Message}");
        }
    }
    
    public static SaveData LoadGame()
    {
        try
        {
            if (!File.Exists(SavePath))
            {
                Debug.Log("No save file found!");
                return null;
            }
            
            BinaryFormatter formatter = new();
            using FileStream stream = new(SavePath, FileMode.Open);
            SaveData data = formatter.Deserialize(stream) as SaveData;
            Debug.Log("Game loaded successfully!");
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load game: {e.Message}");
            return null;
        }
    }
    
    public static bool SaveExists()
    {
        return File.Exists(SavePath);
    }
    
    public static void DeleteSave()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("Save file deleted!");
        }
    }
}