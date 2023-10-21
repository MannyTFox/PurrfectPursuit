using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

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

    [Header("Options_Menu")]
    [SerializeField] Toggle fullscreenToggle;

    [Space(10)]
    [SerializeField] TMP_Dropdown resolutionsDropdown;
    Resolution[] resolutions;

    [Space(10)]
    [SerializeField] GameObject areYouSureBox;


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
        // Get references
        levelManager = GameObject.Find("LevelManager").GetComponent<MainMenuLevelManager>();
        mainCamAnimator = Camera.main.GetComponent<Animator>();


        // Depending on player prefs, set them at start ----

        // fullscreen
        if (PlayerPrefs.HasKey(Prefs.FullscreenOption))
        {
            SetFullscreenToggleAtStart();
        }
        else
        {
            fullscreenToggle.isOn = false;
        }

        // resolutions
        GetResolutionsAtStart();

        // Make transition go away
        StartCoroutine(MainMenuLevelManager.levelManInstance.DelayToTransitionOut());
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
        if (MainMenuLevelManager.levelManInstance.IsTransitioning() == false)
        {
            switch (methodID)
            {
                
                case "Play":
                    if (canPressMainMenuButtons)
                    {
                        Play();
                    }
                    break;

                case "Options":
                    if (canPressMainMenuButtons)
                    {
                        Options();
                    }
                    break;

                case "Exit":
                    if (canPressMainMenuButtons)
                    {
                        Exit();
                    }
                    break;

                case "Exit_Level_Select":
                    ExitLevelSelect();
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


    // SOUND
    public void MainMenuButtonHoverSound()
    {
        // This method exists because when Level Selector is open they shouldnt make hover sound
        if (atLevelSelect == false && canPressMainMenuButtons)
        {
            AudioManagerMenu.audioManagerinstance.ButtonHoverSound();
        }
    }


    // OPTIONS MENU -------

    // Fullscreen
    public void FullscreenToggle(bool toggleBool)
    {
        Screen.fullScreen = toggleBool;

        if (toggleBool) 
        {
            PlayerPrefs.SetInt(Prefs.FullscreenOption, 1);
        }
        else
        {
            PlayerPrefs.SetInt(Prefs.FullscreenOption, 0);
        }

        PlayerPrefs.Save();
    }

    public void SetFullscreenToggleAtStart()
    {
        // If fullscreen true
        if(PlayerPrefs.GetInt(Prefs.FullscreenOption) == 1)
        {
            Screen.fullScreen = true;
            fullscreenToggle.isOn = true;
        }
        else
        {
            Screen.fullScreen = false;
            fullscreenToggle.isOn = false;
        }
    }

    // Resolutions
    public void GetResolutionsAtStart()
    {
        // Get all resolutions that player has on his screen
        resolutions = Screen.resolutions;

        resolutionsDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        // Pass all of the resolutions to a list, so you can pass them on the resolution dropdown menu
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            // Save the current resolution that player is using as default option
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height &&
                resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                currentResolutionIndex = i;
            }
        }

        // Add options to dropDown menu and display the correct options
        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.RefreshShownValue();


        // If there are saved prefs, set them
        if (PlayerPrefs.HasKey(Prefs.ResolutionWidth))
        {
            // Make dropdown show correct value

            // Get correct res based on prefs
            int correctRes = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                if(PlayerPrefs.GetInt(Prefs.ResolutionWidth) == resolutions[i].width &&
                    PlayerPrefs.GetInt(Prefs.ResolutionHeight) == resolutions[i].height)
                {
                    correctRes = i;
                }
            }

            // Set it to dropdown to show correct value
            resolutionsDropdown.value = correctRes;

            // Change resolution
            Resolution resolution = resolutions[correctRes];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
        else
        {
            // Dropdown shows default value based on screen of player
            resolutionsDropdown.value = currentResolutionIndex;
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        PlayerPrefs.SetInt(Prefs.ResolutionWidth, resolution.width);
        PlayerPrefs.SetInt(Prefs.ResolutionHeight, resolution.height);

        PlayerPrefs.Save();
    }


    // Reset Progress
    public void ResetProgressButton()
    {
        // Make are you sure box appear
        areYouSureBox.SetActive(true);
    }

    public void NoDontResetButton() 
    {
        // Make are you sure box disappear
        areYouSureBox.SetActive(false);
    }

    public void YesResetButton()
    {
        // reset progress and make are you sure box dissapear
        // demo progress reset
        PlayerPrefs.SetInt(Prefs.DemoProgress, 0);
        PlayerPrefs.SetInt(Prefs.DemoHighScore, 0);
        PlayerPrefs.Save();

        areYouSureBox.SetActive(false);

        // Make delete progress sound
    }
}
