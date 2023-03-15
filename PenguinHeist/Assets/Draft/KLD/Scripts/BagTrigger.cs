using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BagTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent onBagEnter;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MoneyBag"))
        {
            Destroy(other.gameObject);
            LevelManager.instance.ObjectivesManager.SecureBag();
        }
    }
}