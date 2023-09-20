using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSpotCauldron : MonoBehaviour
{
    /// <summary>
    /// When player enters, call method
    /// </summary>

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<CatInventory>())
        {
            CatInventory catInv = other.GetComponent<CatInventory>();

            if (catInv.CatHasIngredient())
            {
                catInv.GiveIngredientToCauldron(catInv.GetCatIngredient());
            }
        }
    }
}
