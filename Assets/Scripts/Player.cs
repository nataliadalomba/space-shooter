using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotLasersPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    private float _powerUpDuration = 5.0f;
    private bool _isTripleShotPowerUpActive;
    private bool _isSpeedPowerUpActive;
    private bool _isShieldPowerUpActive;

    private float _speedMultiplier = 2f;

    void Start() {
        transform.position = Vector3.zero;
        _spawnManager = GameObject.FindGameObjectWithTag("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
            Debug.LogError("The Spawn Manager is null.");
    }

    void Update() {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
            FireLaser();
    }

    void CalculateMovement() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);
        //if _isSpeedPowerUpActive
        //increase _speed to 8.5

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.6f, 0), 0);

        if (transform.position.x >= 11.3f)
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        else if (transform.position.x <= -11.3f)
            transform.position = new Vector3(11.3f, transform.position.y, 0);
    }

    void FireLaser() {
        _canFire = Time.time + _fireRate;

        if(Input.GetKeyDown(KeyCode.Space)) {
            if (_isTripleShotPowerUpActive)
                Instantiate(_tripleShotLasersPrefab, transform.position, Quaternion.identity);
            else Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.075f, 0), Quaternion.identity);
        }
    }

    //is public so Enemy script can communicate with this method
    //not directly accessing the _lives variable when enemy collides
    //with player (enemy has rb). bad practice to directly access
    //public variables. use methods!
    public void Damage() {
        if (_isShieldPowerUpActive) {
            _isShieldPowerUpActive = false;
            return;
        }
        else _lives--;

        if (_lives < 1) {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotPowerUpActive() {
        _isTripleShotPowerUpActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    
    IEnumerator TripleShotPowerDownRoutine() {
        while(_isTripleShotPowerUpActive) {
            yield return new WaitForSeconds(_powerUpDuration);
            _isTripleShotPowerUpActive = false;
        }
    }

    public void SpeedPowerUpActive() {
        _isSpeedPowerUpActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator SpeedPowerDownRoutine() {
        while(_isSpeedPowerUpActive) {
            yield return new WaitForSeconds(_powerUpDuration);
            _speed /= _speedMultiplier;
            _isSpeedPowerUpActive = false;
        }
    }

    public void ShieldPowerUpActive() {
        _isShieldPowerUpActive = true;
    }
}
