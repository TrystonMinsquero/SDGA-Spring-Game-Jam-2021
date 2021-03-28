using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public GameObject SpawnPoints;
    public List<GameObject> enemyTypes;

    Transform[] spawnPoints;

    private void Start()
    {
        spawnPoints = SpawnPoints.GetComponentsInChildren<Transform>();
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
            Instantiate(enemyType, spawnPoints[Random.Range(0, spawnPoints.Length)].transform);
        }
    }
}
