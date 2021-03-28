using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager levelManager;
    [Header("Stats")]
    public int startDifficulty = 2;
    public int scale = 2;
    public int round = 1;

    [Header("Draggables")]
    public GameObject SpawnPoints;
    public List<GameObject> enemyTypes;

    public static List<Enemy> enemies;
    public static Transform[] spawnPoints;

    private void Awake()
    {
        if (LevelManager.levelManager == null)
            LevelManager.levelManager = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        spawnPoints = SpawnPoints.GetComponentsInChildren<Transform>();
        enemies = new List<Enemy>();
        SpawnEnemiesDiff(startDifficulty);
    }

    public void Spawnenemies(int numOfEnemies)
    {
        for(int i = 0; i < numOfEnemies; i++)
            Instantiate(enemyTypes[Random.Range(0, enemyTypes.Count)], spawnPoints[Random.Range(0, spawnPoints.Length)].transform);
    }

    public void SpawnEnemiesDiff(int difficulty)
    {
        while(difficulty > 0)
        {
            GameObject enemyType = enemyTypes[Random.Range(0, enemyTypes.Count)];
            difficulty -= enemyType.GetComponent<Enemy>().difficulty;
            enemies.Add(Instantiate(enemyType, spawnPoints[Random.Range(0, spawnPoints.Length)].transform, false).GetComponent<Enemy>());
        }
    }

    public void Update()
    {
        while (enemies.Count <= 0)
        {
            round++;
            SpawnEnemiesDiff(startDifficulty + scale * round);
        }
    }
}
