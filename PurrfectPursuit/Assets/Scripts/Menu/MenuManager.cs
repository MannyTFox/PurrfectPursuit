using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public static MenuManager menuManagerInstance;

    bool canPressMainMenuButtons = false;
    bool atMainMenu = true;
    bool atOptions = false;

    // ----

    [SerializeField] Button PlayButton;
    [SerializeField] Button OptionsButton;
    [SerializeField] Button ExitButton;
    [Space(10)]
    [SerializeField] Canvas mainCanvas;
    Animator mainCamAnimator;

    [Header("Level Manager")]
    [SerializeField] GameObject levelSelectObj;
    MainMenuLevelManager levelManager;
    bool atLevelSelect = false;

    private void Awake()
    {
        menuManagerInstance = this.GetComponent<MenuManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<MainMenuLevelManager>();
        mainCamAnimator = Camera.main.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMainMenuButtons();
    }

    public void UpdateMainMenuButtons()
    {
        if (canPressMainMenuButtons)
        {
            PlayButton.interactable = true;
            OptionsButton.interactable = true;
            ExitButton.interactable = true;
        }
        else
        {
            PlayButton.interactable = false;
            OptionsButton.interactable = false;
            ExitButton.interactable = false;
        }

        #region Getting mouse Pos on UI screen
        Vector2 movePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            mainCanvas.transform as RectTransform,
            Input.mousePosition, mainCanvas.worldCamera,
            out movePos);

        Vector2 mousePos = mainCanvas.transform.TransformPoint(movePos);
        
        // DEBUG
        //print(mousePos);
        #endregion

        #region Checking mouse Pos
        bool MouseXPosCheck = false;
        bool MouseYPosCheck = false;

        if(mousePos.x < 60 && mousePos.x > -60)
        {
            MouseXPosCheck = true;
        }
        else
        {
            MouseXPosCheck = false;
        }

        if(mousePos.y < 20 && mousePos.y > -60) 
        {
            MouseYPosCheck = true;
        }
        else
        {
            MouseYPosCheck = false;
        }

        if (MouseXPosCheck && MouseYPosCheck && atMainMenu && atLevelSelect == false)
        {
            canPressMainMenuButtons = true;
        }
        else
        {
            canPressMainMenuButtons = false;
        }
        #endregion
    }

    public void CallMenuMethod(string methodID)
    {
        if (canPressMainMenuButtons)
        {
            switch (methodID)
            {
                case "Play":
                    Play();
                    break;

                case "Options":
                    Options();
                    break;

                case "Exit":
                    Exit();
                    break;
            }
        }
    }

    // PLAY BUTTON
    public void Play()
    {
        atLevelSelect = true;

        levelSelectObj.SetActive(true);
    }

    public void ExitLevelSelect()
    {
        atLevelSelect = false;

        levelSelectObj.SetActive(false);
    }

    // OPTIONS BUTTON
    public void Options()
    {
        atMainMenu = false;

        mainCamAnimator.SetTrigger("toOptions");
    }

    public void CanClickOptionsButtons()
    {
        atOptions = true;
    }

    public void CanClickMainMenuButtons()
    {
        atMainMenu = true;
    }

    public void GoBackToMainMenu()
    {
        atOptions = false;

        mainCamAnimator.SetTrigger("toMainMenu");
    }

    // EXIT BUTTON
    public void Exit()
    {
        print("exit method");
        Application.Quit();
    }
}
