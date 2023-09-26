using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSpot : MonoBehaviour
{

    /// <summary>
    /// If player is standing inside, can press button and do action of interactSpot. If player leaves the space, can cancel 
    /// the action.
    /// </summary>

    [SerializeField] BookBehaviour book;
    [SerializeField] bool pressedButton = false;
    bool playerIn = false;

    private void Update()
    {
        if (Input.GetKey(KeyCode.E) && pressedButton == false && playerIn)
        {
            pressedButton = true;
            
            book.ShowHolograms();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerIn = false;

            // Reset bool
            pressedButton = false;

            book.StopShowingHolograms();
        }
    }

    public bool IsPlayerIn()
    {
        if (playerIn)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
