using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void GOMainMenu()
    {
        SceneManager.LoadScene("MainMenuGA");
    }

    public void ReLaunchGame()
    {
        SceneManager.LoadScene("_Scenes/CustomizationScene");
    }
}
