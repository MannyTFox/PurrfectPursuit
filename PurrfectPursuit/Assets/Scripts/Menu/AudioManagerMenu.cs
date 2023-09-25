using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerMenu : MonoBehaviour
{
    public static AudioManagerMenu audioManagerinstance;

    [SerializeField] AudioSource mainMenuMusicSource;
    [SerializeField] AudioSource meowFunnyMotiffSource;

    [Header("UI")]
    [SerializeField] AudioSource UISource1;
    [SerializeField] AudioClip pop1;
    [SerializeField] AudioClip click1;

    private void Awake()
    {
        audioManagerinstance = this.GetComponent<AudioManagerMenu>();
    }

    private void Start()
    {
        StartMenuMusicFunnyMotiff();
    }


    // --------- MUSIC ---------

    public void MainMenuMusicLever(bool playMusic)
    {
        if (playMusic)
        {
            mainMenuMusicSource.Play();
        }
        else 
        {
            mainMenuMusicSource.Stop();
        }
    }

    public void StartMenuMusicFunnyMotiff()
    {
        meowFunnyMotiffSource.Play();

        StartCoroutine(MotiffLoop());
    }

    public IEnumerator MotiffLoop()
    {
        yield return new WaitForSeconds(Random.Range(5, 9));

        meowFunnyMotiffSource.pitch = Random.Range(0.75f, 2);
        meowFunnyMotiffSource.Play();

        StartCoroutine(MotiffLoop());
    }


    // --------- UI ---------

    public void ButtonClickSound()
    {
        UISource1.clip = click1;
        UISource1.Play();
    }

    public void ButtonHoverSound()
    {
        UISource1.clip = pop1;
        UISource1.Play();
    }
}
