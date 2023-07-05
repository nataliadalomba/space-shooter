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
    
    [Space(20)]
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
            int randPowerUp = Random.Range(0, powerUps.Length);
            WaitForSeconds waitRandom = new WaitForSeconds(Random.Range(powerUps[randPowerUp].minSpawnTime,
                powerUps[randPowerUp].maxSpawnTime + 1));
            yield return waitRandom;
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 8.5f, 0);
            Instantiate(powerUps[randPowerUp].prefab, posToSpawn, Quaternion.identity);
        }
    }

    IEnumerator SpawnProvisionCoroutine() {
        WaitForSeconds wait = new WaitForSeconds(3);
        yield return wait;
        while (spawning) {
            int randProvision = Random.Range(0, provisions.Length);
            WaitForSeconds waitRandom = new WaitForSeconds(Random.Range(provisions[randProvision].minSpawnTime,
                provisions[randProvision].maxSpawnTime + 1));
            yield return waitRandom;
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 8.5f, 0);
            Instantiate(provisions[randProvision].prefab, posToSpawn, Quaternion.identity);
        }
    }

    public void OnPlayerDeath() {
        spawning = false;
    }
}
