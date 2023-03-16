using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    int nbPlayersIn = 0;

    [SerializeField] float secondsToEscape = 2f;

    float curSecs = 0f;

    bool escaping = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            nbPlayersIn++;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            nbPlayersIn--;
        }
    }

    void Update()
    {
        if (nbPlayersIn >= 2)
        {
            curSecs += Time.deltaTime;
        }
        else
        {
            curSecs = 0f;
        }

        if (!escaping && curSecs > secondsToEscape)
        {
            escaping = true;

            //load other scene
            print("exit game");
        }
    }
}
