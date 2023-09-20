using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatInventory : MonoBehaviour
{
    public Ingredient ingredientImHolding;
    public GameObject itemImHolding;

    Transform cauldron;
    GameObject gameManager;

    [Space(10)]
    [SerializeField] Transform itemHolding;
    public bool canGetIngredient = true;

    [Header("UI Control")]
    int x;

    private void Start()
    {
        gameManager = GameObject.Find("Game Manager");
        cauldron = GameObject.Find("Cauldron").GetComponent<Transform>();
    }
    private void Update()
    {
        DropInputCheck();

        // Checks if player let go of certain inputs to reset bools (input.getkeyup)
        ResetInput();
    }

    public void GiveIngredientToCauldron(Ingredient ing)
    {
        gameManager.GetComponent<GameManager>().IngredientAdded(ing);

        // Remove ingredient from cats mouth
        CatLosesIngredient();
        
    }

    // DROPPING
    #region Droping Items
    public void DropInputCheck()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            // Check what cat has in mouth to drop
            if(ingredientImHolding != null)
            {
                // Drop ingredient
                DropObject(ingredientImHolding.GetIngredientDroppedObject(), true);
                CatLosesIngredient();
            }
            else if(itemImHolding != null)
            {
                // Drop other item cat is holding
                DropObject(itemImHolding, false);
                CatLosesItem();
            }
            else
            {
                // There is nothing to drop
            }
        }
    }

    public void DropObject(GameObject dropableObj, bool isIngredient)
    {
        Vector3 droppedItemPos = new Vector3(itemHolding.position.x, itemHolding.position.y, itemHolding.position.z);
        GameObject droppedObject = Instantiate(dropableObj, droppedItemPos, Quaternion.identity);


        if (isIngredient)
        {
            // Activate its ingredient properties
        }
        else
        {
            // Nothing else happens if just normal object
        }
    }

    public void DropObject(GameObject dropableObj, bool isIngredient, float newDissapearTimeForDroppedObject)
    {
        Vector3 droppedItemPos = new Vector3(itemHolding.position.x, itemHolding.position.y, itemHolding.position.z);
        GameObject droppedObject = Instantiate(dropableObj, droppedItemPos, Quaternion.identity);
        droppedObject.GetComponent<IngredientDroppedObject>().ChangeDissapearTimer(newDissapearTimeForDroppedObject);


        if (isIngredient)
        {
            // Activate its ingredient properties
        }
        else
        {
            // Nothing else happens if just normal object
        }
    }

    // Force cat to drop, making the dropped item dissapear much faster
    public void DropObjectOnCommand()
    {
        /// Dropped item will dissapear much faster than normal

        if (ingredientImHolding != null)
        {
            // Drop ingredient
            DropObject(ingredientImHolding.GetIngredientDroppedObject(), true, 15);
            CatLosesIngredient();
        }
        else if (itemImHolding != null)
        {
            // Drop other item cat is holding
            DropObject(itemImHolding, false, 15);
            CatLosesItem();
        }
        else
        {
            // There is nothing to drop
        }
    }
    #endregion

    // PICKUP INGREDIENT
    private void OnTriggerEnter(Collider other)
    {
        // INTERACTABLE ITEMS
        if (other.gameObject.GetComponent<MoreTags>())
        {
            if (other.gameObject.GetComponent<MoreTags>().HasTag("InteractableIngredient"))
            {
                // Turn on control tip animation
                if (UIManager.instanceUIManager.PressInteractButtonAnim.GetBool("on") != true)
                {
                    UIManager.instanceUIManager.InteractButtonTipLever(true, "ingredient");
                }
            }

            if (other.gameObject.GetComponent<MoreTags>().HasTag("InteractableBook"))
            {
                // Turn on control tip animation
                if (UIManager.instanceUIManager.PressInteractButtonAnim.GetBool("on") != true)
                {
                    UIManager.instanceUIManager.InteractButtonTipLever(true, "book");
                }
            }

            if (other.gameObject.GetComponent<MoreTags>().HasTag("InteractableMortar"))
            {
                // Turn on control tip animation
                if (UIManager.instanceUIManager.PressInteractButtonAnim.GetBool("on") != true)
                {
                    UIManager.instanceUIManager.InteractButtonTipLever(true, "mortar");
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "IngredientPickup")
        {
            // INGREDIENT PICKUP
            if (other.gameObject.GetComponent<IngredientPickup>())
            {
                // Check if ingredient pickup can give its ingredient
                if (other.gameObject.GetComponent<IngredientPickup>().IsPickupable())
                {
                    // Now its possible for cat to get ingredient
                    GetIngredientInput(other.gameObject);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // INTERACTABLE ITEMS
        if (other.gameObject.GetComponent<MoreTags>())
        {
            if (other.gameObject.GetComponent<MoreTags>().HasTag("InteractableIngredient") ||
                other.gameObject.GetComponent<MoreTags>().HasTag("InteractableBook") ||
                other.gameObject.GetComponent<MoreTags>().HasTag("InteractableMortar"))
            {
                // Turn off control tip animation
                UIManager.instanceUIManager.InteractButtonTipLever(false);
            }
        }
    }

    public IEnumerator DelayToTurnControlAnimationOff()
    {
        yield return new WaitForSeconds(0.2f);

        // Turn off control tip animation
        UIManager.instanceUIManager.InteractButtonTipLever(false);
    }

    // GET INGREDIENT
    public void GetIngredientInput(GameObject ingredientPickupObject)
    {
        if (Input.GetKey(KeyCode.E) && canGetIngredient)
        {
            // Set bool to false so it only enters once per press
            canGetIngredient = false;

            IngredientPickup pickupObj = ingredientPickupObject.GetComponent<IngredientPickup>();

            GiveCatNewIngredient(pickupObj.GetIngredient());

            // Depending on the ingredient, some things might have to be triggered (respawn, etc)
            #region Ingredient specific behaviour
            switch (pickupObj.GetIngredient().GetIngredientName())
            {
                // If its the mouse, set flag so another one can spawn
                case "Magical Mouse":
                    MouseManager.mouseManagerInstance.CanMouseSpawnLever(true);
                    if (pickupObj.transform.parent != null)
                    {
                        Destroy(pickupObj.transform.parent.gameObject);
                    }
                    else
                    {
                        Destroy(pickupObj.gameObject);
                    }
                    break;
            }
            #endregion

            // Check if ingredient source is finite
            if (pickupObj.IsItInfiniteSource())
            {
                // Nothing happens because the source is infinite
            }
            else
            {
                // If source of ingredient is not infinite, destroy it
                Destroy(ingredientPickupObject);
            }

            // Turn off control tip animation
            StartCoroutine(DelayToTurnControlAnimationOff());
        }
    }

    public bool CatHasIngredient()
    {
        if (ingredientImHolding)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Ingredient GetCatIngredient()
    {
        return ingredientImHolding;
    }

    public void ResetInput()
    {
        // Reset bool so cat can press again
        if (Input.GetKeyUp(KeyCode.E))
        {
            canGetIngredient = true;
        }
    }

    public void GiveCatNewIngredient(Ingredient newIngredient)
    {
        // Drop what cat is holding
        DropObjectOnCommand();

        // Make it so the cat is holding new ingredient
        ingredientImHolding = newIngredient;

        // Instantiate ingredient in mouth of cat
        PutIngredientInMouth();
    }

    public void PutIngredientInMouth()
    {
        if (ingredientImHolding.GetIngredientObject() != null)
        {
            // If already holding something, destroy it to substitute
            if(itemHolding.childCount > 0)
            {
                Destroy(itemHolding.GetChild(0).gameObject);            
            }

            // Put current NEW ingredient in mouth
            GameObject newMouthObj = Instantiate(ingredientImHolding.GetIngredientObject(), itemHolding.position, Quaternion.identity);

            newMouthObj.GetComponent<IngredientObject>().ObjectInDecorativeMode(true);
            newMouthObj.transform.parent = itemHolding;
            newMouthObj.transform.localEulerAngles = ingredientImHolding.GetIngredientRotation();
        }
        else
        {
            DestroyItemInMouth();
        }
    }

    #region Losing Item/Ingredient Methods
    public void CatLosesIngredient()
    {
        ingredientImHolding = null;
        DestroyItemInMouth();
    }

    public void CatLosesItem()
    {
        itemImHolding = null;
        DestroyItemInMouth();
    }

    public void DestroyItemInMouth()
    {
        // Destroy what is on the cats mouth
        if (itemHolding.childCount > 0)
        {
            Destroy(itemHolding.GetChild(0).gameObject);
        }
    }
    #endregion
}
