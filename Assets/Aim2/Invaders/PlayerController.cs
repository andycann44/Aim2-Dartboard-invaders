using UnityEngine;

namespace Aim2.Invaders {
  public class PlayerController : MonoBehaviour {
    public float moveSpeed = 8f;
    public float fireCooldown = 0.25f;
    public float bulletScale = 0.18f;

    float _cool;
    Camera _cam;

    void Start() { _cam = Camera.main; }

    void Update() {
      // Horizontal move (A/D or arrows)
      float h = Input.GetAxisRaw("Horizontal");
      transform.Translate(Vector3.right * h * moveSpeed * Time.deltaTime, Space.World);

      // Clamp to screen
      if (_cam) {
        float halfH = _cam.orthographicSize;
        float halfW = halfH * _cam.aspect;
        Vector3 p = transform.position;
        p.x = Mathf.Clamp(p.x, -halfW + 0.5f, halfW - 0.5f);
        transform.position = p;
      }

      // Shoot
      _cool -= Time.deltaTime;
      if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && _cool <= 0f) {
        _cool = fireCooldown;
        Fire();
      }
    }

    void Fire() {
      // Create a white 1x1 sprite on the fly
      var tex = new Texture2D(1,1, TextureFormat.RGBA32, false);
      tex.SetPixel(0,0, Color.white); tex.Apply();
      var spr = Sprite.Create(tex, new Rect(0,0,1,1), new Vector2(0.5f,0.5f), 1f);

      var go = new GameObject("Bullet");
      var sr = go.AddComponent<SpriteRenderer>(); sr.sprite = spr;
      go.transform.localScale = new Vector3(bulletScale, bulletScale*2.5f, 1f);
      go.transform.position = transform.position + Vector3.up * 0.7f;

      var col = go.AddComponent<BoxCollider2D>(); col.isTrigger = true;
      var rb = go.AddComponent<Rigidbody2D>(); rb.bodyType = RigidbodyType2D.Kinematic; rb.gravityScale = 0f;

      go.AddComponent<Bullet>();
    }
  }
}
