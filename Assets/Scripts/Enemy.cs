using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    private float speed = 4f;

    private Player player;
    private Animator anim;

    private new AudioSource audio;
    private Collider2D col2D;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        col2D = GetComponent<Collider2D>();

        if (player == null)
            Debug.LogError("The Player component on the Enemy is null.");
        if (anim == null)
            Debug.LogError("The Enemy Animator is null.");
        if (audio == null)
            Debug.LogError("The AudioSource on the enemy is null.");
        if (col2D == null)
            Debug.LogError("The Collider2D on the enemy is null.");
    }

    void Update() {
        CalculateMovement();
    }

    void CalculateMovement() {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        if (transform.position.y <= -7f) {
            float randX = Random.Range(-10f, 10f);
            transform.position = new Vector3(randX, 9, 0);
        }
    }

    private void OnTriggerEnter2D (Collider2D other) {
        if (other.tag == "Player") {
            player.Damage();
            anim.SetTrigger("OnEnemyDeath");
            speed = 0;
            audio.Play();

            col2D.enabled = false;
            Destroy(this.gameObject, 3f);
        }

        if (other.tag == "Laser") {
            Destroy(other.gameObject);
            if (player != null)
                player.AddToScore(10);
            anim.SetTrigger("OnEnemyDeath");
            speed = 0;
            audio.Play();

            col2D.enabled = false;
            Destroy(this.gameObject, 3f);
        }
    }
}
