using System.Collections;
using UnityEngine;

namespace A2P.DBI {
  public class DBI_PlayerLives : MonoBehaviour {
    public float invulnSeconds = 1.25f;
    bool invuln;
    SpriteRenderer sr;

    void Awake() {
      sr = GetComponentInChildren<SpriteRenderer>();
    }

    public void HitByEnemyBullet() {
      if (DBI_GameManager.I == null || DBI_GameManager.I.isGameOver) return;
      if (invuln) return;
      DBI_GameManager.I.LoseLife();
      if (DBI_GameManager.I.isGameOver) {
        // Hide player on game over; restart handled by HUD
        gameObject.SetActive(false);
      } else {
        StartCoroutine(InvulnFlash());
      }
    }

    IEnumerator InvulnFlash() {
      invuln = true;
      float t=0f;
      while (t < invulnSeconds) {
        t += 0.1f;
        if (sr != null) sr.enabled = !sr.enabled;
        yield return new WaitForSeconds(0.1f);
      }
      if (sr != null) sr.enabled = true;
      invuln = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
      if (other.GetComponent<DBI_EnemyBullet>() != null) {
        HitByEnemyBullet();
      }
    }
  }
}
