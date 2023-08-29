using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatInventory : MonoBehaviour
{

    public Ingredient ingredientImHolding;

    Transform cauldron;
    GameObject gameManager;

    [Space(10)]
    [SerializeField] Transform itemHolding;
    public bool canGetIngredient = true;

    private void Start()
    {
        gameManager = GameObject.Find("Game Manager");
        cauldron = GameObject.Find("Cauldron").GetComponent<Transform>();
    }
    private void Update()
    {
        if(Vector3.Distance(transform.position, cauldron.position) <= 5) //If the cat is near the cauldron
        {
            if (ingredientImHolding) //If the cat has an ingredient
            {
                ThrowIngredient(ingredientImHolding);
            }
        }

        ResetInput();
    }

    public void ThrowIngredient(Ingredient ing)
    {
        ingredientImHolding = null;
        gameManager.GetComponent<GameManager>().IngredientAdded(ing);

        // Remove ingredient from cats mouth
        DestroyIngredientInMouth();
        
    }


    // PICKUP INGREDIENT
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "IngredientPickup")
        {
            // Turn on control tip animation
            UIManager.instanceUIManager.InteractButtonTipLever(true);

            // Now its possible for cat to get ingredient
            GetIngredientInput(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "IngredientPickup")
        {
            // Turn off control tip animation
            UIManager.instanceUIManager.InteractButtonTipLever(false);
        }
    }

    public void GetIngredientInput(GameObject ingredientPickupObject)
    {
        if (Input.GetKey(KeyCode.E) && ingredientPickupObject.GetComponent<IngredientPickup>() && canGetIngredient)
        {
            // Set bool to false so it only enters once per press
            canGetIngredient = false;

            // Make it so the cat is holding new ingredient
            ingredientImHolding = ingredientPickupObject.GetComponent<IngredientPickup>().GetIngredient();

            // Instantiate ingredient in mouth of cat
            PutIngredientInMouth();
        }
    }

    public void ResetInput()
    {
        // Reset bool so cat can press again
        if (Input.GetKeyUp(KeyCode.E))
        {
            canGetIngredient = true;
        }
    }

    public void PutIngredientInMouth()
    {
        if(ingredientImHolding.GetIngredientObject() != null)
        {
            // If already holding something, destroy it to substitute
            if(itemHolding.childCount > 0)
            {
                Destroy(itemHolding.GetChild(0).gameObject);            
            }

            // Put current NEW ingredient in mouth
            GameObject newMouthObj = Instantiate(ingredientImHolding.GetIngredientObject(), itemHolding.position, Quaternion.identity);

            newMouthObj.transform.parent = itemHolding;
            newMouthObj.transform.localEulerAngles = ingredientImHolding.GetIngredientRotation();
        }
        else
        {
            DestroyIngredientInMouth();
        }
    }

    public void DestroyIngredientInMouth()
    {
        // Destroy what is on the cats mouth
        if (itemHolding.childCount > 0)
        {
            Destroy(itemHolding.GetChild(0).gameObject);
        }
    }
}
