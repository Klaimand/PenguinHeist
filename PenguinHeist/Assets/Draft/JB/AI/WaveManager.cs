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

    public int maxPolicemen = 30;
    public int maxPolicemenSpawn = 8;
    
    public static WaveManager instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    float waveCount;

    [ContextMenu("Start Waves")]
    public void StartWaves()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (LevelManager.instance.policeEnemies.Count <= maxPolicemen)
            {
                if (waveCount + enemyCountToSpawn <= maxPolicemenSpawn)
                {
                    waveCount++;
                }
                
                for (int i = 0; i < enemyCountToSpawn + waveCount; i++)
                {
                    LevelManager.instance.AddPoliceEnemy(Instantiate(enemies[Random.Range(0, enemies.Length)], 
                        spawnPoints[Random.Range(0, spawnPoints.Length)], Quaternion.identity).GetComponent<AIStateManager>());
                }
            }
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
