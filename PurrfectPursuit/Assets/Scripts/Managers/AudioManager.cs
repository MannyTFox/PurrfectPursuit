using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioManagerInstance;

    /// <summary>
    /// AudioManager is going to take care of all sounds that dont need to be 3D, making our life much easier when organizing 
    /// the sources because they will all be managed here!
    /// </summary>

    [Header("Music - Sources")]
    [SerializeField] AudioSource MusicMainSource;

    [Header("SFX - Sources")]
    [SerializeField] AudioSource SFXCatSource;
    [Space(10)]
    [SerializeField] AudioSource SFXIngredientPickupSource;
    [Space(10)]
    [SerializeField] AudioSource SFXIngredientFeedbackSource;
    [SerializeField] AudioClip correctIngredientClip;
    [SerializeField] AudioClip wrongIngredientClip;

    [Header("UI - Sources")]
    [SerializeField] AudioSource SFXUISource;
    [SerializeField] AudioClip pop1;
    [SerializeField] AudioClip click1;

    private void Awake()
    {
        audioManagerInstance = this.GetComponent<AudioManager>();    
    }


    // ------ MUSIC ------

    public void PlayMainMusic()
    {
        if (MusicMainSource.clip != null)
        {
            if (MusicMainSource.isPlaying)
            {
                MusicMainSource.Stop();
            }

            MusicMainSource.Play();
        }
        else
        {
            Debug.LogWarning("Music Main Source has no clip to play!");
        }
    }


    // ------ SFX ------

    public void PlayIngredientPickupSound(AudioClip clip)
    {
        SFXIngredientPickupSource.Stop();
        SFXIngredientPickupSource.clip = clip;

        // Randomize pitch randomly
        SFXIngredientPickupSource.pitch = RandomPitch(60);

        SFXIngredientPickupSource.Play();
    }

    public float RandomPitch(float chanceOfChangingPitchPercentage)
    {
        int randomNumber = Random.Range(0, 101);

        if(randomNumber < chanceOfChangingPitchPercentage)
        {
            // Get random pitch value
            float randomPitchValue = Random.Range(0.5f, 0.98f);

            return randomPitchValue;
        }
        else
        {
            return 1;
        }
    }

    public void PlayIngredientFeedbackAudio(bool wasIngredientCorrect)
    {
        SFXIngredientFeedbackSource.Stop();

        if (wasIngredientCorrect)
        {
            SFXIngredientFeedbackSource.clip = correctIngredientClip;
        }
        else
        {
            SFXIngredientFeedbackSource.clip = wrongIngredientClip;
        }

        SFXIngredientFeedbackSource.Play();
    }

    public void PlayCatSFX(AudioClip newClip, float newPitch)
    {
        if (SFXCatSource.isPlaying)
        {
            SFXCatSource.Stop();
        }

        SFXCatSource.pitch = newPitch;
        SFXCatSource.clip = newClip;
        SFXCatSource.Play();
    }


    // ------ UI ------
    public void ButtonClickSound()
    {
        SFXUISource.clip = click1;
        SFXUISource.Play();
    }

    public void ButtonHoverSound()
    {
        SFXUISource.clip = pop1;
        SFXUISource.Play();
    }
}
