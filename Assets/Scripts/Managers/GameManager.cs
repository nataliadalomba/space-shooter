using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private bool isGameOver;
    private int score = 0;

    public event Action onScoreChanged;

    public int Score {
        get { return score; }
        set {
            score = Mathf.Max(0, value);
            if (onScoreChanged != null)
                onScoreChanged();
        }
    }

    private void Start() {
        Enemy.onAnyDefeated += OnAnyEnemyDefeated;
    }

    private void OnDestroy() {
        Enemy.onAnyDefeated -= OnAnyEnemyDefeated;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R) && isGameOver)
            SceneManager.LoadScene(1); //game scene

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    private void OnAnyEnemyDefeated() {
        Score += 100;
    }

    public void GameOver() {
        isGameOver = true;
    }
}
