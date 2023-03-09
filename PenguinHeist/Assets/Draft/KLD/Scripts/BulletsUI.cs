using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BulletsUI : MonoBehaviour
{
    [SerializeField] PlayerShoot playerShoot;
    [SerializeField] TMP_Text text;

    IEnumerator Start()
    {
        yield return null;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (playerShoot == null) Debug.LogError("PlayerShoot is null");

        text.text = playerShoot.CurMagazineBullets.ToString("00") + "/" + playerShoot.CurTotalBullets.ToString("000");
    }
}
