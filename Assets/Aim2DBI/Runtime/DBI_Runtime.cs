using UnityEngine;

namespace A2P.DBI {
  public static class DBI_Runtime {
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init() {
      // Ensure persistent services
      var go = GameObject.Find("DBI_GameServices");
      if (go == null) {
        go = new GameObject("DBI_GameServices");
        Object.DontDestroyOnLoad(go);
        go.AddComponent<DBI_GameManager>();
        go.AddComponent<DBI_HUDOverlay>();
        go.AddComponent<DBI_EnemyVolleySpawner>();
      }
      // Ensure Player has lives component
      var player = SafeFindPlayer();
      if (player != null && player.GetComponent<DBI_PlayerLives>() == null) {
        player.AddComponent<DBI_PlayerLives>();
      }
      // Attach score-awarder to enemies
      var all = Object.FindObjectsOfType<Transform>();
      foreach (var t in all) {
        if (!t.gameObject.activeInHierarchy) continue;
        var n = t.gameObject.name.ToLowerInvariant();
        if (n.Contains("enemy") && t.GetComponent<DBI_ScoreOnHit>() == null) {
          // Only if it can be hit
          if (t.GetComponent<Collider2D>() != null) {
            t.gameObject.AddComponent<DBI_ScoreOnHit>();
          }
        }
      }
    }

    static GameObject SafeFindPlayer() {
      // Try by tag (guarded), then by name contains "player"
      try {
        foreach (var t in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()) {
          // avoid throwing on undefined tags by not using FindWithTag
          if (t.name.ToLowerInvariant().Contains("player")) return t;
          var kids = t.GetComponentsInChildren<Transform>(true);
          foreach (var k in kids) {
            if (k.gameObject.name.ToLowerInvariant().Contains("player")) return k.gameObject;
          }
        }
      } catch { }
      return null;
    }
  }
}
