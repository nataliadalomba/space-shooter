using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TextMeshProUGUI gameOverText;
    [SerializeField]
    private TextMeshProUGUI restartGameText;
    [SerializeField]
    private Image lifeImage;
    [SerializeField]
    private Sprite[] lifeSprites;

    private GameManager gameManager;

    void Start() {
        scoreText.text = "Score: " + 0;
        gameOverText.enabled = false;
        restartGameText.enabled = false;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public void UpdateScore(int playerScore) {
        scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives) {
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

    IEnumerator GameOverFlickerRoutine() {
        while(true) {
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
