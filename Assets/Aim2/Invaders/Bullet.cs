using UnityEngine;

namespace Aim2.Invaders {
  public class Bullet : MonoBehaviour {
    public float speed = 12f;
    public float lifetime = 5f;

    void Start() { Destroy(gameObject, lifetime); }

    void Update() {
      transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D other) {
      if (other.GetComponent<EnemyMarker>()) {
        Destroy(other.gameObject);
        Destroy(gameObject);
      }
    }
  }
}
