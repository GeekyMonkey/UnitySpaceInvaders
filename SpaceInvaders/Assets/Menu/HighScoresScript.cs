using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class HighScoresScript : MonoBehaviour
{
    public Transform HighScoresContainer;
    public Transform HighScoreTemplate;
    public float Height = 10;
    public int HighScoresCountMax = 10;

    private List<Transform> HighScoreRows = new List<Transform>();

    void Start()
    {
        Debug.Log("High Scores Start");
        for (int i = 0; i < HighScoresCountMax; i++)
        {
            Transform clone = Instantiate(HighScoreTemplate, HighScoresContainer);
            RectTransform rect = clone.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, rect.anchoredPosition.y - Height * i);
            HighScoreRows.Add(rect);
        }
        HighScoreTemplate.gameObject.SetActive(false);
        DrawScores();
    }

    void DrawScores()
    {
        var HighScoreData = GlobalStateScript.Instance.HighScoreData;
        int HighScoresCount = HighScoreData.HighScores.Count;

        for (int i = 0; i < HighScoresCountMax; i++)
        {
            Transform row = HighScoreRows[i];
            string indexText = "";
            string nameText = "";
            string scoreText = "";
            if (HighScoresCount > i)
            {
                indexText = (i + 1).ToString("D2");
                nameText = HighScoreData.HighScores[i].Name;
                scoreText = HighScoreData.HighScores[i].Score.ToString();
            }
            row.Find("IndexText").GetComponent<TMP_Text>().text = indexText;
            row.Find("NameText").GetComponent<TMP_Text>().text = nameText;
            row.Find("ScoreText").GetComponent<TMP_Text>().text = scoreText;
        }
    }

}
