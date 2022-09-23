using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HighScoreEntry
{
    public string Name;
    public int Score;
    public long Timestamp;
}

[Serializable]
public class HighScoreEntries {
    public List<HighScoreEntry> HighScores;
}
