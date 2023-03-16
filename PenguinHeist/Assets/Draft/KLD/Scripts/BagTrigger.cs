using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BagTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent onBagEnter;

    [SerializeField] float playerDistance = 10f;

    List<PlayerBag> players = new List<PlayerBag>();

    [SerializeField] Animator bagPenguinAnimator;

    bool isPenguinOut = false;

    void Start()
    {
        PlayerController2[] p = FindObjectsOfType<PlayerController2>();

        players.Clear();
        foreach (var pl in p)
        {
            players.Add(pl.GetComponent<PlayerBag>());
        }
    }

    void Update()
    {
        foreach (var player in players)
        {
            if (player.IsCarrying && !isPenguinOut && Vector3.Distance(transform.position, player.transform.position) < playerDistance)
            {
                isPenguinOut = true;
                bagPenguinAnimator.SetTrigger("goOut");
                return;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isPenguinOut && other.CompareTag("MoneyBag"))
        {
            LevelManager.instance.StopEnemyTakeBag(other.transform);
            Destroy(other.gameObject);
            LevelManager.instance.ObjectivesManager.SecureBag();
            isPenguinOut = false;
            bagPenguinAnimator.SetTrigger("takeBag");
        }
    }
}