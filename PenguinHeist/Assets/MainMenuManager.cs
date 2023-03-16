using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string CustomScene;
    [SerializeField] private Animator playButtonAnimator;
    [SerializeField] private Animator exitButtonAnimator;

    public void PlayCO() => StartCoroutine(Play());
    public void ExitCO() => StartCoroutine(Exit());

    private IEnumerator Play()
    {
        yield return new WaitWhile(() => playButtonAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        SceneManager.LoadScene("CustomizationScene");
    }

    private IEnumerator Exit()
    {
        yield return new WaitWhile(() => playButtonAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        Application.Quit();
    }
}
