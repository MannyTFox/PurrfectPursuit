using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IngredientProcessor : MonoBehaviour
{

    [Header("References")]
    public Ingredient ingredientIn;
    public Ingredient ingredientOut;
    public float timer;
    public bool isReady;

    public TextMeshProUGUI statusDisplay;

    //FOR NOW we're going to use the index of items on both these lists to determine what this script will spew out, ie input #1 will be output #1
    public List<Ingredient> validInputs = new List<Ingredient>();
    public List<Ingredient> validOutputs = new List<Ingredient>();

    private void Start()
    {
        
        statusDisplay.text = ("Standing by");
    }

    private IEnumerator Timer()
    {
        statusDisplay.text = ("Prossesing...");
        yield return new WaitForSeconds(timer); 
        isReady = true;
        ingredientOut = validOutputs[validInputs.IndexOf(ingredientIn)]; // set the output to the corresponding ingredient
        ingredientIn = null;
    }


    private void Update()
    {
        if(isReady)
        {
            
            statusDisplay.text = ("Done processing ");
            
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CatInventory>())
        {

            CatInventory inventory = other.GetComponent<CatInventory>();

            if (inventory.ingredientImHolding != null && ingredientIn == null && ingredientOut == null) // move the item in the cat's inventory to the modifier.
            {
                if (validInputs.Contains(inventory.ingredientImHolding)) //ingredient is valid
                {
                    ingredientIn = inventory.ingredientImHolding;
                    inventory.ingredientImHolding = null;
                    StartCoroutine(Timer());
                }

            }
            else if (ingredientOut && inventory.ingredientImHolding == null && isReady) // move output to cat's inventory
            {
                inventory.ingredientImHolding = ingredientOut;
                ingredientOut = null;
                isReady = false;
            }
        }
    }


     
   
}
