using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLauncher : MonoBehaviour
{
    [SerializeField] private string musicToPlay;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance.AudioManager.curMusic != musicToPlay)
        {
            
        GameManager.instance.AudioManager.StopAllLoopingSounds(0f);
        //yield return new WaitForSeconds(0.15f);
        }
        GameManager.instance.AudioManager.PlaySound(musicToPlay);
    }

}
