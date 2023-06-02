using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {
    #region
    /// <summary>
    /// The player's currrent speed.
    /// </summary>
    [SerializeField]
    private float speed = 5f;
    private float shiftSpeed = 7f;
    private float speedPowerUpMultiplier = 2f;
    #endregion
    private bool canTakeDamage = true;
    #region
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
    [SerializeField]
    private GameObject tripleShotLasersPrefab;
    private bool isSpeedPowerUpActive;
    #endregion
    private bool IsShieldPowerUpActive => currentShieldProtection > 0;
    private int currentShieldProtection;
    [SerializeField]
    private int totalShieldProtection = 3;
    [SerializeField]
    private GameObject shield;
    private SpriteRenderer shieldVisualizer;
    #region
    [SerializeField]
    private SpriteRenderer thrusterSprite;

    private int score;
    private UIManager uiManager;
    void Start() {
        transform.position = Vector3.zero;

        spawnManager = GameObject.FindGameObjectWithTag("Spawn Manager").GetComponent<SpawnManager>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        audio = GetComponent<AudioSource>();
    #endregion
        shieldVisualizer = shield.GetComponent<SpriteRenderer>();
        #region
        if (spawnManager == null)
            Debug.LogError("The Spawn Manager is null.");
        if (uiManager == null)
            Debug.LogError("The UI Manager is null.");
        if (audio == null)
            Debug.LogError("The AudioSource on the player is null.");
        #endregion
        if (shieldVisualizer == null)
            Debug.LogError("The SpriteRenderer on the Shield is null.");
    }
    #region
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
    #endregion
    public void Damage() {
        if (IsShieldPowerUpActive) {
            currentShieldProtection--;
            UpdateShieldColor();
            if (currentShieldProtection <= 0)
                ShieldPowerDown();
        } else {
            if (canTakeDamage) {
                lives--;
                Debug.Log("no shield on and minus a life");
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
        StartCoroutine(InvincibilityRoutine());
    }

    private void UpdateShieldColor() {
        Color c = shieldVisualizer.color;
        c.a = (float) currentShieldProtection / totalShieldProtection;
        shieldVisualizer.color = c;
    }
    #region
    private float getBaseSpeed() {
        speed = 5f;
        return speed;
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
        canTakeDamage = false;
        currentShieldProtection = totalShieldProtection;
        UpdateShieldColor();
        shield.SetActive(true);
    }

    private void ShieldPowerDown() {
        canTakeDamage = true;
        shield.SetActive(false);
    }

    public void AddToScore(int points) {
        score += points;
        uiManager.UpdateScore(score);
    }

    private IEnumerator InvincibilityRoutine() {
        Debug.Log("coroutine start");
        canTakeDamage = false;
        yield return new WaitForSeconds(2f);
        canTakeDamage = true;
        Debug.Log("coroutine end");
    }
}
