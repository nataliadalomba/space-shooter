using UnityEngine;

public class Collectable : MonoBehaviour {

    [SerializeField] private float speed = 3;
    [SerializeField] private AudioClip clip;

    private void Update() {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        if (transform.position.y <= -6f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            GameObject player = other.gameObject;

            AudioSource.PlayClipAtPoint(clip, transform.position);
            OnPickUp(player);
            Destroy(gameObject);
        }
    }

    //TODO: make this abstract later
    protected virtual void OnPickUp(GameObject player) { }
}
