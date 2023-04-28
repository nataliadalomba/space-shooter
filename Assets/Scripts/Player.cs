using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField]
    private float _speed = 4.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;

    void Start() {
        transform.position = Vector3.zero;
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

        //clamping the player bounds on the y axis
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.1f, 0), 0);

        //wrapping the player movement on the bounds of the screen for x axis
        if (transform.position.x >= 15.2f)
            transform.position = new Vector3(-15.2f, transform.position.y, 0);
        else if (transform.position.x <= -15.2f)
            transform.position = new Vector3(15.2f, transform.position.y, 0);
    }

    void FireLaser() {
        //player is only able to fire a laser at the fireRate
        _canFire = Time.time + _fireRate;

        //laser spawns with an offset of 0.8f in the y (above the player)
        Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
    }
}
