using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Assertions;

public class UIManager : MonoBehaviour {

    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TextMeshProUGUI gameOverText;
    [SerializeField]
    private Image lifeImage;
    [SerializeField]
    private Sprite[] lifeSprites;

    void Start() {
        scoreText.text = "Score: " + 0;
        gameOverText.enabled = false;
    }

    public void UpdateScore(int playerScore) {
        scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives) {
        lifeImage.sprite = lifeSprites[currentLives];
    }

    public void GameOverDisplay() {
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
}
