using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instanceUIManager;

    [Header("References")]
    public Animator PressInteractButtonAnim;

    private void Awake()
    {
        instanceUIManager = this.GetComponent<UIManager>();
    }


    // CONTROLS HELP UI
    public void InteractButtonTipLever(bool lever)
    {
        if (lever)
        {
            PressInteractButtonAnim.SetBool("on", true);
        }
        else
        {
            PressInteractButtonAnim.SetBool("on", false);
        }
    }
}
