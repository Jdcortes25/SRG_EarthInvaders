using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    [Tooltip("The Health Bar that will update depending on the current health")]
    [SerializeField] GameObject healthBar;

    [Tooltip("The Resources Bar that will update depending on the current resources")]
    [SerializeField] GameObject resourceBar;

    [Tooltip("The Resources Bar that will update depending on the current resources")]
    [SerializeField] GameObject gameOverScreen;

    [Tooltip("The text used to display the total amount of damage per second")]
    [SerializeField] TextMeshProUGUI damageValueText;

    [Tooltip("The text used to display the total amount of mining per second")]
    [SerializeField] TextMeshProUGUI miningValueText;

    [Tooltip("The text used to display how many ships have been destroyed")]
    [SerializeField] TextMeshProUGUI shipsDestroyedText;

    [Tooltip("The text used to display You Win or You Lose")]
    [SerializeField] TextMeshProUGUI gameOverMainText;

    [Tooltip("The amount of health the earth has at the start")]
    [SerializeField] float maxHealth;

    [Tooltip("The amount of resources the aliens need to gather")]
    [SerializeField] float targetResources;

    [Tooltip("The amount of ships to destroy in order to win")]
    [SerializeField] float shipsToDestroy;


    /// <summary>
    /// Reference to the AlienShipSpawner script in the scene
    /// </summary>
    AlienShipSpawner spawner;

    /// <summary>
    /// Reference to the UserControls script in the scene
    /// </summary>
    UserControls player;

    /// <summary>
    /// The currrent amount of health the earth has
    /// </summary>
    float currentHealth;

    /// <summary>
    /// The current amount of resources the aliens have gathered
    /// </summary>
    float currentResources;

    /// <summary>
    /// The total amount of damage the aliens are doing per second
    /// </summary>
    float totalDamage;

    /// <summary>
    /// The total amounf of resources being gathered per second
    /// </summary>
    float totalMining;

    /// <summary>
    /// Current amount of ships destroyed
    /// </summary>
    int shipsDestroyed;

    /// <summary>
    /// Set initial values and start the game
    /// </summary>
    void Start()
    {
        player = FindObjectOfType<UserControls>();
        spawner = FindObjectOfType<AlienShipSpawner>();
        StartGame();
    }

    /// <summary>
    /// Do the necessary setup to start the game
    /// </summary>
    public void StartGame()
    {
        //Disable the game over screen, reset values, Spawn inital ships and enable player input
        gameOverScreen.SetActive(false);
        currentHealth = maxHealth;
        currentResources = 0;
        UpdateHealth(0);
        UpdateResources(0);
        totalDamage = 0;
        totalMining = 0;
        shipsDestroyed = -1;
        AddShipDestroyed();
        spawner.SpawnInitialShips();
        player.SetControlsActive(true);
        StartCoroutine(DisplayTotals());
    }

    /// <summary>
    /// Stop the game and display the game over screen
    /// </summary>
    void GameOver(string winOrLoseText)
    {
        //Display the game over screen, stop all coroutines, stop the spawner and stop player input
        gameOverScreen.SetActive(true);
        gameOverMainText.text = "GAME OVER: " + winOrLoseText;
        StopAllCoroutines();
        spawner.Stop();
        player.SetControlsActive(false);
    }

    /// <summary>
    /// Update the current amount of health by substracing it with the passed value
    /// </summary>
    /// <param name="val"></param>
    public void UpdateHealth(float val)
    {
        //The passed value will be added to the toalDamage to calculate the total amount of damage being done per second
        //The health bar position will be update based on the percantage of the current health over the max health
        //The health bar has a starting position of 0 on the x and -250 is the x position if health reaches 0
        currentHealth -= val;
        totalDamage += val;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        float newPosX = -250 + (currentHealth / maxHealth)*250;

        healthBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(newPosX,0);

        //If health reaches 0 or less then initate GameOver
        if(currentHealth <= 0)
        {
            GameOver("YOU LOSE!");
        }
    }

    /// <summary>
    /// Update the current amout of resources that have been gathered by adding it with the passed value
    /// </summary>
    /// <param name="val"></param>
    public void UpdateResources(float val)
    {
        //The passed value will be added to the totalResources to calculate the total amount of resources being gathered per second
        //The resource bar position will be update based on the percantage of the current resource amount gathered over the target resource amount
        //The resource bar has a starting position of -250 on the x and 0 is the x position if resources reaches the target amount
        currentResources += val;
        totalMining += val;
        float newPosX = -250 + ((currentResources / targetResources) * 250);

        resourceBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(newPosX, 0);

        //If resources reaches the target amount or more then initate GameOver
        if (currentResources >= targetResources)
        {
            GameOver("YOU LOSE!");
        }
    }

    /// <summary>
    /// Every second display the total amount of damage of and resources being done by the aliens per second
    /// </summary>
    /// <returns></returns>
    IEnumerator DisplayTotals()
    {
        yield return new WaitForSeconds(1.0f);
        damageValueText.text = totalDamage + "/s";
        miningValueText.text = totalMining + "/s";

        //Reset the values in case a change has occured
        totalDamage = 0;
        totalMining = 0;

        //Display updated values next second
        StartCoroutine(DisplayTotals());
    }

    /// <summary>
    /// Call when a ship is destroyed to increment the counter
    /// </summary>
    public void AddShipDestroyed()
    {
        shipsDestroyed++;
        shipsDestroyedText.text = shipsDestroyed + "/" + shipsToDestroy;
        if(shipsDestroyed >= shipsToDestroy)
        {
            GameOver("YOU WIN!");
        }
    }

    /// <summary>
    /// Load the main menu scene
    /// </summary>
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
