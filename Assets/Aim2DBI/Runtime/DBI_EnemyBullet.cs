using UnityEngine;

namespace A2P.DBI {
  public class DBI_EnemyBullet : MonoBehaviour {
    public float speed = 8f;
    public float ttl = 8f;

    float life;
    void Update() {
      transform.position += Vector3.down * speed * Time.deltaTime;
      life += Time.deltaTime;
      if (life >= ttl || transform.position.y < -200f) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other) {
      if (!Application.isPlaying) return;
      var pl = other.GetComponent<DBI_PlayerLives>();
      if (pl != null) {
        pl.HitByEnemyBullet();
        Destroy(gameObject);
      }
    }

    public static GameObject Spawn(Vector3 pos, float speed) {
      var go = new GameObject("EnemyBullet(DBI)");
      go.transform.position = pos;
      var sr = go.AddComponent<SpriteRenderer>();
      sr.sortingOrder = 1000;
      sr.sprite = MakeSprite();
      var col = go.AddComponent<CircleCollider2D>();
      col.isTrigger = true;
      var rb = go.AddComponent<Rigidbody2D>();
      rb.gravityScale = 0f;
      var eb = go.AddComponent<DBI_EnemyBullet>();
      eb.speed = speed;
      return go;
    }

    static Sprite MakeSprite() {
      // 4x4 white pixel sprite
      var tex = new Texture2D(4,4, TextureFormat.RGBA32, false);
      var px = new Color[16];
      for (int i=0;i<px.Length;i++) px[i] = Color.white;
      tex.SetPixels(px); tex.Apply();
      var sp = Sprite.Create(tex, new Rect(0,0,4,4), new Vector2(0.5f,0.5f), 64);
      return sp;
    }
  }
}
