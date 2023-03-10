using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public Vector3[] spawnPoints;
    [SerializeField] GameObject[] enemies;
    [SerializeField] float spawnDelay;

    [ContextMenu("Start Waves")]
    public void StartWaves()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            Instantiate(enemies[Random.Range(0, enemies.Length)], spawnPoints[Random.Range(0, spawnPoints.Length)], Quaternion.identity);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
