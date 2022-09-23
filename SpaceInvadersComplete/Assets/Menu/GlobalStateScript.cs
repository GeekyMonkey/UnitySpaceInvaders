using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GlobalStateScript : MonoBehaviour
{
    public static GlobalStateScript Instance;

    internal string PlayerName = "Player A";

    internal HighScoreEntries HighScoreData;
    private string FileName = "C:/temp/HighScores.json";

    private void Awake()
    {
        //Debug.Log("Global State Awake");
        Cursor.visible = false;

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadScores();
    }


    public void AddHighScore(string name, int score)
    {
        // Ingore mentors & testers;
        string[] IgnorePlayers = new string[]{
            "Player A",
            "Quinn",
            "Russ"
            };
        if (IgnorePlayers.Any((n) => n.ToLower() == name.ToLower()))
        {
            // Play testing
            return;
        }

        DateTime now = DateTime.Now;
        long unixTime = ((DateTimeOffset)now).ToUnixTimeSeconds();

        var newHighScore = new HighScoreEntry
        {
            Name = name,
            Score = score,
            Timestamp = unixTime
        };
        LoadScores();
        HighScoreData.HighScores.Add(newHighScore);

        string json = JsonUtility.ToJson(HighScoreData);
        var writer = new StreamWriter(FileName, false, System.Text.Encoding.UTF8);
        writer.Write(json);
        writer.Close();
        LoadScores();
    }

    private void LoadScores()
    {
        try
        {
            StreamReader reader = new StreamReader(FileName);
            string json = reader.ReadToEnd();
            reader.Close();
            HighScoreData = JsonUtility.FromJson<HighScoreEntries>(json);
            HighScoreData.HighScores = HighScoreData.HighScores
                .OrderByDescending((hs) => hs.Score)
                .ThenBy((hs) => hs.Timestamp)
                .ToList();
        }
        catch (Exception ex)
        {
            HighScoreData = new HighScoreEntries
            {
                HighScores = new List<HighScoreEntry>()
            };
        }
    }


}
