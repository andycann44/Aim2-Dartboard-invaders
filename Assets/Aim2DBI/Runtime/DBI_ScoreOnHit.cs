using UnityEngine;

namespace A2P.DBI {
  // Attach at runtime to enemies; awards score when hit by a player bullet-like object.
  public class DBI_ScoreOnHit : MonoBehaviour {
    public int points = 100;
    bool awarded;

    void OnCollisionEnter2D(Collision2D c) { TryAward(c.collider); }
    void OnTriggerEnter2D(Collider2D c) { TryAward(c); }

    void TryAward(Collider2D c) {
      if (awarded || DBI_GameManager.I == null) return;
      var n = c.gameObject.name.ToLowerInvariant();
      // Very tolerant: any collider that looks like a player bullet.
      if (n.Contains("bullet") || n.Contains("shot") || n.Contains("laser") || c.gameObject.CompareTag("PlayerBullet")) {
        awarded = true;
        DBI_GameManager.I.AddScore(points);
      }
    }
  }
}
