using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientPickup : MonoBehaviour
{

    public Ingredient ingredient;


    // Update is called once per frame




    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CatInventory>())
        {
           
           CatInventory inventory = other.GetComponent<CatInventory>();

            if(inventory.ingredientImHolding == null)
            {
                inventory.ingredientImHolding = ingredient;
            }
        }
    }
}
