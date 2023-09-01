using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    [SerializeField] Transform ingredientDropSpot;

    [Header("Particles")]
    [SerializeField] ParticleSystem correctIngredientParticle;
    [SerializeField] ParticleSystem wrongIngredientParticle;


    public void SpawnIngredient(GameObject ingredient)
    {
        GameObject obj = Instantiate(ingredient, ingredientDropSpot.position, Quaternion.identity);

        // Turn on gravity on object
        obj.GetComponent<IngredientObject>().GravityOn();
    }

    public void CorrectIngredientBehaviour()
    {
        if (correctIngredientParticle != null)
        {
            correctIngredientParticle.Play();
        }
    }

    public void WrongIngredientBehaviour()
    {
        if (wrongIngredientParticle != null)
        {
            wrongIngredientParticle.Play();
        }
    }
}
