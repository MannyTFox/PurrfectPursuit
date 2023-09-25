using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookBehaviour : MonoBehaviour
{
    Animator bookAnim;
    [SerializeField] InteractSpot interactSpot;

    bool ingredientsShowing = false;

    // When holograms are showing, put hologram ingredients floating in random positions
    [SerializeField] List<hologramPos> hologramPositions = new List<hologramPos>();
    List<GameObject> hologramReferences = new List<GameObject>();

    [Header("Audio")]
    AudioSource bookAmbianceSource;

    // Start is called before the first frame update
    void Start()
    {
        bookAnim = GetComponent<Animator>();
        bookAmbianceSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (ingredientsShowing == false && interactSpot.IsPlayerIn() == false)
        {
            // Holograms instantly dissapear when player leaves
            MakeHologramsDissapearLever(true);
        }

        // Check to play hologram audio
        if (ingredientsShowing)
        {
            if (bookAmbianceSource.isPlaying == false)
            {
                bookAmbianceSource.Play();
            }
        }
        else
        {
            if (bookAmbianceSource.isPlaying)
            {
                bookAmbianceSource.Stop();
            }
        }
    }

    public void ShowHolograms()
    {
        ingredientsShowing = true;

        // Setting parameters for animation
        bookAnim.SetTrigger("open");
        bookAnim.SetBool("hologramIdle", true);
        bookAnim.SetBool("idle", false);

        // Holograms appear after a little delay
        StartCoroutine(DelayToAppearHolograms());
    }

    public void StopShowingHolograms()
    {
        ingredientsShowing = false;

        // Set parameters for animations
        bookAnim.SetBool("hologramIdle", false);
        bookAnim.SetBool("idle", true);

        // Holograms instantly dissapear when player leaves
        MakeHologramsDissapearLever(true);
    }

    public void UpdateHolograms()
    {
        // Get how many holograms will be instantiated
        int howManyHolograms = GameManager.gameManagerInstance.GetRemainingIngredients().Count;

        foreach (hologramPos holoPos in hologramPositions)
        {
            holoPos.LoseHoloReference();
        }

        // Instantiate that many holograms in a random space
        for (int i = 0; i < howManyHolograms; i++)
        {
            hologramPos chosenPos = GetRandomHologramPos().GetComponent<hologramPos>();

            GameObject newHolo =
                       Instantiate(
                       GameManager.gameManagerInstance.GetRemainingIngredients()[i].GetIngredientHologramObject(),
                       chosenPos.GetHologramTransform().position,
                       Quaternion.identity);

            chosenPos.SetIngredientHologramReference(newHolo);
            print("pos has reference: " + chosenPos.GetHologramObject());
            //hologramReferences.Add(newHolo);
        }

        // Depending if holograms are showing, make them appear or dissapear
        if (ingredientsShowing)
        {
            // Make holograms appear because player is inside 
            MakeHologramsDissapearLever(false);
        }
        else
        {
            // Make holograms appear because player is inside 
            MakeHologramsDissapearLever(true);
        }
    }

    public Transform GetRandomHologramPos()
    {
        List<hologramPos> usablePositions = new List<hologramPos>();

        foreach (hologramPos pos in hologramPositions)
        {
            if (pos.IsItBeingUsed() == false)
            {
                usablePositions.Add(pos);
            }
            else
            {
                // its being used, discard it
            }
        }

        int randomNumber;

        if (usablePositions.Count > 1)
        {
            randomNumber = Random.Range(0, usablePositions.Count);
        }
        else
        {
            randomNumber = 0;
        }

        return usablePositions[randomNumber].GetHologramTransform();
    }

    public IEnumerator DelayToAppearHolograms()
    {
        yield return new WaitForSeconds(0.4f);

        MakeHologramsDissapearLever(false);
    }

    public void MakeHologramsDissapearLever(bool lever)
    {
        // If true, dissapear, if false, appear
        if (lever)
        {
            foreach (hologramPos holoPos in hologramPositions)
            {
                if (holoPos.GetHologramObject() != null)
                {
                    holoPos.GetHologramObject().SetActive(false);
                }
            }
        }
        else
        {
            foreach (hologramPos holoPos in hologramPositions)
            {
                if (holoPos.GetHologramObject() != null)
                {
                    holoPos.GetHologramObject().SetActive(true);
                }
            }
        }
    }

    public void ResetHologramReferenceList()
    {
        hologramReferences.Clear();
    }
}
