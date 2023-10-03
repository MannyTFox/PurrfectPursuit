using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLevelManager : MonoBehaviour
{
    public static GameLevelManager levelManagerInstance;

    [SerializeField] Animator sceneTransitionAnimator;

    bool atSceneTransition = false;

    private void Awake()
    {
        levelManagerInstance = this.GetComponent<GameLevelManager>();
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

                case "menu":
                    // Load first demo
                    StartCoroutine(DelayToChangeScene("MainMenu"));
                    break;
            }
        }
    }

    public void RestartScene()
    {
        StartCoroutine(DelayToChangeScene(SceneManager.GetActiveScene().name));
    }

    public IEnumerator DelayToChangeScene(string sceneName)
    {
        atSceneTransition = true;

        // Start animation
        sceneTransitionAnimator.SetTrigger("transitionIn");

        yield return new WaitForSeconds(1.6f);

        // When its finished, load scene
        SceneManager.LoadScene(sceneName);
    }

    public IEnumerator DelayToTransitionOut()
    {
        yield return new WaitForSeconds(0.5f);

        // Start animation
        sceneTransitionAnimator.SetTrigger("transitionOut");
    }

    public bool IsTransitioning()
    {
        if (atSceneTransition)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
