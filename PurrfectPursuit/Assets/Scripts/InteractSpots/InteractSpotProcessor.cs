using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSpotProcessor : MonoBehaviour
{
    /// <summary>
    /// When player enters, call method
    /// </summary>

    [SerializeField] IngredientProcessor processor;
    [SerializeField] ParticleSystem interactSpotParticle;
    [SerializeField] Color hasToInteractColor;
    [SerializeField] Color justWalkColor;
    [SerializeField] Renderer gradientAreaRenderer;
    [SerializeField] Material hasToInteractMaterial;
    [SerializeField] Material justWalkInMaterial;
    MoreTags mtags;

    bool playerIsIn = false;
    bool playerCanPress = true;

    private void Start()
    {
        mtags = GetComponent<MoreTags>();
    }

    private void Update()
    {
        if (processor.IsIngredientReady())
        {
            gradientAreaRenderer.material = justWalkInMaterial;
        }
        else
        {
            gradientAreaRenderer.material = hasToInteractMaterial;
        }

        if (processor.IsMachineProcessing())
        {
            // Change tag temporarily so input text doenst appear on screen
            mtags.ChangeTag(0, "noTag");
        }
        else
        {
            // Change tag back to normal so input appears
            mtags.ChangeTag(0, "InteractableMortar");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<CatInventory>())
        {
            playerIsIn = true;

            // If processor is done, player only needs to walk in to get ingredient
            if (processor.IsIngredientReady())
            {
                processor.PlayerEntersMethod(other.GetComponent<CatInventory>());
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<CatInventory>())
        {
            if (Input.GetKey(KeyCode.E) && playerCanPress && processor.IsIngredientReady() == false 
                && processor.DoesPlayerHaveValidIngredient(other.GetComponent<CatInventory>()))
            {
                playerCanPress = false;

                processor.PlayerEntersMethod(other.GetComponent<CatInventory>());

                // Make ui dissappear
                UIManager.instanceUIManager.InteractButtonTipLever(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerIsIn = false;
        playerCanPress = true;
    }
}
