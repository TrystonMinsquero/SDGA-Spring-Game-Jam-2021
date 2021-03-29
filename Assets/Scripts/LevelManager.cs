using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager levelManager;
    [Header("Stats")]
    public int StartDifficulty = 2;
    public int Scale = 1;
    public int StartRound = 1;

    [Header("Draggables")]
    public GameObject SpawnPoints;
    public List<GameObject> EnemyTypes;

    private static Player player;
    private static List<Enemy> enemies;
    private static Transform[] spawnPoints;
    private static List<GameObject> enemyTypes;

    private static int startDifficulty;
    private static int scale;
    private static int round;
    private static int enemiesKilled;

    private void Awake()
    {
        if (LevelManager.levelManager == null)
            LevelManager.levelManager = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        //Remove inital spawnpoint transform
        Transform[] tempArray = SpawnPoints.GetComponentsInChildren<Transform>();
        spawnPoints = new Transform[tempArray.Length - 1];
        for (int i = 1; i < tempArray.Length; i++)
            spawnPoints[i - 1] = tempArray[i];

        player = GameObject.Find("Player").GetComponent<Player>();
        enemies = new List<Enemy>();
        enemyTypes = EnemyTypes;
        startDifficulty = StartDifficulty;
        scale = Scale;
        round = StartRound;

        SpawnEnemiesDiff(StartDifficulty);
    }

    public static void SpawnEnemies(int numOfEnemies)
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

    public static void SpawnEnemiesDiff(int difficulty)
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

    public static void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        enemiesKilled++;
        CheckEnemies();
    }

    public static void ClearEnemies()
    {
        foreach (Enemy enemy in enemies)
            enemy.Die();
    }

    public static void NextRound()
    {
        round++;
        ClearEnemies();
        SpawnEnemiesDiff(startDifficulty + (scale * round));
        player.current_health = player.max_health;
        HUD.updateHearts(player.current_health);

    }

    public static void StartOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void CheckEnemies()
    {
        HUD.updateEnemies(enemies.Count);
        if (enemies.Count <= 0)
            NextRound();
    }
}
