using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instanceUIManager;

    [Header("References")]
    public Animator PressInteractButtonAnim;
    [SerializeField] TextMeshProUGUI interactButtonText;

    private void Awake()
    {
        instanceUIManager = this.GetComponent<UIManager>();
    }

    // CONTROLS HELP UI
    public void InteractButtonTipLever(bool lever, string textID)
    {
        if (lever)
        {
            PressInteractButtonAnim.SetBool("on", true);
        }
        else
        {
            PressInteractButtonAnim.SetBool("on", false);
        }

        if (lever)
        {
            switch (textID)
            {
                case "ingredient":
                    interactButtonText.text = "Press to pick up ingredient";
                    break;
                case "book":
                    interactButtonText.text = "Press to check recipe";
                    break;
                case "mortar":
                    interactButtonText.text = "Press to grind ingredient";
                    break;
            }
        }
    }

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
