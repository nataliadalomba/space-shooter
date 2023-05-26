using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour {
    //responsible to launch the game

    public void LoadGame() {
        SceneManager.LoadScene(1); //game scene
    }

    public void ExitApplication() {
        Application.Quit();
    }
}
