using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    public static DataHandler instance;

    public static Difficulty startingDifficulty; // 1 : Easy, 2 : Medium, 3 : Hard
    public static int round;
    public static int enemiesKilled;
    public static string playerName;

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;
    }

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
