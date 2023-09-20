using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamAnimationMethods : MonoBehaviour
{

    public void AtOptions()
    {
        MenuManager.menuManagerInstance.CanClickOptionsButtons();
    }

    public void AtMainMenu()
    {
        MenuManager.menuManagerInstance.CanClickMainMenuButtons();
    }
}
