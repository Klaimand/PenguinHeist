using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public Transform player1;
    public Transform player2;
    public List<AIStateManager> mafiaEnemies;
    List<AIStateManager> policeEnemies = new List<AIStateManager>();

    [SerializeField] ObjectivesManager objectivesManager;
    public ObjectivesManager ObjectivesManager => objectivesManager;

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
        AIStateManager randomPolice = policeEnemies[Random.Range(0, policeEnemies.Count)];
        AIStateManager randomMafia = mafiaEnemies[Random.Range(0, mafiaEnemies.Count)];
        int random = Random.Range(0, 2);
        if (random == 0)
        {
            return randomPolice;
        }
        else
        {
            return randomMafia;
        }
    }

    public void TakeBag(Transform bag)
    {
        AIStateManager randomEnemy = TakeRandomEnemy();
        if (randomEnemy.currentState is AITakeBagState)
        {
            TakeBag(bag);
        }
        randomEnemy.SwitchToTakeBag(bag);
    }
}
