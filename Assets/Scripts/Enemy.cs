using System;
using UnityEngine;

using Random = UnityEngine.Random; //NOTE: This is an alias, because System.Random also exists!

public class Enemy : MonoBehaviour {
    public static event Action onAnyDefeated;

    [SerializeField] private float speed = 4f;
    [SerializeField] private GameObject laserPrefab;

    private HealthEntity playerHealth;
    private Animator anim;

    private new AudioSource audio;
    private float fireRate = 3.0f;
    private float canFire = -1;

    private Collider2D col2D;

    private bool isAlive = true;

    private void Start() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<HealthEntity>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        col2D = GetComponent<Collider2D>();

        if (playerHealth == null)
            Debug.LogError("The " + nameof(HealthEntity) + " component on the " + nameof(player) + " (" + player.name + ") is null.", player);
        if (anim == null)
            Debug.LogError("The Enemy Animator is null.");
        if (audio == null)
            Debug.LogError("The AudioSource on the enemy is null.");
        if (col2D == null)
            Debug.LogError("The Collider2D on the enemy is null.");
    }

    private void Update() {
        CalculateMovement();
        FireLaser();
    }

    private void CalculateMovement() {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        if (transform.position.y <= -7) {
            float randX = Random.Range((float) -10, 10);
            transform.position = new Vector3(randX, 9, 0);
        }
    }

    private void FireLaser() {
        if (Time.time > canFire && isAlive) {
            fireRate = Random.Range(3, 7);
            canFire = Time.time + fireRate;
            GameObject enemyLaser = Instantiate (laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
                lasers[i].AssignEnemyLaser();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
            TouchDamageWithPlayer();
        else
            CheckToDefeatFromPlayer(other);
    }

    private void TouchDamageWithPlayer() {
        playerHealth.TryDamage();
        anim.SetTrigger("OnEnemyDeath");
        speed = 0;
        audio.Play();

        col2D.enabled = false;
        isAlive = false;
        Destroy(this.gameObject, 3);
    }

    private bool CheckToDefeatFromPlayer(Collider2D other) {
        if ((other.TryGetComponent(out Laser laser) && !laser.IsEnemyLaser())
            || other.tag == "Wave") {
            if (laser != null)
                Destroy(laser.gameObject);

            anim.SetTrigger("OnEnemyDeath");
            speed = 0;
            audio.Play();

            col2D.enabled = false;
            isAlive = false;

            if (onAnyDefeated != null)
                onAnyDefeated();
            Destroy(this.gameObject, 3);
            return true;
        }
        return false;
    }
}