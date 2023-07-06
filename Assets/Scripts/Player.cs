using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {
    [Tooltip("The player's currrent speed.")]
    [SerializeField] private float speed = 5;

    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private SpriteRenderer[] wingDamageSprites = new SpriteRenderer[2];
    [SerializeField] private AudioClip laserClip;
    [SerializeField] private AudioClip outOfAmmoClip;
    [SerializeField] private int ammoCount = 15;
    [SerializeField] private float fireRate = 0.15f;
    [SerializeField] private int health = 3;
    [SerializeField] private GameObject tripleShotLasersPrefab;
    [SerializeField] private SpriteRenderer thrusterSprite;
    [SerializeField] private int totalShieldProtection = 3;
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject wave;

    private float shiftSpeed = 7;
    private float speedPowerUpMultiplier = 2;

    private new AudioSource audio;
    private float canFire = -1;
    private SpawnManager spawnManager;

    private bool isTripleShotPowerUpActive;
    private bool isSpeedPowerUpActive;
    private bool isWavePowerUpActive;
    private bool isShiftSpeedActive;
    private bool isShiftSpeedCoolingDown;

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
        ShiftSpeedIncrease();
        transform.Translate(direction * speed * Time.deltaTime);

        Vector3 previousPosition = transform.position;

        previousPosition = new Vector3(previousPosition.x, Mathf.Clamp(previousPosition.y, -3.6f, 0), 0);
        if (previousPosition.x >= 11.3f)
            previousPosition = new Vector3(-11.3f, previousPosition.y, 0);
        else if (previousPosition.x <= -11.3f)
            previousPosition = new Vector3(11.3f, previousPosition.y, 0);
        transform.position = previousPosition;
    }

    private void ShiftSpeedIncrease() {
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && isSpeedPowerUpActive == false && isShiftSpeedActive == false && isShiftSpeedCoolingDown == false) {
            isShiftSpeedActive = true;
            speed = shiftSpeed;
            StartCoroutine(ShiftSpeedActiveCoroutine());
            StartCoroutine(ShiftSpeedCooldownCoroutine());
        }
        else if (isSpeedPowerUpActive == false)
            speed = GetBaseSpeed();
    }

    //can hold shift for 3 seconds
    //cooldown is for 20 seconds
    private IEnumerator ShiftSpeedActiveCoroutine() {
        WaitForSeconds wait = new WaitForSeconds(3);
        while (isShiftSpeedActive) {
            yield return wait;
            uiManager.IncreaseThrusterSlider();
            isShiftSpeedActive = false;
            isShiftSpeedCoolingDown = true;
        }
    }

    private IEnumerator ShiftSpeedCooldownCoroutine() {
        WaitForSeconds wait = new WaitForSeconds(20);
        while (isShiftSpeedCoolingDown) {
            yield return wait;
            uiManager.DecreaseThrusterSlider();
            isShiftSpeedCoolingDown = false;
        }
    }

    private void FireLaser() {
        canFire = Time.time + fireRate;

        if (Input.GetKeyDown(KeyCode.Space) && ammoCount >= 1) {
            if (isTripleShotPowerUpActive) {
                Instantiate(tripleShotLasersPrefab, transform.position, Quaternion.identity);
                SubtractAmmo(3);
            } else {
                Instantiate(laserPrefab, transform.position + new Vector3(0, 1.075f, 0), Quaternion.identity);
                SubtractAmmo(1);
            }
            audio.clip = laserClip;
            audio.Play();
        }

        if (Input.GetKeyDown(KeyCode.Space) && ammoCount <= 0) {
            audio.clip = outOfAmmoClip;
            audio.Play();
        }
    }

    public int GetAmmoCount() {
        return ammoCount;
    }

    public void AddAmmoCount(int count) {
        ammoCount += count;
        uiManager.UpdateAmmoCount(ammoCount);
    }

    public void SubtractAmmo(int ammo) {
        ammoCount -= ammo;
        if (ammoCount < 0)
            ammoCount = 0;
        uiManager.UpdateAmmoCount(ammoCount);
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
        health--;
        OnDamaged();
    }

    private void TakeShieldDamage() {
        currentShieldProtection--;
        UpdateShieldColor();
        if (currentShieldProtection <= 0)
            ShieldPowerDown();
        StartInvincibility(2);
    }

    //NOTE: This updates both us, AND other scripts, after we take damage.
    //This is IN RESPONSE to damage.
    private void OnDamaged() {
        uiManager.UpdateHealth(health);
        if (health == 2)
            wingDamageSprites[Random.Range(0, wingDamageSprites.Length)].enabled = true;
        else if (health == 1) {
            if (wingDamageSprites[0].enabled)
                wingDamageSprites[1].enabled = true;
            else wingDamageSprites[0].enabled = true;
        } else if (health <= 0) {
            spawnManager.OnPlayerDeath();
            uiManager.GameOverSequence();
            Destroy(this.gameObject);
            return;
        }
        StartInvincibility(2);
    }

    public void AddHealth() {
        if (health == 1 || health == 2) {
            health++;
            uiManager.UpdateHealth(health);
            if (wingDamageSprites[0].enabled)
                wingDamageSprites[0].enabled = false;
            else wingDamageSprites[1].enabled = false;
        }
    }

    private float GetBaseSpeed() {
        return 5;
    }

    public void TripleShotPowerUpActive() {
        isTripleShotPowerUpActive = true;
        StartCoroutine(TripleShotPowerDownCoroutine());
    }

    private IEnumerator TripleShotPowerDownCoroutine() {
        WaitForSeconds wait = new WaitForSeconds(5);
        while (isTripleShotPowerUpActive) {
            yield return wait;
            isTripleShotPowerUpActive = false;
        }
    }

    public void SpeedPowerUpActive() {
        isSpeedPowerUpActive = true;
        speed *= speedPowerUpMultiplier;
        thrusterSprite.color = Color.cyan;
        StartCoroutine(SpeedPowerDownCoroutine());
    }

    private IEnumerator SpeedPowerDownCoroutine() {
        WaitForSeconds wait = new WaitForSeconds(5);
        while (isSpeedPowerUpActive) {
            yield return wait;
            speed /= speedPowerUpMultiplier;
            thrusterSprite.color = Color.white;
            isSpeedPowerUpActive = false;
        }
    }

    private void UpdateShieldColor() {
        Color c = shieldVisualizer.color;
        c.a = (float) currentShieldProtection / totalShieldProtection;
        shieldVisualizer.color = c;
    }

    public void ShieldPowerUpActive() {
        currentShieldProtection = totalShieldProtection;
        UpdateShieldColor();
        shield.SetActive(true);
    }

    private void ShieldPowerDown() {
        shield.SetActive(false);
    }

    public void WavePowerUpActive() {
        isWavePowerUpActive = true;
        StartInvincibility(5);
        wave.SetActive(true);
        StartCoroutine(WavePowerDownCoroutine());
    }

    private IEnumerator WavePowerDownCoroutine() {
        WaitForSeconds wait = new WaitForSeconds(5);
        while (isWavePowerUpActive) {
            yield return wait;
            wave.SetActive(false);
            isWavePowerUpActive = false;
        }
    }

    public void StartInvincibility(int seconds) {
        timeInvincibleUntil = Time.time + seconds;
    }

    public void AddToScore(int points) {
        score += points;
        uiManager.UpdateScore(score);
    }
}
