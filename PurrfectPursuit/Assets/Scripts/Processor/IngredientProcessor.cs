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

    private void Start()
    {
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

        // Text animation turn on
        statusText.color = Color.yellow;
        StartCoroutine(TextAnimationWhileMachineWorking());
    }

    public void MachineStopsWork()
    {
        // Machine stops
        machineBeingUsed = false;

        // Stop animation
        StopAllCoroutines();

        // Change text so player knows its ready
        statusText.text = "?";
    }

    // Detect player to decide on action
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CatInventory>())
        {

            CatInventory inventory = other.GetComponent<CatInventory>();

            // If player is putting something to be processed
            if (inventory.ingredientImHolding != null && ingredientIn == null && ingredientOut == null) // move the item in the cat's inventory to the modifier.
            {
                if (validInputs.Contains(inventory.ingredientImHolding)) //ingredient is valid
                {
                    ingredientIn = inventory.ingredientImHolding;
                    inventory.ingredientImHolding = null;

                    MachineStartsWork();
                }

            }
            // If player is collecting the resulting ingredient
            else if (ingredientOut && inventory.ingredientImHolding == null && isReady) // move output to cat's inventory
            {
                inventory.ingredientImHolding = ingredientOut;
                ingredientOut = null;
                isReady = false;

                // Return text to normal after player collects item, waiting for another job
                statusText.color = Color.green;
                statusText.text = "!";
                statusDisplayFillImage.fillAmount = 0;
            }
        }
    }
   
}
