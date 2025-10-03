using UnityEngine;

namespace Aim2.Invaders {
  public class GameController : MonoBehaviour {
    public static GameController Instance { get; private set; }

    public EnemyFormation formation;
    public Transform player;
    public float nextLevelDelay = 1.2f;
    public int currentLevel { get; private set; } = 1;

    bool _progressing = false;

    void Awake() { Instance = this; }

    void Start() {
      if (!formation) formation = FindObjectOfType<EnemyFormation>();
      if (!player) {
        var p = GameObject.Find("Player");
        if (p) player = p.transform;
      }
      StartLevel(1);
    }

    void Update() {
      if (_progressing) return;
      int alive = FindObjectsOfType<EnemyMarker>().Length;
      if (alive == 0) {
        _progressing = true;
        ClearBullets();
        Invoke(nameof(NextLevel), nextLevelDelay);
      }
    }

    void NextLevel() => StartLevel(currentLevel + 1);

    public void StartLevel(int level) {
      currentLevel = Mathf.Max(1, level);
      if (formation) formation.BuildFormationForLevel(currentLevel);
      ResetPlayer();
      _progressing = false;
    }

    void ResetPlayer() {
      if (player) player.position = new Vector3(0f, -3.5f, 0f);
    }

    void ClearBullets() {
      foreach (var b in FindObjectsOfType<Bullet>()) {
        if (b) Destroy(b.gameObject);
      }
    }
  }
}
