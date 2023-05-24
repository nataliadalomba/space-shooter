using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    private float speed = 4f;

    private Player player;
    private Animator anim;

    private new AudioSource audio;
    [SerializeField]
    private GameObject laserPrefab;
    private float fireRate = 3.0f;
    private float canFire = -1;

    private Collider2D col2D;

    private bool isAlive = true;

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
        FireLaser();
    }

    void CalculateMovement() {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        if (transform.position.y <= -7f) {
            float randX = Random.Range(-10f, 10f);
            transform.position = new Vector3(randX, 9, 0);
        }
    }

    void FireLaser() {
        if (Time.time > canFire && isAlive) {
            fireRate = Random.Range(3f, 7f);
            canFire = Time.time + fireRate;
            GameObject enemyLaser = Instantiate (laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
                lasers[i].AssignEnemyLaser();
        }
    }

    private void OnTriggerEnter2D (Collider2D other) {
        if (other.tag == "Player") {
            player.Damage();
            anim.SetTrigger("OnEnemyDeath");
            speed = 0;
            audio.Play();

            col2D.enabled = false;
            isAlive = false;
            Destroy(this.gameObject, 3f);
        }

        if (other.tag == "Laser") {
            Laser laser = other.GetComponent<Laser>();
            if (laser.IsEnemyLaser() == false) {
                Destroy(other.gameObject);
                if (player != null)
                    player.AddToScore(100);
                anim.SetTrigger("OnEnemyDeath");
                speed = 0;
                audio.Play();

                col2D.enabled = false;
                isAlive = false;
                Destroy(this.gameObject, 3f);
            }

        }
    }
}
