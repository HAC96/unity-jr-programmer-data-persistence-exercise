using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;

    public HighScore[] HighScores;
    private static HighScore[] defaultHighScores = new HighScore[]
    {
        new HighScore("ABC", 50),
        new HighScore("ABC", 45),
        new HighScore("ABC", 40),
        new HighScore("ABC", 35),
        new HighScore("ABC", 30),
        new HighScore("ABC", 25),
        new HighScore("ABC", 20),
        new HighScore("ABC", 15),
        new HighScore("ABC", 10),
        new HighScore("ABC", 5)
    };
    public string playerName = "Player";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadHighScores();
        Array.Sort(HighScores);
        Array.Reverse(HighScores); // to get highest first
    }

    public class HighScore : IComparable<HighScore>
    {
        public string name;
        public int score;
        public HighScore(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
        // IComparable so Array.Sort will work
        public int CompareTo(HighScore other) => this.score.CompareTo(other.score);
    }

    public void AddHighScore(int score)
    {
        if (score > HighScores[9].score)
        {
            HighScores[9] = new HighScore(playerName, score);
            Array.Sort(HighScores);
            Array.Reverse(HighScores);
        }
    }

    [Serializable]
    class SaveData
    {
        public string[] names;
        public int[] scores;
        public SaveData(HighScore[] highScores)
        {
            names = new string[highScores.Length];
            scores = new int[highScores.Length];
            for (int i = 0; i < highScores.Length; i++)
            {
                names[i] = highScores[i].name;
                scores[i] = highScores[i].score;
            }
        }
        public HighScore[] ToHighScoreArray()
        {
            HighScore[] highScores = new HighScore[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                highScores[i] = new HighScore(names[i], scores[i]);
            }
            return highScores;
        }
    }

    public void SaveHighScores()
    {
        SaveData data = new SaveData(HighScores);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighScores()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            HighScores = data.ToHighScoreArray();
        }
        else
        {
            HighScores = defaultHighScores;
        }
    }
}
