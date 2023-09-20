using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hologramPos : MonoBehaviour
{
    [SerializeField] GameObject ingredientHologramReference;

    bool isBeingUsed = false;

    public Transform GetHologramTransform()
    {
        return this.transform;
    }

    public GameObject GetHologramObject()
    {
        return ingredientHologramReference;
    }

    public void SetIngredientHologramReference(GameObject ingredientHolo)
    {
        ingredientHologramReference = ingredientHolo;

        isBeingUsed = true;
    }

    public void LoseHoloReference()
    {
        if (ingredientHologramReference != null)
        {
            Destroy(ingredientHologramReference);
        }
        ingredientHologramReference = null;

        isBeingUsed = false;
    }

    public bool IsItBeingUsed()
    {
        if (isBeingUsed)
        {
            return true;
        }
        else 
        {
            return false;
        }
    }
}
