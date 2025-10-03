using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Aim2.Invaders {
  public class Aim2InvadersBootstrap : MonoBehaviour {
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init() {
      // Camera
      var cam = Camera.main;
      if (!cam) {
        var goCam = new GameObject("Main Camera");
        cam = goCam.AddComponent<Camera>();
        goCam.tag = "MainCamera";
      }
      cam.orthographic = true;
      cam.orthographicSize = 5f;
      cam.backgroundColor = new Color(0.06f,0.06f,0.10f);

      // Player
      var pTex = new Texture2D(1,1, TextureFormat.RGBA32, false);
      pTex.SetPixel(0,0, Color.cyan); pTex.Apply();
      var pSpr = Sprite.Create(pTex, new Rect(0,0,1,1), new Vector2(0.5f,0.5f), 1f);

      var player = new GameObject("Player");
      var psr = player.AddComponent<SpriteRenderer>(); psr.sprite = pSpr;
      player.transform.position = new Vector3(0f, -3.5f, 0f);
      player.transform.localScale = new Vector3(0.7f, 0.35f, 1f);
      var pcb = player.AddComponent<BoxCollider2D>(); pcb.isTrigger = true;
      var prb = player.AddComponent<Rigidbody2D>(); prb.bodyType = RigidbodyType2D.Kinematic; prb.gravityScale = 0f;
      player.AddComponent<PlayerController>();

      // Enemy formation
      var formation = new GameObject("EnemyFormation").AddComponent<EnemyFormation>();

      // Game controller
      var gc = new GameObject("GameController").AddComponent<GameController>();
      gc.formation = formation;
      gc.player = player.transform;

      // HUD
      new GameObject("HUD").AddComponent<HUDMini>();
    }
  }

  // Minimal HUD showing level + restart hint on win
  public class HUDMini : MonoBehaviour {
    int _startCount;

    void Update() {
      int alive = GameObject.FindObjectsOfType<EnemyMarker>().Length;
      if (alive == 0 && Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
      }

      // When a new wave spawns, refresh baseline
      int total = GameObject.FindObjectsOfType<EnemyMarker>().Length;
      if (total > _startCount) _startCount = total;
    }

    void OnGUI() {
      int alive = GameObject.FindObjectsOfType<EnemyMarker>().Length;
      int destroyed = Mathf.Max(0, _startCount - alive);
      int level = GameController.Instance ? GameController.Instance.currentLevel : 1;

      GUI.Label(new Rect(10,10,420,24), $"Level {level} — Destroyed: {destroyed}/{_startCount}");
      if (alive == 0) {
        GUI.Label(new Rect(10,34,380,24), "Wave cleared! Next level incoming…  (R to restart)");
      }
    }
  }
}
