using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [Header("List of potions game can pick from")]
    public List<PotionRecipe> recipes = new List<PotionRecipe>();

    [Header("UI references")]
    public TextMeshProUGUI witchSpeech;
    public TextMeshProUGUI dayTimer;
    public GameObject ingredientPanel;
    public GameObject ingredientImageSlot;



    [Header("Debug only")]
    public PotionRecipe currentRecipe;
    public List<Ingredient> ingredientsIwant;
    public int ingredientsLeft = 0; // pointer used by this script to know how far along a recipe we are
    bool finished;

    public Image timeBar;
    float time = 0;
    bool congratulating = false; // to avoid calling congratulate more than once

    [Header ("Tally References")]

    GameObject tallyPanel;
    Animator witchAnimator;

    public TextMeshProUGUI potionsSoldText;
    public TextMeshProUGUI daySalesText;
    public TextMeshProUGUI currentBalanceText;
    public bool currentBalanceHasBeenUpdated;

    public int potionsSold; //# of potions sold on the current day
    public int daySales; // $ obtained from sales on the current day
    public int currentBalance; // $ saved from previous and current sales

    [Header("Player Reference")]
    public PlayerMovement playerMov;
    public CinemachineBrain playerCameraBrain;

    [Header("Tutorial References")]
    public GameObject startTutorialRef;

    private void UpdateIngredientPanel()
    {
        foreach(Transform child in ingredientPanel.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var ing in ingredientsIwant)
        {
           GameObject slot = Instantiate(ingredientImageSlot, ingredientPanel.transform);
            slot.GetComponent<Image>().sprite = ing.ingredientImage;
        }
    }

    private void Start()
    {
        witchAnimator = GameObject.Find("Witch").GetComponent<Animator>();
        currentBalance = PlayerPrefs.GetInt("currentBalance");
        currentBalanceHasBeenUpdated = false;
        ChangePotion();

        tallyPanel = GameObject.Find("Tally Panel");
        tallyPanel.SetActive(false);

        // Set game time limit
        time = 100;

        // Cursor is visible to skip tutorial
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void GameStart()
    {
        // Tutorial Dissapears
        startTutorialRef.SetActive(false);

        // Start timer of the day countdown
        StartCoroutine(Timer());

        // Free player, allowing them to move
        playerMov.UnlockPlayer();
        // enable camera
        playerCameraBrain.enabled = true;
        
        // Cursor becomes invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void checkTime()
    {
        //dayTimer.text = time.ToString();
        timeBar.fillAmount = time / 100;
        if (time <= 0)
        {
            DayOver();
        }
    }

    IEnumerator Timer()
    {
            
  
            time -= 1;
            yield return new WaitForSeconds(1.0f);
            StartCoroutine(Timer());
    

    }


    void Update()
    {
        
        checkTime();

     
        
        if(ingredientsLeft > 0)
        {

            //potion not yet finished
            
            
          
        }
        else if(ingredientsLeft <= 0 && !congratulating)
        {
            //potion is finished

            potionsSold += 1;
            daySales += currentRecipe.potionSellPrice;
           

            StartCoroutine(Congratulate());
        }

        
    }

    public void IngredientAdded(Ingredient ing) // call this function whenever the cat throws an ingredient in the cauldron
    {
        if(ingredientsIwant.Contains(ing))
        {
            StartCoroutine(WasIngredientCorrect(true, ing));
        }
        else
        {
            StartCoroutine(WasIngredientCorrect(false, ing));
        }
    }


    public IEnumerator WasIngredientCorrect(bool lever, Ingredient ing)
    {
        // Spawn Ingredient on top of Cauldron
        

        // Wait a little bit for the ingredient to fall into cauldron
        yield return new WaitForSeconds(2);

        // Result
        
        // Correct ingredient
        if (lever)
        {
            // Good particle effect plays

            // Ingredient is removed, potion progress & time reward
            witchAnimator.SetTrigger("throw");
            ingredientsLeft -= 1;
            time += 3;
            ingredientsIwant.Remove(ing);
        }
        // Wrong ingredient 
        else
        {
            // Bad particle effect plays

            // Wrong ingredient penalty
            time -= 10;
        }

        UpdateIngredientPanel();
    }

    IEnumerator Congratulate()
    {
        // Money particle because potion was sold!

        // Change potion and animations
        congratulating = true;
        yield return new WaitForSeconds(0.14f);
        witchAnimator.SetTrigger("power");
        yield return new WaitForSeconds(3f);
        ChangePotion();
        congratulating = false;
        
    }

    public void ChangePotion()
    {
        
        currentRecipe = recipes[Random.Range(0, recipes.Count)];
        ingredientsIwant = new List<Ingredient>(currentRecipe.ingredients);
        ingredientsLeft = ingredientsIwant.Count;

        UpdateIngredientPanel();
    }

    public void DayOver()
    {

        GameObject.Find("Cat").GetComponent<PlayerMovement>().LockPlayer();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //show tally screen, save balance.


        


        potionsSoldText.text = ("Potions sold: #" + potionsSold.ToString());
        daySalesText.text = ("Day's sales: $" + daySales.ToString());
        UpdateCurrentBalance(daySales);
        currentBalanceText.text = ("Current Balance: $" + currentBalance.ToString());
        

        tallyPanel.gameObject.SetActive(true);


    }
    
    void UpdateCurrentBalance(int _daySales) //something wrong here
    {
        while(currentBalanceHasBeenUpdated == false)
        {
            currentBalance = PlayerPrefs.GetInt("currentBalance");
            int updatedValue = currentBalance + _daySales;
            PlayerPrefs.SetInt("currentBalance", updatedValue);
            currentBalance = PlayerPrefs.GetInt("currentBalance");
            currentBalanceHasBeenUpdated = true;
        }

       
    }
     
    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }

}
