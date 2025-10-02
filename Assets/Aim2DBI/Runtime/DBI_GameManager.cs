using UnityEngine;
using UnityEngine.SceneManagement;

namespace A2P.DBI {
  public class DBI_GameManager : MonoBehaviour {
    public static DBI_GameManager I;
    [Header("Gameplay")]
    public int startingLives = 3;
    public int lives;
    public int score;
    public int highScore;
    public bool isGameOver;

    void Awake() {
      if (I != null) { Destroy(gameObject); return; }
      I = this;
      DontDestroyOnLoad(gameObject);
      highScore = PlayerPrefs.GetInt("DBI_HighScore", 0);
      ResetRun();
      SceneManager.sceneLoaded += (_, __) => { /* keep persistent; no-op */ };
    }

    public void ResetRun() {
      isGameOver = false;
      lives = startingLives;
      score = 0;
    }

    public void AddScore(int pts) {
      if (isGameOver) return;
      score += pts;
      if (score > highScore) {
        highScore = score;
        PlayerPrefs.SetInt("DBI_HighScore", highScore);
      }
    }

    public void LoseLife() {
      if (isGameOver) return;
      lives = Mathf.Max(0, lives - 1);
      if (lives <= 0) {
        isGameOver = true;
      }
    }

    public void RestartScene() {
      var s = SceneManager.GetActiveScene().name;
      ResetRun();
      SceneManager.LoadScene(s);
    }
  }
}
