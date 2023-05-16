using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private bool isGameOver;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R) && isGameOver)
            SceneManager.LoadScene(1); //game scene
    }

    public void GameOver() {
        isGameOver = true;
    }

}
