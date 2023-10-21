using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudioManager : MonoBehaviour
{
    public static CharacterAudioManager charAudioManInstance;

    [Header("VoiceFX - Sources")]
    [SerializeField] AudioSource BookVoiceSource;

    [Space(10)]
    [SerializeField] int chanceToPlayVoiceClip = 50;
    [SerializeField] bool audioPlaying = true;

    [Header("Book_VoiceClips")]
    [SerializeField] List<AudioClip> bookIngredientClips = new List<AudioClip>();
    [SerializeField] List<AudioClip> bookAnyIngredientClips = new List<AudioClip>();
    [SerializeField] List<AudioClip> possibleClips = new List<AudioClip>();

    // Clips will enter a ban list so they wont play repeatedly
    [Space(10)]
    [SerializeField]List<AudioClip> bannedClips = new List<AudioClip>();
    int unbanInt = 0;

    private void Awake()
    {
        charAudioManInstance = this.GetComponent<CharacterAudioManager>();
    }

    private void Update()
    {
        if (audioPlaying)
        {
            if (BookVoiceSource.isPlaying == false)
            {
                // When audio stops playing, make portrait dissapear
                UIManager.instanceUIManager.CharacterPortraitOff();
                print("book source not playing anymore");
                audioPlaying = false;
            }
        }
    }

    public void ChanceToPlayBookClip(string ingredientName)
    {
        // See if the clip will be player at all
        int randomNumber = Random.Range(0, 101);
        if (randomNumber <= chanceToPlayVoiceClip && audioPlaying == false)
        {

            // Create clean list of possible voice clips
            possibleClips.Clear();

            // Add the clips that can be played for any ingredient
            foreach (AudioClip clip in bookAnyIngredientClips)
            {
                AddClipIfNotBanned(clip, 1);
            }

            // Depending on which ingredient was offered, add its lines to the list also
            switch (ingredientName)
            {
                case "Blue Herb":
                    // Add twice the specfic interactions
                    AddClipIfNotBanned(bookIngredientClips[0], 2);
                    AddClipIfNotBanned(bookIngredientClips[1], 2);
                    break;
                case "Red Herb":
                    AddClipIfNotBanned(bookIngredientClips[10], 2);
                    AddClipIfNotBanned(bookIngredientClips[11], 2);
                    break;
                case "Yellow Herb":
                    AddClipIfNotBanned(bookIngredientClips[13], 2);
                    break;
                case "Poison Ivy":
                    AddClipIfNotBanned(bookIngredientClips[4], 2);
                    AddClipIfNotBanned(bookIngredientClips[5], 2);
                    AddClipIfNotBanned(bookIngredientClips[6], 2);
                    AddClipIfNotBanned(bookIngredientClips[8], 2);
                    break;
                case "Toadstool":
                    AddClipIfNotBanned(bookIngredientClips[12], 2);
                    break;
                case "Mana Flower":
                    AddClipIfNotBanned(bookIngredientClips[7], 2);
                    break;
                case "Devil's Finger":
                    AddClipIfNotBanned(bookIngredientClips[2], 2);
                    AddClipIfNotBanned(bookIngredientClips[3], 2);
                    break;
                default:
                    break;
            }

            // Get random clip to play. Ingredient specific lines will have a higher chance to play
            int randomClip = Random.Range(0, possibleClips.Count - 1);

            // Associate clip
            BookVoiceSource.Stop();
            BookVoiceSource.clip = possibleClips[randomClip];

            // Clip enters ban list and some other clip gets unbanned, maybe
            bannedClips.Add(possibleClips[randomClip]);
            unbanInt += 1;
            if(unbanInt == 2)
            {
                // Reset int
                unbanInt = 0;

                // Remove first item added
                bannedClips.RemoveAt(0);
            }

            // Play clip
            BookVoiceSource.Play();

            // Make character portrait appear
            UIManager.instanceUIManager.CharacterPortraitOn();
            audioPlaying = true;
        }
        else
        {
            // No clip is played at all
        }
    }

    public void AddClipIfNotBanned(AudioClip newClip, int timesToBeAdded)
    {
        if (IsClipBanned(newClip) == false)
        {
            for (int i = 0; i < timesToBeAdded; i++)
            {
                possibleClips.Add(newClip);
            }
        }
    }

    public bool IsClipBanned(AudioClip clip)
    {
        if (bannedClips.Count > 0)
        {
            foreach (AudioClip bannedClip in bannedClips)
            {
                if(clip == bannedClip)
                {
                    return true;
                }
            }

            return false;
        }
        else
        {
            return false;
        }
    }
}
