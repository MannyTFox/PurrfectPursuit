using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObjects/PotionRecipe")]
public class PotionRecipe : ScriptableObject
{
    public string potionName;
    public List<Ingredient> ingredients = new List<Ingredient>();
    public float potionTime;
    public int potionSellPrice;
    
}
