using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {
    [Tooltip("The player's currrent speed.")]
    [SerializeField] private float speed = 5;

    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private SpriteRenderer[] wingDamageSprites = new SpriteRenderer[2];
    [SerializeField] private AudioClip laserClip;
    [SerializeField] private AudioClip outOfAmmoClip;
    [SerializeField] private int laserCount = 15;
    [SerializeField] private float fireRate = 0.15f;
    [SerializeField] private int lives = 3;
    [SerializeField] private GameObject tripleShotLasersPrefab;
    [SerializeField] private SpriteRenderer thrusterSprite;
    [SerializeField] private int totalShieldProtection = 3;
    [SerializeField] private GameObject shield;

    private float shiftSpeed = 7;
    private float speedPowerUpMultiplier = 2;

    private new AudioSource audio;
    private float canFire = -1;
    private SpawnManager spawnManager;

    private float powerUpDuration = 5;
    private bool isTripleShotPowerUpActive;
    private bool isSpeedPowerUpActive;

    private int score;
    private UIManager uiManager;

    private int currentShieldProtection;
    private SpriteRenderer shieldVisualizer;
    private float timeInvincibleUntil = 0;

    public bool IsInvincible => Time.time <= timeInvincibleUntil;
    public bool IsShieldPowerUpActive => currentShieldProtection > 0;

    private void Start() {
        transform.position = Vector3.zero;

        spawnManager = GameObject.FindGameObjectWithTag("Spawn Manager").GetComponent<SpawnManager>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        audio = GetComponent<AudioSource>();
        shieldVisualizer = shield.GetComponent<SpriteRenderer>();
        if (spawnManager == null)
            Debug.LogError("The Spawn Manager is null.");
        if (uiManager == null)
            Debug.LogError("The UI Manager is null.");
        if (audio == null)
            Debug.LogError("The AudioSource on the player is null.");
        if (shieldVisualizer == null)
            Debug.LogError("The SpriteRenderer on the Shield is null.");
    }

    private void Update() {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > canFire)
            FireLaser();
    }

    private void CalculateMovement() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && isSpeedPowerUpActive == false)
            speed = shiftSpeed;
        else if (isSpeedPowerUpActive == false)
            speed = GetBaseSpeed();
        transform.Translate(direction * speed * Time.deltaTime);

        //Part 1: Get it!
        Vector3 previousPosition = transform.position;

        //Part 2: Change it!
        previousPosition = new Vector3(previousPosition.x, Mathf.Clamp(previousPosition.y, -3.6f, 0), 0);
        if (previousPosition.x >= 11.3f)
            previousPosition = new Vector3(-11.3f, previousPosition.y, 0);
        else if (previousPosition.x <= -11.3f)
            previousPosition = new Vector3(11.3f, previousPosition.y, 0);

        //Part 3: Set it!
        transform.position = previousPosition;
    }

    private void FireLaser() {
        canFire = Time.time + fireRate;

        if (Input.GetKeyDown(KeyCode.Space) && laserCount >= 1) {
            if (isTripleShotPowerUpActive) {
                Instantiate(tripleShotLasersPrefab, transform.position, Quaternion.identity);
                SubtractLaserAmmo(3);
            } else {
                Instantiate(laserPrefab, transform.position + new Vector3(0, 1.075f, 0), Quaternion.identity);
                SubtractLaserAmmo(1);
            }
            audio.clip = laserClip;
            audio.Play();
        }

        if (Input.GetKeyDown(KeyCode.Space) && laserCount <= 0) {
            audio.clip = outOfAmmoClip;
            audio.Play();
        }
    }

    public int GetLaserCount() {
        return laserCount;
    }

    public void SubtractLaserAmmo(int subtractLasers) {
        laserCount -= subtractLasers;
        uiManager.UpdateLaserCount(laserCount);
    }

    public bool TryDamage() {
        if (IsInvincible)
            return false;

        if (IsShieldPowerUpActive)
            TakeShieldDamage();
        else Damage();
        return true;
    }
    
    private void Damage() {
        lives--;
        OnDamaged();
    }

    private void TakeShieldDamage() {
        currentShieldProtection--;
        UpdateShieldColor();
        if (currentShieldProtection <= 0)
            ShieldPowerDown();
        StartInvincibility();
    }

    //NOTE: This updates both us, AND other scripts, after we take damage.
    //This is IN RESPONSE to damage.
    private void OnDamaged() {
        uiManager.UpdateLives(lives);
        if (lives == 2)
            wingDamageSprites[Random.Range(0, wingDamageSprites.Length)].enabled = true;
        else if (lives == 1) {
            if (wingDamageSprites[0].enabled)
                wingDamageSprites[1].enabled = true;
            else wingDamageSprites[0].enabled = true;
        } else if (lives <= 0) {
            spawnManager.OnPlayerDeath();
            uiManager.GameOverSequence();
            Destroy(this.gameObject);
            return;
        }
        StartInvincibility();
    }

    private void UpdateShieldColor() {
        Color c = shieldVisualizer.color;
        c.a = (float) currentShieldProtection / totalShieldProtection;
        shieldVisualizer.color = c;
    }

    private float GetBaseSpeed() {
        return 5;
    }

    public void TripleShotPowerUpActive() {
        isTripleShotPowerUpActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    private IEnumerator TripleShotPowerDownRoutine() {
        while (isTripleShotPowerUpActive) {
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

    private IEnumerator SpeedPowerDownRoutine() {
        while (isSpeedPowerUpActive) {
            yield return new WaitForSeconds(powerUpDuration);
            speed /= speedPowerUpMultiplier;
            thrusterSprite.color = Color.white;
            isSpeedPowerUpActive = false;
        }
    }
    public void ShieldPowerUpActive() {
        currentShieldProtection = totalShieldProtection;
        UpdateShieldColor();
        shield.SetActive(true);
    }

    private void ShieldPowerDown() {
        shield.SetActive(false);
    }

    private void StartInvincibility() {
        timeInvincibleUntil = Time.time + 2;
    }
    public void AddToScore(int points) {
        score += points;
        uiManager.UpdateScore(score);
    }
}
