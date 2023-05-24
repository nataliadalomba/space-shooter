using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private bool isGameOver;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R) && isGameOver)
            SceneManager.LoadScene(1); //game scene

        //if the escape key is pressed
        //quit application
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void GameOver() {
        isGameOver = true;
    }

}
