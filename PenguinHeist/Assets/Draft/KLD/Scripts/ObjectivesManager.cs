using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectivesManager : MonoBehaviour
{
    [SerializeField] int nbObjectives = 8;

    List<Objective> objectives = new List<Objective>();

    int bagsSecured = 0;

    void Start()
    {
        objectives = FindObjectsOfType<Objective>().ToList();

        int nbToDestroy = Mathf.Max(0, objectives.Count - nbObjectives);

        List<Objective> toDestroy = new List<Objective>();

        for (int i = 0; i < nbToDestroy; i++)
        {
            int rdmIndx = Random.Range(0, objectives.Count);
            toDestroy.Add(objectives[rdmIndx]);
            objectives.Remove(objectives[rdmIndx]);
        }

        for (int i = toDestroy.Count - 1; i >= 0; i--)
        {
            Destroy(toDestroy[i].gameObject);
        }
    }

    public void CompleteObjective(Objective _objective)
    {
        objectives.Remove(_objective);
    }

    public void SecureBag()
    {
        bagsSecured++;
    }

}