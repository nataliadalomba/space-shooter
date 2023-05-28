using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {
    /// <summary>
    /// The player's currrent speed.
    /// </summary>
    [SerializeField]
    private float speed = 5f;
    private float shiftSpeed = 7f;
    #region
    private float speedPowerUpMultiplier = 2f;

    private bool canTakeDamage = true;
    [SerializeField]
    private GameObject laserPrefab;
    [SerializeField]
    private SpriteRenderer[] wingDamageSprites = new SpriteRenderer[2];
    private new AudioSource audio;
    [SerializeField]
    private AudioClip laserClip;

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
    [SerializeField]
    private int shieldProtectionLeft = 3;
    [SerializeField]
    private GameObject tripleShotLasersPrefab;
    [SerializeField]
    private GameObject shieldVisualizer;
    private Color shieldAlpha;
    [SerializeField]
    private SpriteRenderer thrusterSprite;

    private int score;
    private UIManager uiManager;
    #endregion

    void Start() {
        transform.position = Vector3.zero;

        spawnManager = GameObject.FindGameObjectWithTag("Spawn Manager").GetComponent<SpawnManager>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        audio = GetComponent<AudioSource>();
        shieldAlpha = shieldVisualizer.GetComponent<SpriteRenderer>().color;

        if (spawnManager == null)
            Debug.LogError("The Spawn Manager is null.");
        if (uiManager == null)
            Debug.LogError("The UI Manager is null.");
        if (audio == null)
            Debug.LogError("The AudioSource on the player is null.");
        if (shieldAlpha == null)
            Debug.LogError("The SpriteRenderer on the Shield is null.");
        if (shieldAlpha != null)
            Debug.Log("The alpha on the shield's SpriteRenderer has been grabbed");
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
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && isSpeedPowerUpActive == false)
            speed = shiftSpeed;
        else if (isSpeedPowerUpActive == false)
            speed = getBaseSpeed();
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
            switch (shieldProtectionLeft) {
                case 3:
                    shieldAlpha.a = 1f;
                    shieldProtectionLeft--;
                    StartCoroutine(InvincibilityRoutine());
                    break;
                case 2:
                    shieldAlpha.a = .66f;
                    shieldProtectionLeft--;
                    StartCoroutine(InvincibilityRoutine());
                    break;
                case 1:
                    shieldAlpha.a = .33f;
                    shieldProtectionLeft--;
                    StartCoroutine(InvincibilityRoutine());
                    break;
                case 0:
                    shieldAlpha.a = 0f;
                    isShieldPowerUpActive = false;
                    shieldVisualizer.SetActive(false);
                    StartCoroutine(InvincibilityRoutine());
                    break;
            }
        }

        if (canTakeDamage) {
            StartCoroutine(InvincibilityRoutine());
            lives--;
            uiManager.UpdateLives(lives);
            if (lives == 2)
                wingDamageSprites[Random.Range(0, wingDamageSprites.Length)].enabled = true;
            else if (lives == 1) {
                if (wingDamageSprites[0].enabled == true)
                    wingDamageSprites[1].enabled = true;
                else wingDamageSprites[0].enabled = true;
            }
            else if (lives <= 0) {
                spawnManager.OnPlayerDeath();
                uiManager.GameOverSequence();
                Destroy(this.gameObject);
            }
        }
    }

    private float getBaseSpeed() {
        speed = 5f;
        return speed;
    }

    #region
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
        speed *= speedPowerUpMultiplier;
        thrusterSprite.color = Color.cyan;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator SpeedPowerDownRoutine() {
        while(isSpeedPowerUpActive) {
            yield return new WaitForSeconds(powerUpDuration);
            speed /= speedPowerUpMultiplier;
            thrusterSprite.color = Color.white;
            isSpeedPowerUpActive = false;
        }
    }
    #endregion
    public void ShieldPowerUpActive() {
        isShieldPowerUpActive = true;
        shieldVisualizer.SetActive(true);
    }

    public void AddToScore(int points) {
        score += points;
        uiManager.UpdateScore(score);
    }

    private IEnumerator InvincibilityRoutine() {
        canTakeDamage = false;
        yield return new WaitForSeconds(2f);
        canTakeDamage = true;
    }
}
