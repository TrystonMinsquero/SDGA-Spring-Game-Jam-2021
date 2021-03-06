using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    public static DataHandler instance;

    public static Difficulty startingDifficulty = Difficulty.EASY; // 1 : Easy, 2 : Medium, 3 : Hard
    public static int round = 1;
    public static int enemiesKilled;

    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;

        Screen.SetResolution(1920, 1200, true);

    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public static void reset()
    {
        startingDifficulty = Difficulty.EASY;
        round = 1;
        enemiesKilled = 0;
    }
}

public enum Difficulty
{
    EASY = 1,
    HARD = 2,
    HARDCORE = 3
}
