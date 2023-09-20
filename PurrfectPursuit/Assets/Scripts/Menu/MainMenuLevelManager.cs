using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLevelManager : MonoBehaviour
{

    public void LoadLevel(string levelID)
    {
        switch (levelID)
        {
            case "demo1":
                // Load first demo
                SceneManager.LoadScene("Demo1");
                break;
        }
    }
}
