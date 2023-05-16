using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
    //responsible to launch the game

    public void LoadGame() {
        SceneManager.LoadScene(1); //game scene
    }
}
