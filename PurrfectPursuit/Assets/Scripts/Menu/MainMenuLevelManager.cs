using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLevelManager : MonoBehaviour
{
    public static MainMenuLevelManager levelManInstance;

    [SerializeField] Animator sceneTransitionAnimator;

    bool atLevelTransition = false;

    private void Awake()
    {
        levelManInstance = this.GetComponent<MainMenuLevelManager>();
    }

    public void LoadLevel(string levelID)
    {
        if (IsTransitioning() == false)
        {
            switch (levelID)
            {
                case "demo1":
                    // Load first demo
                    StartCoroutine(DelayToChangeScene("Demo1"));
                    break;
            }
        }
    }

    public IEnumerator DelayToChangeScene(string sceneName)
    {
        atLevelTransition = true;

        // Start animation
        sceneTransitionAnimator.SetTrigger("sceneTransition");

        yield return new WaitForSeconds(1.6f);

        // When its finished, load scene
        SceneManager.LoadScene(sceneName);
    }

    public IEnumerator DelayToTransitionOut()
    {
        sceneTransitionAnimator.SetTrigger("transitionOut");

        atLevelTransition = true;

        yield return new WaitForSeconds(1.2f);

        atLevelTransition = false;
    }

    public bool IsTransitioning()
    {
        if (atLevelTransition)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
