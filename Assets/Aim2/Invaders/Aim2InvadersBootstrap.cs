using UnityEngine;

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
      var formation = new GameObject("EnemyFormation");
      formation.AddComponent<EnemyFormation>();

      // Simple HUD
      var hud = new GameObject("HUD").AddComponent<HUDMini>();
    }
  }

  // Minimal OnGUI HUD (score = enemies destroyed)
  public class HUDMini : MonoBehaviour {
    int _startCount;
    int _lastCount;
    float _y;

    void Start() {
      _startCount = GameObject.FindObjectsOfType<EnemyMarker>().Length;
      _lastCount  = _startCount;
    }

    void OnGUI() {
      int alive = GameObject.FindObjectsOfType<EnemyMarker>().Length;
      int destroyed = _startCount - alive;
      string msg = $"Invaders destroyed: {destroyed} / {_startCount}";
      GUI.Label(new Rect(10,10,380,30), msg);

      if (alive == 0) {
        GUI.Label(new Rect(10,40,320,30), "YOU WIN — Press R to restart");
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.R) {
          UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
      }
    }
  }
}
