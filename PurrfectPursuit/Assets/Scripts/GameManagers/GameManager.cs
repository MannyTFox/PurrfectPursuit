using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance;

    [Header("List of potions game can pick from")]
    public List<PotionRecipe> recipes = new List<PotionRecipe>();

    [Header("Game Started")]
    bool gameStarted = false;

    [Header("UI references")]
    public TextMeshProUGUI dayTimer;
    [SerializeField] TextMeshProUGUI potionPointsText;
    [SerializeField] Image potionImage;
    [SerializeField] Sprite potionTransitionImage;

    [Header("Pause_References")]
    [SerializeField] TextMeshProUGUI pauseText;
    [SerializeField] GameObject pauseMenuObj;
    bool pauseMenuOpen = false;

    [Header("Debug only")]
    public PotionRecipe currentRecipe;
    PotionRecipe lastRecipe;
    public List<Ingredient> ingredientsIwant;
    public int ingredientsLeft = 0; // pointer used by this script to know how far along a recipe we are
    bool finished;

    public Image timeBar;
    float time = 0;
    bool timerRunning = false;
    [Space(10)]
    [SerializeField] float timeMax = 150;
    bool congratulating = false; // to avoid calling congratulate more than once

    [Header ("Tally References")]

    GameObject tallyPanel;
    Animator witchAnimator;

    public TextMeshProUGUI potionsSoldText;
    public TextMeshProUGUI potionsPointsTallyText;
    public TextMeshProUGUI potionsSoldHighscoreTallyText;
    [SerializeField] Image potionMeterFillImage;
    [SerializeField] Image potionChallengeImage1;
    [SerializeField] Image potionChallengeImage2;
    [SerializeField] Image potionChallengeImage3;

    /// <summary>
    ///  Each level has a threshold of potions to be sold each day to unlock the next Day/Challenge
    /// </summary>
    [Header("Day_Over")]
    [SerializeField] int potionsSold; // # of potions sold on the current day
    [SerializeField] int potionPoints;
    [SerializeField] int potionHighscore;
    [SerializeField] int potionChallenge1 = 2;
    [SerializeField] int potionChallenge2 = 3; // = howManyPotionsToBeatLevel
    [SerializeField] int potionChallenge3 = 4;
    float meterFillAmount = 0;
    bool dayOverOnce = true;
    bool canUpdatePotionMeter = false;
    bool setFillMeterAmountOnce = true;
    bool challenge1completed = false;
    bool challenge2completed = false;
    bool challenge3completed = false;

    [Header("Highscore_Sign")]
    [SerializeField] TextMeshProUGUI highscoreSignText;

    [Header("Player Reference")]
    public PlayerMovement playerMov;
    public Transform playerTransform;
    public CinemachineBrain playerCameraBrain;

    [Header("Witch References")]
    public Cauldron cauldronScript;
    [SerializeField] BookBehaviour book;

    [Header("Tutorial References")]
    public GameObject startTutorialRef;

    [Header("Day Presentation")]
    public Animator dayPresentatorAnim;
    [SerializeField] TextMeshProUGUI dayPresentatorText;
    public string dayTitleMain;

    private void Awake()
    {
        gameManagerInstance = this.GetComponent<GameManager>();
    }

    private void Start()
    {
        witchAnimator = GameObject.Find("Witch").GetComponent<Animator>();

        // ---

        // Check player prefs
        CheckSceneToSelectPrefs(SceneManager.GetActiveScene().name);

        // ---

        ChangePotion();

        // Set highscore to beat in the garden sign
        highscoreSignText.text = PlayerPrefs.GetInt(Prefs.DemoHighScore).ToString();

        // Deactivate tally panel
        tallyPanel = GameObject.Find("Tally Panel");
        tallyPanel.SetActive(false);

        // Set game time limit
        time = timeMax;

        // Make transition go away
        StartCoroutine(GameLevelManager.levelManagerInstance.DelayToTransitionOut());

        // Cursor is visible to skip tutorial
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // START OF GAME METHODS ------------
    public void GameStart()
    {
        gameStarted = true;

        // Tutorial Dissapears
        startTutorialRef.SetActive(false);

        // Start timer of the day countdown
        timerRunning = true;

        // Free player, allowing them to move
        playerMov.UnlockPlayer();
        // enable camera
        playerCameraBrain.enabled = true;
        
        // Cursor becomes invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // ---

        // After a little bit of time, show level title and start music
        StartCoroutine(DelayToPresentDay());
    }

    public bool HasGameStarted()
    {
        if (gameStarted)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator DelayToPresentDay()
    {
        yield return new WaitForSeconds(1);

        if (dayPresentatorText.text == "")
        {
            dayPresentatorText.text = "New Day!";
        }
        else
        {
            dayPresentatorText.text = dayTitleMain;
        }

        dayPresentatorAnim.SetTrigger("dayPresentation");

        // Start main music theme
        AudioManager.audioManagerInstance.PlayMainMusic();
    }

    
    // PAUSE GAME --------
    public void PauseGame()
    {
        // Stop cat movement
        playerMov.LockPlayer();

        // Stop camera movement
        playerCameraBrain.enabled = false;

        // Stop game timer from running
        timerRunning = false;
    }

    public void UnpauseGame()
    {
        // Cat movement enabled
        playerMov.UnlockPlayer();

        // Camera movement enabled
        playerCameraBrain.enabled = true;

        // Game timer starts running again
        timerRunning = true;
    }


    // OPEN MENU
    public void OpenMenu()
    {
        PauseGame();

        // Paused text appears
        pauseText.gameObject.SetActive(true);

        // Set bool for methods to check
        pauseMenuOpen = true;

        // Menu appears
        pauseMenuObj.SetActive(true);

        // Make open menu sound
        AudioManager.audioManagerInstance.OpenMenuSound();

        // Cursor appears
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseMenu()
    {
        // Menu dissapears
        pauseMenuObj.SetActive(false);

        // Paused text disappears
        pauseText.gameObject.SetActive(false);

        print("menu closed");
        // Bool is set to false
        pauseMenuOpen = false;

        // Cursor hides away
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        UnpauseGame();
    }

    public void CheckMenuInput()
    {
        // If menu is not open and game is already onGoing
        if (Input.GetKey(KeyCode.Escape) && pauseMenuOpen == false && HasGameStarted() &&
            GameLevelManager.levelManagerInstance.IsTransitioning() == false)
        {
            OpenMenu();
        }
    }


    // UPDATE STUFF ---------------------
    public void UpdatePotionPointsText()
    {
        if (int.Parse(potionPointsText.text) < 10)
        {
            potionPointsText.text = "0" + potionPoints.ToString();
        }
        else
        {
            potionPointsText.text = potionPoints.ToString();
        }
    }


    // GAME TIMER RELATED METHODS -------
    void checkTime()
    {
        //dayTimer.text = time.ToString();
        timeBar.fillAmount = time / timeMax;

        if (time <= 0 && dayOverOnce)
        {
            dayOverOnce = false;

            DayOver();
        }
    }

    public void BetterTimer()
    {
        if (timerRunning)
        {
            time -= Time.deltaTime;
        }
        else
        {

        }
    }

    void Update()
    {
        // Day time update
        BetterTimer();
        checkTime();

        // Input to open menu
        CheckMenuInput();

        // Update UI that shows player the amount of points he has
        UpdatePotionPointsText();
        
        if(ingredientsLeft > 0)
        {
            // Potion not yet finished
             
        }
        else if(ingredientsLeft <= 0 && !congratulating)
        {
            // Potion is finished
            potionsSold += 1;
            potionPoints += currentRecipe.GetPotionValue();

            // Check if player beat highscore!
            if(potionPoints > potionHighscore)
            {
                PlayerPrefs.SetInt(Prefs.DemoHighScore, potionPoints);
                potionHighscore = PlayerPrefs.GetInt(Prefs.DemoHighScore);
                PlayerPrefs.Save();
            }

            StartCoroutine(Congratulate());
        }

        // When day is over
        if (canUpdatePotionMeter)
        {
            UpdatePotionMeter();
        }

        #if UNITY_EDITOR
        // DEBUG INPUTS
        if (Input.GetKey(KeyCode.T))
        {
            challenge1completed = true;
            challenge2completed = true;
            challenge3completed = true;
            meterFillAmount = 1;
            DayOver();
        }
        #endif

    }


    // GAME METHODS -----------
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
        // Spawn Ingredient Object on top of Cauldron
        if (ing.GetIngredientObject() != null)
        {
            cauldronScript.SpawnIngredient(ing.GetIngredientObject());
        }

        // Wait a little bit for the ingredient to fall into cauldron
        yield return new WaitForSeconds(2);

        // Result
        
        // Correct ingredient
        if (lever)
        {
            // Good particle effect plays
            cauldronScript.CorrectIngredientBehaviour();

            // Ingredient is removed, potion progress & time reward
            witchAnimator.SetTrigger("ingredientAdd");
            ingredientsLeft -= 1;
            time += 3;
            ingredientsIwant.Remove(ing);

            // Play good feedbacksound
            AudioManager.audioManagerInstance.PlayIngredientFeedbackAudio(true);

            // Book gets references and makes new holograms
            book.UpdateHolograms();
        }
        // Wrong ingredient 
        else
        {
            // Bad particle effect plays
            cauldronScript.WrongIngredientBehaviour();

            // Witch animation
            witchAnimator.SetTrigger("ingredientAdd");

            // Play bad feedbacksound
            AudioManager.audioManagerInstance.PlayIngredientFeedbackAudio(false);

            // Wrong ingredient penalty
            time -= 10;
        }

        // Chance to Play book special voice interaction
        CharacterAudioManager.charAudioManInstance.ChanceToPlayBookClip(ing.GetIngredientName());
    }

    IEnumerator Congratulate()
    {
        // Money particle because potion was sold!

        // Change potion and animations
        congratulating = true;

        yield return new WaitForSeconds(0.14f);


        // Witch animation
        witchAnimator.SetTrigger("potionFinalized");

        // Change thinking bubble to different image
        potionImage.sprite = potionTransitionImage;


        yield return new WaitForSeconds(3f);

        ChangePotion();
        congratulating = false;
        
    }

    public void ChangePotion()
    {
        // Do not repeat recipe!
        PotionRecipe newRecipe = recipes[Random.Range(0, recipes.Count)];

        while (newRecipe == currentRecipe)
        {
            newRecipe = recipes[Random.Range(0, recipes.Count)];
        }

        currentRecipe = newRecipe;
        ingredientsIwant = new List<Ingredient>(currentRecipe.GetPotionIngredients());
        ingredientsLeft = ingredientsIwant.Count;

        // Update witch thinking bubble
        potionImage.sprite = currentRecipe.GetPotionImage();

        // Book gets references and makes new holograms
        book.UpdateHolograms();
    }


    // LEVEL/DAY END ---------------
    public void DayOver()
    {
        PauseGame();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        
        // Show tally screen, save balance.

        potionsSoldText.text = ("Poções Vendidas: " + potionsSold.ToString());
        potionsPointsTallyText.text = ("Pontos de Poção: " + potionPoints.ToString());
        potionsSoldHighscoreTallyText.text = ("Pontos de Poção Highscore: " + PlayerPrefs.GetInt(Prefs.DemoHighScore));


        // Make tally panel appear
        tallyPanel.gameObject.SetActive(true);

        // Allow Meter to update
        canUpdatePotionMeter = true;

        // Filling sound starts
        AudioManager.audioManagerInstance.PotionMeterFillingSoundOn();
    }

    public void UpdatePotionMeter()
    {
        // Meter starts to fill in, and, depending on the score of the player, stop at a certain amount and turn images on
        if (setFillMeterAmountOnce)
        {
            setFillMeterAmountOnce = false;

            if (potionPoints >= potionChallenge1 && potionPoints < potionChallenge2)
            {
                meterFillAmount = 0.3f;

                challenge1completed = true;
            }
            else if (potionPoints >= potionChallenge2 && potionPoints < potionChallenge3)
            {
                meterFillAmount = 0.6f;

                challenge1completed = true;
                challenge2completed = true;
            }
            else if (potionPoints >= potionChallenge3)
            {
                meterFillAmount = 1f;

                challenge1completed = true;
                challenge2completed = true;
                challenge3completed = true;
            }

            // Save in player prefs the progress on this level
            #region Challenge Playerprefs
            // if any challenge was completed
            if (challenge1completed == true || challenge2completed == true || challenge3completed == true)
            {
                // If the prefs already exist, compare to save
                if (PlayerPrefs.HasKey(Prefs.DemoProgress))
                {
                    if (challenge3completed && PlayerPrefs.GetInt(Prefs.DemoProgress) < 3)
                    {
                        PlayerPrefs.SetInt(Prefs.DemoProgress, 3);
                    }
                    else if (challenge2completed && PlayerPrefs.GetInt(Prefs.DemoProgress) < 2)
                    {
                        PlayerPrefs.SetInt(Prefs.DemoProgress, 2);
                    }
                    else if (challenge1completed && PlayerPrefs.GetInt(Prefs.DemoProgress) < 1)
                    {
                        PlayerPrefs.SetInt(Prefs.DemoProgress, 1);
                    }

                    PlayerPrefs.Save();
                }
                // If its the first time the player is completing the level
                else
                {
                    if (challenge3completed)
                    {
                        PlayerPrefs.SetInt(Prefs.DemoProgress, 3);
                    }
                    else if (challenge2completed)
                    {
                        PlayerPrefs.SetInt(Prefs.DemoProgress, 2);
                    }
                    else if (challenge1completed)
                    {
                        PlayerPrefs.SetInt(Prefs.DemoProgress, 1);
                    }

                    PlayerPrefs.Save();
                }
            }
            else
            {
                // if no challenge was completed, there is nothing to save
            }
            #endregion
        }

        if (potionMeterFillImage.fillAmount >= meterFillAmount && canUpdatePotionMeter)
        {
            // Finished filling up!
            canUpdatePotionMeter = false;

            // Stop filling sound
            AudioManager.audioManagerInstance.PotionMeterFillingSoundOff();
            // Cheer sound
            AudioManager.audioManagerInstance.CheerSoundPlay();
        }
        else if(potionMeterFillImage.fillAmount < meterFillAmount)
        {
            potionMeterFillImage.fillAmount += Time.deltaTime * 0.5f;
        }

        if(potionMeterFillImage.fillAmount >= 0.3f)
        {
            potionChallengeImage1.color = Color.white;

            // Play challenge completed sound
            if (challenge1completed == true)
            {
                challenge1completed = false;

                // Filling sound starts
                AudioManager.audioManagerInstance.PlayChallengeCompletedSound();
            }
        }

        if (potionMeterFillImage.fillAmount >= 0.6f)
        {
            potionChallengeImage2.color = Color.white;

            // Play challenge completed sound
            if (challenge2completed == true)
            {
                challenge2completed = false;

                // Filling sound starts
                AudioManager.audioManagerInstance.PlayChallengeCompletedSound();
            }
        }

        if (potionMeterFillImage.fillAmount >= 1f)
        {
            potionChallengeImage3.color = Color.white;

            // Play challenge completed sound
            if (challenge3completed == true)
            {
                challenge3completed = false;

                // Filling sound starts
                AudioManager.audioManagerInstance.PlayChallengeCompletedSound();
            }
        }
    }

    // Gets
    public Transform GetPlayerTransform()
    {
        return playerTransform;
    }

    public PotionRecipe GetCurrentRecipe()
    {
        return currentRecipe;
    }

    public List<Ingredient> GetRemainingIngredients()
    {
        return ingredientsIwant;
    }

    // PREFS

    public void CheckSceneToSelectPrefs(string sceneName)
    {
        switch (sceneName)
        {
            case "Demo1":
                potionHighscore = PlayerPrefs.GetInt(Prefs.DemoHighScore);
                break;
            default:
                potionHighscore = 0;
                Debug.LogWarning("Scene name not found, no highscore set");
                break;
        }
    }

}
