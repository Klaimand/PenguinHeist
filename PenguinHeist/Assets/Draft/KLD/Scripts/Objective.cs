using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    public void CompleteObjective()
    {
        LevelManager.instance.ObjectivesManager.CompleteObjective(this);
    }
}
