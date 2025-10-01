using UnityEngine;

namespace Aim2.Invaders {
  public class EnemyFormation : MonoBehaviour {
    public int rows = 4;
    public int cols = 8;
    public float spacing = 1.0f;
    public float moveSpeed = 2.0f;
    public float stepDown = 0.4f;

    Camera _cam;
    float _dir = 1f; // 1 right, -1 left

    void Start() {
      _cam = Camera.main;
      BuildFormation();
    }

    void Update() {
      if (transform.childCount == 0) return;

      transform.Translate(Vector3.right * _dir * moveSpeed * Time.deltaTime, Space.World);

      // Check bounds using extreme child positions
      float halfH = _cam.orthographicSize;
      float halfW = halfH * _cam.aspect;

      foreach (Transform child in transform) {
        if (!child) continue;
        if (child.position.x > halfW - 0.5f || child.position.x < -halfW + 0.5f) {
          // Hit edge: flip direction and step down
          _dir *= -1f;
          transform.Translate(Vector3.down * stepDown, Space.World);
          break;
        }
      }
    }

    void BuildFormation() {
      // Simple white square sprite
      var tex = new Texture2D(1,1, TextureFormat.RGBA32, false);
      tex.SetPixel(0,0, Color.white); tex.Apply();
      var spr = Sprite.Create(tex, new Rect(0,0,1,1), new Vector2(0.5f,0.5f), 1f);

      for (int r = 0; r < rows; r++) {
        for (int c = 0; c < cols; c++) {
          var e = new GameObject($"Enemy_{r}_{c}");
          var sr = e.AddComponent<SpriteRenderer>(); sr.sprite = spr; sr.color = Color.HSVToRGB(0.12f + 0.04f*r, 0.8f, 0.9f);
          e.transform.SetParent(transform, worldPositionStays:false);
          e.transform.localScale = new Vector3(0.4f, 0.3f, 1f);
          e.AddComponent<BoxCollider2D>().isTrigger = true;
          e.AddComponent<EnemyMarker>();
        }
      }

      // Center grid
      float width = (cols - 1) * spacing;
      float height = (rows - 1) * spacing;
      int i = 0;
      foreach (Transform child in transform) {
        int r = i / cols;
        int c = i % cols;
        float x = -width/2f + c * spacing;
        float y = height/2f - r * spacing + 2.5f; // start a bit above center
        child.localPosition = new Vector3(x, y, 0f);
        i++;
      }
    }
  }
}
