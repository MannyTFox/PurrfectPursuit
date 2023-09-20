using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObjects/PotionRecipe")]
public class PotionRecipe : ScriptableObject
{
    [SerializeField] string potionName;
    [SerializeField] List<Ingredient> ingredients = new List<Ingredient>();
    [SerializeField] float potionTime;
    [SerializeField] int potionPointsValue;

    [Space(10)]
    [SerializeField] Sprite potionSprite;
    
    public List<Ingredient> GetPotionIngredients()
    {
        return ingredients;
    }

    public Ingredient GetSingleIngredientFromPotion(int index)
    {
        if (index < ingredients.Count)
        {
            return ingredients[index];
        }
        else
        {
            return ingredients[0];
        }
    }

    public Sprite GetPotionImage()
    {
        return potionSprite;
    }

    public string GetPotionName()
    {
        return potionName;
    }

    public int GetPotionValue()
    {
        return potionPointsValue;
    }
}
