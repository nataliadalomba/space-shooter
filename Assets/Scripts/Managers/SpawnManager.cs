using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemyContainer;
    [SerializeField] private GameObject[] powerUps;
    [SerializeField] private GameObject[] collectables;

    private bool spawning = true;

    public void StartSpawning() {
        StartCoroutine(SpawnEnemyCoroutine());
        StartCoroutine(SpawnPowerUpCoroutine());
        StartCoroutine(SpawnCollectableCoroutine());
    }

    IEnumerator SpawnEnemyCoroutine() {
        WaitForSeconds wait = new WaitForSeconds(3);
        yield return wait;
        while (spawning) {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 8.5f, 0);
            GameObject newEnemy = Instantiate(enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = enemyContainer.transform;
            WaitForSeconds wait5Secs = new WaitForSeconds(5.0f);
            yield return wait5Secs;
        }
    }

    IEnumerator SpawnPowerUpCoroutine() {
        WaitForSeconds wait = new WaitForSeconds(3);
        yield return wait;
        while (spawning) {
            WaitForSeconds waitRandom = new WaitForSeconds(Random.Range(7f, 15f));
            yield return waitRandom;
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 8.5f, 0);
            int randomPowerUp = Random.Range(0, powerUps.Length);
            Instantiate(powerUps[randomPowerUp], posToSpawn, Quaternion.identity);
        }
    }

    IEnumerator SpawnCollectableCoroutine() {
        WaitForSeconds wait = new WaitForSeconds(3);
        yield return wait;
        while (spawning) {
            WaitForSeconds waitRandom = new WaitForSeconds(Random.Range(7f, 15f));
            yield return waitRandom;
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 8.5f, 0);
            int randomCollectable = Random.Range(0, collectables.Length);
            Instantiate(collectables[randomCollectable], posToSpawn, Quaternion.identity);
        }
    }

    public void OnPlayerDeath() {
        spawning = false;
    }
}
