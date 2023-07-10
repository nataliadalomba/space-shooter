using System.Collections;
using UnityEngine;

//TODO: Maybe append "Collectable" at the end of all the Collectable power up class names?
//(and then call this WavePowerUp)
public class WaveAttack : MonoBehaviour {
    [SerializeField] private HealthEntity health;
    [SerializeField] private GameObject wave;

    private bool isWavePowerUpActive;

    public bool IsWavePowerUpActive => isWavePowerUpActive;

    public void WavePowerUpActive() {
        StartCoroutine(WavePowerDownCoroutine());
        health.StartInvincibility(5);
        wave.SetActive(true);
    }

    private IEnumerator WavePowerDownCoroutine() {
        isWavePowerUpActive = true;
        yield return new WaitForSeconds(5);
        wave.SetActive(false);
        isWavePowerUpActive = false;
    }
}
