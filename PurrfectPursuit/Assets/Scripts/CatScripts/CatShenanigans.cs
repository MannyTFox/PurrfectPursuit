using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatShenanigans : MonoBehaviour
{
    [SerializeField] List<AudioClip> catMeows = new List<AudioClip>();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.gameManagerInstance.HasGameStarted())
        {
            PlayCatSound();
        }
    }

    public void PlayCatSound()
    {
        // Random audio
        int randomNumber = Random.Range(0, catMeows.Count);

        // Random Pitch
        float randomPitch = Random.Range(0.6f, 2f);

        // Play audio from manager
        AudioManager.audioManagerInstance.PlayCatSFX(catMeows[randomNumber], randomPitch, 0.1f);
    }
}
