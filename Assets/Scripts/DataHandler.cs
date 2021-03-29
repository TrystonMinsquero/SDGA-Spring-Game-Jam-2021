using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    public Difficulty startingDifficulty; // 1 : Easy, 2 : Medium, 3 : Hard
    public int totalScore;
    public int enemiesKilled;
    public string playerName;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}

public enum Difficulty
{
    EASY = 1,
    HARD = 2,
    HARDCORE = 3
}
