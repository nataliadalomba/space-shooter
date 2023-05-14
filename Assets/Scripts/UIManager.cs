using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {

    [SerializeField]
    private TMP_Text scoreText;

    void Start() {
        scoreText.text = "Score: " + 0;
    }

    public void UpdateScore(int playerScore) {
        scoreText.text = "Score: " + playerScore.ToString();
    }
}
