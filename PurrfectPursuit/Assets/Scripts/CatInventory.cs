using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatInventory : MonoBehaviour
{

    public Ingredient ingredientImHolding;
    Transform cauldron;
    GameObject gameManager;

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
    }

    public void AddIngredient(Ingredient ing)
    {
        ingredientImHolding = ing;
    }

    public void ThrowIngredient(Ingredient ing)
    {
        ingredientImHolding = null;
        gameManager.GetComponent<GameManager>().IngredientAdded(ing);
        
    }

}
