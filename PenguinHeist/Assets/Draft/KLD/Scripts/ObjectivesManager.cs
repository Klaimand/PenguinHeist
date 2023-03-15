using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectivesManager : MonoBehaviour
{
    [SerializeField] int nbObjectives = 8;

    List<Objective> objectives = new List<Objective>();

    void Start()
    {
        objectives = FindObjectsOfType<Objective>().ToList();

        int nbToDestroy = Mathf.Max(0, objectives.Count - nbObjectives);

        List<Objective> toDestroy = new List<Objective>();

        for (int i = 0; i < nbToDestroy; i++)
        {
            //toDestroy.Add(Random.Range(0,))
        }
    }

    public void CompleteObjective(Objective _objective)
    {

    }

}
