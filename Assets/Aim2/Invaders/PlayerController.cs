using UnityEngine;
using UnityEngine.InputSystem; // NEW

namespace Aim2.Invaders {
  public class PlayerController : MonoBehaviour {
    public float moveSpeed = 8f;
    public float fireCooldown = 0.25f;
    public float bulletScale = 0.18f;

    float _cool;
    Camera _cam;

    void Start() { _cam = Camera.main; }

    void Update() {
      // Horizontal from Gamepad/Keyboard (new Input System)
      float h = 0f;
      if (Gamepad.current != null) {
        h = Gamepad.current.leftStick.ReadValue().x;
      }
      if (Keyboard.current != null) {
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) h -= 1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) h += 1f;
      }
      h = Mathf.Clamp(h, -1f, 1f);
      transform.Translate(Vector3.right * h * moveSpeed * Time.deltaTime, Space.World);

      // Clamp to screen
      if (_cam) {
        float halfH = _cam.orthographicSize;
        float halfW = halfH * _cam.aspect;
        var p = transform.position;
        p.x = Mathf.Clamp(p.x, -halfW + 0.5f, halfW - 0.5f);
        transform.position = p;
      }

      // Shoot (space/mouse left/gamepad south)
      _cool -= Time.deltaTime;
      bool fire = false;
      if (Keyboard.current != null && Keyboard.current.spaceKey.isPressed) fire = true;
      if (Mouse.current != null && Mouse.current.leftButton.isPressed) fire = true;
      if (Gamepad.current != null && Gamepad.current.buttonSouth.isPressed) fire = true;

      if (fire && _cool <= 0f) {
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
