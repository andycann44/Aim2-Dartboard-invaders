using UnityEngine;

namespace A2P.DBI {
  public class DBI_HUDOverlay : MonoBehaviour {
    Rect tl = new Rect(10,10,600,30);
    Rect tr = new Rect(Screen.width-210,10,200,30);
    Rect mid = new Rect(Screen.width/2-200, Screen.height/2-20, 400, 40);

    void OnGUI() {
      if (DBI_GameManager.I == null) return;

      var prev = GUI.color;
      GUI.color = Color.white;

      GUI.Label(tl, $"Score: {DBI_GameManager.I.score}   High: {DBI_GameManager.I.highScore}");
      GUI.Label(tr, $"Lives: {DBI_GameManager.I.lives}");

      if (DBI_GameManager.I.isGameOver) {
        GUI.color = Color.red;
        GUI.Label(mid, "GAME OVER — Press R to Restart");
        GUI.color = prev;
      } else {
        GUI.color = prev;
      }
    }

    void Update() {
      if (DBI_GameManager.I != null && DBI_GameManager.I.isGameOver && (Input.GetKeyDown(KeyCode.R))) {
        DBI_GameManager.I.RestartScene();
      }
    }
  }
}
