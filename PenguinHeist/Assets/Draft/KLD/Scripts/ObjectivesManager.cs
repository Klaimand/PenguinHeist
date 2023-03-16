using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class ObjectivesManager : MonoBehaviour
{

    [Header("References")]
    [SerializeField] Image moneyBar;

    [Header("Values")]
    [SerializeField] int nbObjectives = 9;

    [SerializeField] int minimumSecuredBags = 3;

    List<Objective> objectives = new List<Objective>();

    int bagsSecured = 0;

    [SerializeField] GameObject secureMore;
    [SerializeField] GameObject secureOrEscape;

    [SerializeField] TMP_Text nbBagsText;

    [SerializeField] GameObject exitTrig;
    [SerializeField] GameObject whitePlane;

    void Start()
    {
        secureMore.SetActive(true);
        secureOrEscape.SetActive(false);

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

        UpdateUI();
    }

    public void CompleteObjective(Objective _objective)
    {
        objectives.Remove(_objective);
        LevelManager.instance.StartAlarm();
    }

    public void SecureBag()
    {
        bagsSecured++;
        UpdateUI();

        if (bagsSecured == minimumSecuredBags)
        {
            secureMore.SetActive(false);
            secureOrEscape.SetActive(true);

            exitTrig.SetActive(true);
            whitePlane.SetActive(true);
        }
    }

    void UpdateUI()
    {
        nbBagsText.text = $"{bagsSecured}/{nbObjectives} money bags secured !";
        moneyBar.fillAmount = (float)bagsSecured / nbObjectives;
    }

}