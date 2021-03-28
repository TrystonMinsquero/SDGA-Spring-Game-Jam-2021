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
        Transform[] tempArray = SpawnPoints.GetComponentsInChildren<Transform>();
        //Remove inital spawnpoint transform
        spawnPoints = new Transform[tempArray.Length - 1];
        for (int i = 1; i < tempArray.Length; i++)
            spawnPoints[i - 1] = tempArray[i];
        enemies = new List<Enemy>();
        SpawnEnemiesDiff(startDifficulty);
    }

    public void Spawnenemies(int numOfEnemies)
    {
        HUD.displayRound(round);
        for (int i = 0; i < numOfEnemies; i++)
        {
            Enemy enemy = Instantiate(enemyTypes[Random.Range(0, enemyTypes.Count)]).GetComponent<Enemy>();
            enemy.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
            enemies.Add(enemy);
        }
        HUD.updateEnemies(enemies.Count);
    }

    public void SpawnEnemiesDiff(int difficulty)
    {
        HUD.displayRound(round);
        while (difficulty > 0)
        {
            GameObject enemyType = enemyTypes[Random.Range(0, enemyTypes.Count)];
            difficulty -= enemyType.GetComponent<Enemy>().difficulty;
            Enemy enemy = Instantiate(enemyType).GetComponent<Enemy>();
            enemy.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
            enemies.Add(enemy);
        }
        HUD.updateEnemies(enemies.Count);
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
