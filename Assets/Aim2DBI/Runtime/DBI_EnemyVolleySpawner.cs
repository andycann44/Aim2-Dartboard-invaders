using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace A2P.DBI {
  public class DBI_EnemyVolleySpawner : MonoBehaviour {
    public float minInterval = 0.8f;
    public float maxInterval = 2.0f;
    public float bulletSpeed = 8f;

    Coroutine loop;

    void OnEnable() {
      loop = StartCoroutine(FireLoop());
    }

    void OnDisable() {
      if (loop != null) StopCoroutine(loop);
    }

    IEnumerator FireLoop() {
      var w = new WaitForEndOfFrame();
      while (true) {
        yield return w;
        if (DBI_GameManager.I == null || DBI_GameManager.I.isGameOver) { yield return new WaitForSeconds(0.25f); continue; }
        var src = PickEnemy();
        if (src != null) {
          var pos = src.position + Vector3.down * 0.3f;
          DBI_EnemyBullet.Spawn(pos, bulletSpeed);
        }
        var wait = Random.Range(minInterval, maxInterval);
        yield return new WaitForSeconds(wait);
      }
    }

    Transform PickEnemy() {
      var all = new List<Transform>();
      var roots = FindObjectsOfType<Transform>();
      foreach (var t in roots) {
        if (!t.gameObject.activeInHierarchy) continue;
        var name = t.gameObject.name.ToLowerInvariant();
        if (name.Contains("enemy")) {
          // Must have some visible/solid presence to shoot from
          if (t.GetComponent<Collider2D>() != null || t.GetComponent<SpriteRenderer>() != null) {
            all.Add(t);
          }
        }
      }
      if (all.Count == 0) return null;
      return all[Random.Range(0, all.Count)];
    }
  }
}
