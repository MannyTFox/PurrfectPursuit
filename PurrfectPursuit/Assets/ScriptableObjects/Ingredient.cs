using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObjects/Ingredient")]
public class Ingredient : ScriptableObject
{
    [SerializeField] string ingredientName;
    public Sprite ingredientImage;
    public int styleAssetIndex;
    [SerializeField] GameObject ingredientObject;
    [SerializeField] GameObject ingredientDroppedObject;
    [SerializeField] GameObject ingredientHologramObject;
    [SerializeField] Vector3 ingredientObjectRotation;

    public string GetIngredientName()
    {
        return ingredientName;
    }

    public GameObject GetIngredientObject()
    {
        return ingredientObject;
    }

    public GameObject GetIngredientDroppedObject()
    {
        return ingredientDroppedObject;
    }

    public GameObject GetIngredientHologramObject()
    {
        return ingredientHologramObject;
    }

    public Vector3 GetIngredientRotation()
    {
        return ingredientObjectRotation;
    }
}
