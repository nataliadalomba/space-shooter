using UnityEngine;

public class Collectable : MonoBehaviour {

    [SerializeField] private float speed = 3.0f;
    [SerializeField] private AudioClip clip;

    void Update() {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        if (transform.position.y <= -6f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            Player player = other.GetComponent<Player>();
            if (player != null) 
                OnPickUp(player);
            Destroy(gameObject);
        }
    }

    //TODO: make this abstract later
    protected virtual void OnPickUp(Player player) { }
}
