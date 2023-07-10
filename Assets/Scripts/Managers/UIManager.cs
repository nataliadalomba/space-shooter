using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using TMPro;

//TODO: Break this into separate scripts:
//  1. HealthUI
//  2. LaserUI
//  3. GameOverUI
//  4. ScoreText
public class UIManager : MonoBehaviour {
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI restartGameText;
    [SerializeField] private Image lifeImage;
    [SerializeField] private Sprite[] lifeSprites;
    [SerializeField] private TMP_Text laserCount;
    [SerializeField] private Animator laserCountAnim;
    
    private GameManager gameManager;

    private GameObject player;
    private ShootController playerShootController;
    private HealthEntity playerHealth;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {
            playerShootController = player.GetComponent<ShootController>();
            if (playerShootController != null)
                playerShootController.onAmmoCountChanged += UpdateAmmoCount;
            else
                Debug.Log("The " + nameof(playerShootController) + " is null");

            playerHealth = player.GetComponent<HealthEntity>();
            if (playerHealth != null) {
                playerHealth.onDamaged += OnPlayerDamaged;
                playerHealth.onHealed += OnPlayerHealed;
            } else {
                Debug.Log("The " + nameof(playerHealth) + " is null");
            }
        } else {
            Debug.LogError("The player is null.");
        }

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        scoreText.text = "Score: " + 0;

        laserCount.text = playerShootController.AmmoCount.ToString();
        laserCountAnim.SetInteger("Laser Count", playerShootController.AmmoCount);
        gameOverText.enabled = false;
        restartGameText.enabled = false;

        if (gameManager != null) {
            gameManager.onScoreChanged += UpdateScore;
        } else
            Debug.Log("The game manager is null.");
    }

    private void OnDestroy() {
        gameManager.onScoreChanged -= UpdateScore;
        if (playerShootController != null)
            playerShootController.onAmmoCountChanged -= UpdateAmmoCount;
        if (playerHealth != null) {
            playerHealth.onDamaged -= OnPlayerDamaged;
            playerHealth.onHealed -= OnPlayerHealed;
        }
    }

    private void UpdateAmmoCount() => UpdateAmmoCount(playerShootController.AmmoCount);
    public void UpdateAmmoCount(int lasers) {
        laserCount.text = lasers.ToString();
        laserCountAnim.SetInteger("Laser Count", lasers);
    }

    private void UpdateScore() => UpdateScore(gameManager.Score);
    public void UpdateScore(int playerScore) {
        scoreText.text = "Score: " + playerScore.ToString();
    }

    private void OnPlayerDamaged() {
        int health = playerHealth.Health;
        UpdateHealth(health); //NOTE: This may benefit from onHealthChanged
        if (health <= 0)
            GameOverSequence();
    }

    private void OnPlayerHealed() {
        int health = playerHealth.Health;
        UpdateHealth(health); //NOTE: This may benefit from onHealthChanged
    }

    public void UpdateHealth(int currentLives) {
        if (currentLives < 0)
            currentLives = 0;
        lifeImage.sprite = lifeSprites[currentLives];
    }

    private void GameOverDisplay() {
        gameOverText.enabled = true;
        Assert.IsTrue(gameOverText.isActiveAndEnabled, "The Game Over text must be both " +
            "active and enabled for the text to show!");
        StartCoroutine(GameOverFlickerRoutine());
    }

    private IEnumerator GameOverFlickerRoutine() {
        while (true) {
            gameOverText.enabled = true;
            yield return new WaitForSeconds(0.5f);
            gameOverText.enabled = false;
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void RestartDisplay() {
        restartGameText.enabled = true;
        Assert.IsTrue(restartGameText.isActiveAndEnabled, "The Restart Game text must be "
            + "both active and enabled for the text to show!");
    }

    public void GameOverSequence() {
        gameManager.GameOver();
        GameOverDisplay();
        RestartDisplay();
    }
}