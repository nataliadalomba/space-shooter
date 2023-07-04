using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    [System.Serializable]
    private struct SpawnInfo {
        public GameObject prefab;

        public float minSpawnTime;
        public float maxSpawnTime;
    }

    [SerializeField] private SpawnInfo[] powerUps = {
        new SpawnInfo {
            minSpawnTime = 7,
            maxSpawnTime = 15
        }
    };

    [SerializeField] private SpawnInfo[] provisions = {
        new SpawnInfo {
            minSpawnTime = 5,
            maxSpawnTime = 14
        }
    };

    [Space(20)]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemyContainer;
    //[SerializeField] private GameObject[] powerUps;
    //[SerializeField] private GameObject[] provisions;

    private bool spawning = true;

    public void StartSpawning() {
        StartCoroutine(SpawnEnemyCoroutine());
        StartCoroutine(SpawnPowerUpCoroutine());
        StartCoroutine(SpawnProvisionCoroutine());
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
            WaitForSeconds waitRandom = new WaitForSeconds(Random.Range(0.5f, 1f));
            yield return waitRandom;
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 8.5f, 0);
            int randomPowerUp = Random.Range(0, powerUps.Length);
            Instantiate(powerUps[randomPowerUp], posToSpawn, Quaternion.identity);
        }
    }

    IEnumerator SpawnProvisionCoroutine() {
        WaitForSeconds wait = new WaitForSeconds(3);
        yield return wait;
        while (spawning) {
            WaitForSeconds waitRandom = new WaitForSeconds(Random.Range(7f, 15f));
            yield return waitRandom;
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 8.5f, 0);
            int randProvision = Random.Range(0, provisions.Length);
            Instantiate(provisions[randProvision], posToSpawn, Quaternion.identity);
        }
    }

    public void OnPlayerDeath() {
        spawning = false;
    }
}
