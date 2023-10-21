using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instanceUIManager;

    [Header("Controls_Help")]
    public Animator PressInteractButtonAnim;
    [SerializeField] TextMeshProUGUI interactButtonText;

    [Header("Ingredient/Area_Names")]
    [SerializeField] List<GameObject> objNames = new List<GameObject>();
    [SerializeField] Animator nameCanvasGroupAnimator;

    [Header("Character_Portrait")]
    [SerializeField] GameObject portrait_Obj;
    [SerializeField] Image characterImage_Book;

    [Header("Start_Tutorial")]
    [SerializeField] GameObject TutorialObj;
    [SerializeField] GameObject page1;
    [SerializeField] GameObject page2;
    [SerializeField] GameObject StartGameButton;
    [SerializeField] GameObject CloseTutorialButton;

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
                    interactButtonText.text = "Pressione para pegar ingrediente";
                    break;
                case "book":
                    interactButtonText.text = "Pressione para ver receita";
                    break;
                case "mortar":
                    interactButtonText.text = "Pressione para moer ingrediente";
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

    // INGREDIENT/AREA NAME UI
    public IEnumerator ShowAreaName(int chosenTextIndex)
    {
        // Make only the right text appear
        for (int i = 0; i < objNames.Count; i++)
        {
            if(i == chosenTextIndex)
            {
                objNames[i].SetActive(true);
            }
            else
            {
                objNames[i].SetActive(false);
            }
        }

        // Start fade in
        nameCanvasGroupAnimator.ResetTrigger("fadeOut");
        nameCanvasGroupAnimator.SetTrigger("fadeIn");

        yield return new WaitForSeconds(2);

        // Fade away
        nameCanvasGroupAnimator.ResetTrigger("fadeIn");
        nameCanvasGroupAnimator.SetTrigger("fadeOut");
    }

    public void DissapearAreaName()
    {
        // Fade away
        nameCanvasGroupAnimator.ResetTrigger("fadeIn");
        nameCanvasGroupAnimator.SetTrigger("fadeOut");
    }


    // CHARACTER PORTRAIT
    public void CharacterPortraitOn()
    {
        portrait_Obj.SetActive(true);
    }

    public void CharacterPortraitOff()
    {
        portrait_Obj.SetActive(false);
    }


    // START TUTORIAL
    public void ChangePage()
    {
        page1.SetActive(false);
        page2.SetActive(true);

        // Depending if game has started or not, change which button appears
        if (GameManager.gameManagerInstance.HasGameStarted())
        {
            CloseTutorialButton.SetActive(true);
            StartGameButton.SetActive(false);
        }
        else
        {
            StartGameButton.SetActive(true);
            CloseTutorialButton.SetActive(false);
        }
    }

    public void OpenTutorial()
    {
        TutorialObj.SetActive(true);

        page1.SetActive(true);
        page2.SetActive(false);

        // Make it so the player cant interact with anything else besides the tutorial

    }

    public void CloseTutorial()
    {
        TutorialObj.SetActive(false);

        // Player can exit menu again
    }
}
