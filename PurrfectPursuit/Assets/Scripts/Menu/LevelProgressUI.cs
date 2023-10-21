using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressUI : MonoBehaviour
{
    [SerializeField] int levelIndex = 0; // 0 = Demo, 1 = Level 1, etc
    [SerializeField] List<GameObject> progressionImages = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckLevelProgress(levelIndex);
    }

    public void CheckLevelProgress(int levelIndexValue)
    {
        switch (levelIndexValue)
        {
            // Demo
            case 0:
                if (PlayerPrefs.HasKey(Prefs.DemoProgress))
                {
                    switch (PlayerPrefs.GetInt(Prefs.DemoProgress))
                    {
                        case 0:
                            progressionImages[0].SetActive(false);
                            progressionImages[1].SetActive(false);
                            progressionImages[2].SetActive(false);
                            break;
                        case 1:
                            progressionImages[0].SetActive(true);
                            break;
                        case 2:
                            progressionImages[0].SetActive(true);
                            progressionImages[1].SetActive(true);
                            break;
                        case 3:
                            progressionImages[0].SetActive(true);
                            progressionImages[1].SetActive(true);
                            progressionImages[2].SetActive(true);
                            break;
                    }
                }
                break;
        }
    }
}
