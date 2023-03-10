using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Tooltip("Spawn points of the police enemies")]
    public Vector3[] spawnPoints;
    [Tooltip("Enemies to spawn")]
    [SerializeField] GameObject[] enemies;
    [Tooltip("Delay between each wave")]
    [SerializeField] float spawnDelay;
    [Tooltip("Number of enemies to spawn")]
    [SerializeField] int enemyCountToSpawn;

    [ContextMenu("Start Waves")]
    public void StartWaves()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            for (int i = 0; i < enemyCountToSpawn; i++)
            {
                Instantiate(enemies[Random.Range(0, enemies.Length)], spawnPoints[Random.Range(0, spawnPoints.Length)], Quaternion.identity);
            }
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
