using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public Transform player1;
    public Transform player2;
    public List<AIStateManager> mafiaEnemies;
    public List<AIStateManager> policeEnemies = new List<AIStateManager>();

    [SerializeField] ObjectivesManager objectivesManager;
    public ObjectivesManager ObjectivesManager => objectivesManager;

    Dictionary<Transform, AIStateManager> enemiesBag = new Dictionary<Transform, AIStateManager>();

    bool alarm = false;

    [SerializeField] private PlayerHealth playerHealth1;
    [SerializeField] private PlayerHealth playerHealth2;

    [SerializeField] Animator policeQuadAnimator;

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

    public void RemoveMafiaEnemy(AIStateManager enemy)
    {
        mafiaEnemies.Remove(enemy);
    }

    public void RemovePoliceEnemy(AIStateManager enemy)
    {
        policeEnemies.Remove(enemy);
    }

    public void AddPoliceEnemy(AIStateManager enemy)
    {
        policeEnemies.Add(enemy);
    }

    AIStateManager TakeRandomEnemy()
    {
        if (policeEnemies.Count <= 0 && mafiaEnemies.Count <= 0) return null;

        int random = Random.Range(0, 2);
        if (policeEnemies.Count != 0 && random == 0)
        {
            return policeEnemies[Random.Range(0, policeEnemies.Count)];
        }

        if (mafiaEnemies.Count <= 0) return null;

        return mafiaEnemies[Random.Range(0, mafiaEnemies.Count)];
    }

    public void EnemyTakeBag(Transform bag)
    {
        AIStateManager randomEnemy = TakeRandomEnemy();

        if (randomEnemy == null) return;

        randomEnemy.agent.isStopped = false;
        enemiesBag.Add(bag, randomEnemy);
        //if (randomEnemy.currentState is AITakeBagState)
        //{
        //    EnemyTakeBag(bag);
        //}
        randomEnemy.SwitchToTakeBag(bag);
    }

    public void StopEnemyTakeBag(Transform bag)
    {
        if (!enemiesBag.ContainsKey(bag)) return;
        enemiesBag[bag].currentState = enemiesBag[bag].chaseState;
        enemiesBag.Remove(bag);
    }

    public void StartAlarm()
    {
        if (alarm) return;
        alarm = true;
        WaveManager.instance.StartWaves();
        policeQuadAnimator.SetTrigger("fadeIn");
        StartCoroutine(WaitAndDisableAlarm());
    }

    IEnumerator WaitAndDisableAlarm()
    {
        yield return new WaitForSeconds(4f);
        policeQuadAnimator.SetTrigger("fadeOut");
    }

    public PlayerHealth GetHealthFromTransform(Transform _transform)
    {
        if (_transform == player1)
        {
            return playerHealth1;
        }
        else if (_transform == player2)
        {
            return playerHealth2;
        }
        else
        {
            return null;
        }
    }

    public void CheckGameOver()
    {
        if (playerHealth1.IsNotAlive && playerHealth2.IsNotAlive)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
