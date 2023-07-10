using System;
using System.Collections;
using UnityEngine;

public class ShootController : MonoBehaviour {
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private AudioClip laserClip;
    [SerializeField] private AudioClip outOfAmmoClip;
    [SerializeField] private float fireRate = 0.15f;
    [SerializeField] private int ammoCount = 15;

    [Header("Triple-Shot Power-Up")]
    [SerializeField] private GameObject tripleShotLasersPrefab;

    private new AudioSource audio;
    private float canFire = -1;
    private bool isTripleShotPowerUpActive;

    public event Action onAmmoCountChanged;

    public int AmmoCount => ammoCount;

    private void Start() {
        audio = GetComponent<AudioSource>();

        if (audio == null)
            Debug.LogError("The AudioSource on the player is null.");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > canFire)
            FireLaser();
    }

    public void AddAmmoCount(int count) {
        ammoCount += count;
        if (onAmmoCountChanged != null)
            onAmmoCountChanged();
    }

    public void SubtractAmmo(int ammo) {
        ammoCount -= ammo;
        if (ammoCount < 0)
            ammoCount = 0;
        if (onAmmoCountChanged != null)
            onAmmoCountChanged();
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
}
