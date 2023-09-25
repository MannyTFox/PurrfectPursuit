using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngredientProcessor : MonoBehaviour
{

    [Header("References")]
    public Ingredient ingredientIn;
    public Ingredient ingredientOut;
    public float timeToProcessIngredient = 10;
    float timer;
    public bool isReady;
    bool machineBeingUsed = false;

    public Image statusDisplayFillImage;
    public TextMeshProUGUI statusText;

    //FOR NOW we're going to use the index of items on both these lists to determine what this script will spew out, ie input #1 will be output #1
    public List<Ingredient> validInputs = new List<Ingredient>();
    public List<Ingredient> validOutputs = new List<Ingredient>();

    [Header("Audio")]
    [SerializeField] AudioClip takeIngredientFromProcessorClip;
    AudioSource processorAmbianceSource;

    private void Start()
    {
        // Getting reference at start
        processorAmbianceSource = GetComponent<AudioSource>();

        // Set timer to normal
        timer = 0;

        statusText.color = Color.green;
    }

    private void Update()
    {
        Timer();

        // Only update the image if machine is working
        if (machineBeingUsed)
        {
            UpdateStatusImage();
        }

        if(isReady && !machineBeingUsed)
        {

            // Set image fill to 0
            statusDisplayFillImage.fillAmount = 1;

        }
    }

    public void Timer()
    {
        if (machineBeingUsed)
        {
            if (timer >= timeToProcessIngredient)
            {
                // Finished using machine ---

                isReady = true;
                ingredientOut = validOutputs[validInputs.IndexOf(ingredientIn)]; // set the output to the corresponding ingredient
                ingredientIn = null;

                timer = 0;

                MachineStopsWork();
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    public void UpdateStatusImage()
    {
        statusDisplayFillImage.fillAmount = timer / timeToProcessIngredient;
    }

    public IEnumerator TextAnimationWhileMachineWorking()
    {
        statusText.text = "";

        yield return new WaitForSeconds(0.8f);

        statusText.text = ".";

        yield return new WaitForSeconds(0.8f);

        statusText.text = "..";

        yield return new WaitForSeconds(0.8f);

        statusText.text = "...";

        yield return new WaitForSeconds(0.8f);

        StartCoroutine(TextAnimationWhileMachineWorking());
    }

    public void MachineStartsWork()
    {
        // Bool to indicate when the status image can start updating based on timer
        machineBeingUsed = true;

        // Start playing clip of processor working
        processorAmbianceSource.Play();

        // Text animation turn on
        statusText.color = Color.yellow;
        StartCoroutine(TextAnimationWhileMachineWorking());
    }

    public void MachineStopsWork()
    {
        // Machine stops
        machineBeingUsed = false;

        // Stop making processor sound
        processorAmbianceSource.Stop();

        // Stop animation
        StopAllCoroutines();

        // Change text so player knows its ready
        statusText.text = "?";
    }

    public void PlayerEntersMethod(CatInventory catVentory)
    {
        CatInventory inventory = catVentory;

        // If player is putting something to be processed
        if (inventory.ingredientImHolding != null && ingredientIn == null && ingredientOut == null) // move the item in the cat's inventory to the modifier.
        {
            if (validInputs.Contains(inventory.ingredientImHolding)) //ingredient is valid
            {
                // Add ingredient cat is holding
                ingredientIn = inventory.ingredientImHolding;

                // Cat loses reference of said ingredient
                inventory.CatLosesIngredient();

                // Machine starts its process
                MachineStartsWork();
            }

        }
        // If player is collecting the resulting ingredient
        else if (ingredientOut && inventory.ingredientImHolding == null && isReady) // move output to cat's inventory
        {
            // Gives ingredient to cat
            inventory.GiveCatNewIngredient(ingredientOut);
            ingredientOut = null;
            isReady = false;
            // Play sound of getting ingredient from processor
            AudioManager.audioManagerInstance.PlayIngredientPickupSound(takeIngredientFromProcessorClip);


            // Return text to normal after player collects item, waiting for another job
            statusText.color = Color.green;
            statusText.text = "!";
            statusDisplayFillImage.fillAmount = 0;
        }
    }

    public bool DoesPlayerHaveValidIngredient(CatInventory catInv)
    {
        if (validInputs.Contains(catInv.ingredientImHolding))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsMachineProcessing()
    {
        if (machineBeingUsed)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsIngredientReady()
    {
        if (isReady)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
   
}
