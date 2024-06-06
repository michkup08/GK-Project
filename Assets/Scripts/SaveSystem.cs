using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void Reset()
    {
        Save(1, new int[5] { 0,0,0,0,0 });
    }
    public static void Save (int level, int[] points)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "tarzan.gk";
        FileStream fs = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData( level, points);
        formatter.Serialize(fs, data);
        fs.Close();
    }

    public static PlayerData Load ()
    {
        string path = Application.persistentDataPath + "tarzan.gk";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);
            PlayerData pd = formatter.Deserialize(fs) as PlayerData;
            fs.Close();
            return pd;
        }
        else 
        {
            Debug.Log("Can't detect save file");
            return null;
        }
    }

    public static void initialize()
    {
        PlayerData loaded = Load();
        if (loaded == null)
        {
            int[] pointsData = new int[5];

            for (int i = 0; i < 5; i++)
            {
                pointsData[i] = 0;
            }

            Save(1, pointsData);
        }
    }

    public static void updateLevel(int level, int points)
    {
        PlayerData loaded = Load();
        if(loaded==null)
        {
            int[] pointsData = new int[5];

            for(int i=0;i<5;i++)
            {
                if(i==level-1)
                {
                    pointsData[i] = points;
                }
                else
                {
                    pointsData[i] = 0;
                }
            }

            Save(level, pointsData);
        }
        else
        {
            int currentLevel = (level > loaded.currentLevel) ? level : loaded.currentLevel;

            int[] currentPoints = loaded.points;

            for(int i = 0; i<5; i++ )
            {
                if((i+2)==level)
                {
                    Debug.Log("points updated on level: " + (level-1) );
                    Debug.Log("old: " + currentPoints[i]);
                    currentPoints[i] = currentPoints[i] > points ? currentPoints[i] : points;
                    Debug.Log("new: " + currentPoints[i]);
                }
            }

            Save(currentLevel, currentPoints);
        }
    }
}
