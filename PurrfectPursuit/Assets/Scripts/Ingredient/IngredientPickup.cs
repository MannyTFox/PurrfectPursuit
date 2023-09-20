using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientPickup : MonoBehaviour
{

    public Ingredient ingredient;

    public bool pickupable = true;

    [Space(10)]
    [SerializeField] bool canBePickedUpOnce = false;


    // Update is called once per frame
    private void Update()
    {
        CheckDistanceFromPlayer();
    }

    public virtual Ingredient GetIngredient()
    {
        return ingredient;
    }

    public void IsIngredientPickupableLever(bool lever)
    {
        if (lever)
        {
            pickupable = true;
        }
        else
        {
            pickupable = false;
        }
    }

    public bool IsPickupable()
    {
        return pickupable;
    }

    public bool IsItInfiniteSource()
    {
        if (canBePickedUpOnce)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void CheckDistanceFromPlayer()
    {
        float distanceFromPlayer = Vector3.Distance(transform.position, GameManager.gameManagerInstance.GetPlayerTransform().position);

        // If close to player
        if(distanceFromPlayer < 5)
        {
            
        }
        else
        {
            
        }
    }
}
