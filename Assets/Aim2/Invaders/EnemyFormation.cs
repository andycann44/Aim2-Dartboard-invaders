using UnityEngine;

namespace Aim2.Invaders {
  public class EnemyFormation : MonoBehaviour {
    [Header("Base (Level 1)")]
    public int baseRows = 3;
    public int baseCols = 6;
    public float baseSpeed = 1.2f;        // ↓ slower first level (was 2.0f)
    public float spacing = 1.0f;
    public float stepDown = 0.4f;

    [Header("Growth per level")]
    public float speedPerLevel = 0.4f;    // each level moves faster
    public int colsPerLevel = 1;          // +1 column per level
    public int rowsEveryNLevels = 2;      // +1 row every N levels
    public int maxCols = 12;
    public int maxRows = 8;

    Camera _cam;
    float _dir = 1f; // 1 right, -1 left
    int _rows, _cols;
    public float moveSpeed { get; private set; }

    void Awake() { _cam = Camera.main; }

    void Update() {
      if (transform.childCount == 0) return;

      transform.Translate(Vector3.right * _dir * moveSpeed * Time.deltaTime, Space.World);

      // Screen bounds
      float halfH = _cam ? _cam.orthographicSize : 5f;
      float halfW = halfH * (_cam ? _cam.aspect : 16f/9f);

      foreach (Transform child in transform) {
        if (!child) continue;
        if (child.position.x > halfW - 0.5f || child.position.x < -halfW + 0.5f) {
          _dir *= -1f;
          transform.Translate(Vector3.down * stepDown, Space.World);
          break;
        }
      }
    }

    public void BuildFormationForLevel(int level) {
      // Compute rows/cols/speed for this level
      int addCols = Mathf.Max(0, (level-1) * colsPerLevel);
      int addRows = rowsEveryNLevels > 0 ? Mathf.Max(0, (level-1) / rowsEveryNLevels) : 0;

      _cols = Mathf.Clamp(baseCols + addCols, 1, maxCols);
      _rows = Mathf.Clamp(baseRows + addRows, 1, maxRows);
      moveSpeed = Mathf.Max(0.3f, baseSpeed + speedPerLevel * (level-1));

      // Reset direction & position a bit above center
      _dir = 1f;
      transform.position = Vector3.zero;

      // Clear previous enemies
      for (int i = transform.childCount - 1; i >= 0; i--) {
        var ch = transform.GetChild(i);
        if (Application.isPlaying) GameObject.Destroy(ch.gameObject);
        else GameObject.DestroyImmediate(ch.gameObject);
      }

      BuildGrid();
    }

    void BuildGrid() {
      // Simple white 1x1 sprite
      var tex = new Texture2D(1,1, TextureFormat.RGBA32, false);
      tex.SetPixel(0,0, Color.white); tex.Apply();
      var spr = Sprite.Create(tex, new Rect(0,0,1,1), new Vector2(0.5f,0.5f), 1f);

      for (int r = 0; r < _rows; r++) {
        for (int c = 0; c < _cols; c++) {
          var e = new GameObject($"Enemy_{r}_{c}");
          var sr = e.AddComponent<SpriteRenderer>();
          sr.sprite = spr;
          sr.color = Color.HSVToRGB(0.12f + 0.04f*r, 0.8f, 0.9f);
          e.transform.SetParent(transform, false);
          e.transform.localScale = new Vector3(0.4f, 0.3f, 1f);
          e.AddComponent<BoxCollider2D>().isTrigger = true;
          e.AddComponent<EnemyMarker>();
        }
      }

      // Center grid
      float width = (_cols - 1) * spacing;
      float height = (_rows - 1) * spacing;
      int i = 0;
      foreach (Transform child in transform) {
        int r = i / _cols;
        int c = i % _cols;
        float x = -width/2f + c * spacing;
        float y = height/2f - r * spacing + 2.5f;
        child.localPosition = new Vector3(x, y, 0f);
        i++;
      }
    }
  }
}
