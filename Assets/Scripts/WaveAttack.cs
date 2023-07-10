using System.Collections;
using UnityEngine;

//TODO: Maybe append "Collectable" at the end of all the Collectable power up class names?
//(and then call this WavePowerUp)
public class WaveAttack : MonoBehaviour {
    [SerializeField] private HealthEntity health;
    [SerializeField] private GameObject wave;

    private bool isWavePowerUpActive;

    public void WavePowerUpActive() {
        isWavePowerUpActive = true;
        health.StartInvincibility(5);
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
}
