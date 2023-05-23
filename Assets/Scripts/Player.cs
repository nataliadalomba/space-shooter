using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private GameObject laserPrefab;
    [SerializeField]
    private GameObject tripleShotLasersPrefab;
    [SerializeField]
    private GameObject shieldVisualizer;
    [SerializeField]
    private SpriteRenderer thrusterSprite;

    [SerializeField]
    private float fireRate = 0.15f;
    private float canFire = -1f;
    [SerializeField]
    private int lives = 3;
    private SpawnManager spawnManager;

    private float powerUpDuration = 5.0f;
    private bool isTripleShotPowerUpActive;
    private bool isSpeedPowerUpActive;
    private bool isShieldPowerUpActive;

    private float speedMultiplier = 2f;

    private int score;
    private UIManager uiManager;

    private SpriteRenderer[] wingDamageSprites = new SpriteRenderer[2];
    private new AudioSource audio;
    [SerializeField]
    private AudioClip laserClip;

    void Start() {
        transform.position = Vector3.zero;
        wingDamageSprites[1] = transform.GetChild(3).GetComponent<SpriteRenderer>(); //leftWing

        spawnManager = GameObject.FindGameObjectWithTag("Spawn Manager").GetComponent<SpawnManager>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        audio = GetComponent<AudioSource>();

        if (spawnManager == null)
            Debug.LogError("The Spawn Manager is null.");
        if (uiManager == null)
            Debug.LogError("The UI Manager is null.");
        if (audio == null)
            Debug.LogError("The AudioSource on the player is null.");
    }

    void Update() {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > canFire)
            FireLaser();
    }

    void CalculateMovement() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.6f, 0), 0);

        if (transform.position.x >= 11.3f)
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        else if (transform.position.x <= -11.3f)
            transform.position = new Vector3(11.3f, transform.position.y, 0);
    }

    void FireLaser() {
        canFire = Time.time + fireRate;

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (isTripleShotPowerUpActive)
                Instantiate(tripleShotLasersPrefab, transform.position, Quaternion.identity);
            else Instantiate(laserPrefab, transform.position + new Vector3(0, 1.075f, 0), Quaternion.identity);
            audio.clip = laserClip;
            audio.Play();
        }
    }

    public void Damage() {
        if (isShieldPowerUpActive) {
            isShieldPowerUpActive = false;
            shieldVisualizer.SetActive(false);
            return;
        }
        
        lives--;
        uiManager.UpdateLives(lives);
        if (lives == 2 || lives == 1)
            wingDamageSprites[Random.Range(0, wingDamageSprites.Length)].enabled = true;
        else if (lives <= 0) {
            spawnManager.OnPlayerDeath();
            uiManager.GameOverSequence();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotPowerUpActive() {
        isTripleShotPowerUpActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    
    IEnumerator TripleShotPowerDownRoutine() {
        while(isTripleShotPowerUpActive) {
            yield return new WaitForSeconds(powerUpDuration);
            isTripleShotPowerUpActive = false;
        }
    }

    public void SpeedPowerUpActive() {
        isSpeedPowerUpActive = true;
        speed *= speedMultiplier;
        thrusterSprite.color = Color.cyan;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator SpeedPowerDownRoutine() {
        while(isSpeedPowerUpActive) {
            yield return new WaitForSeconds(powerUpDuration);
            speed /= speedMultiplier;
            thrusterSprite.color = Color.white;
            isSpeedPowerUpActive = false;
        }
    }

    public void ShieldPowerUpActive() {
        isShieldPowerUpActive = true;
        shieldVisualizer.SetActive(true);
    }

    public void AddToScore(int points) {
        score += points;
        uiManager.UpdateScore(score);
    }
}
