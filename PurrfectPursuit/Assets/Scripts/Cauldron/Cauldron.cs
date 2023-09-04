using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    [SerializeField] Transform ingredientDropSpot;

    [Header("Particles")]
    [SerializeField] ParticleGroup correctIngredientParticleGroup;
    [SerializeField] ParticleGroup wrongIngredientParticleGroup;


    public void SpawnIngredient(GameObject ingredient)
    {
        GameObject obj = Instantiate(ingredient, ingredientDropSpot.position, Quaternion.identity);

        // Turn on gravity on object
        obj.GetComponent<IngredientObject>().GravityOn();
    }

    public void CorrectIngredientBehaviour()
    {
        if (correctIngredientParticleGroup != null)
        {
            correctIngredientParticleGroup.PlayParticleGroup();
        }
    }

    public void WrongIngredientBehaviour()
    {
        if (wrongIngredientParticleGroup != null)
        {
            wrongIngredientParticleGroup.PlayParticleGroup();
        }
    }
}
