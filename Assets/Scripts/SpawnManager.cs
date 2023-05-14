using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject enemyContainer;
    [SerializeField]
    private GameObject[] powerUps;

    private bool spawning = true;

    void Start() {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator SpawnEnemyRoutine() {
        while (spawning) {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 8.5f, 0);
            GameObject newEnemy = Instantiate(enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnPowerUpRoutine() {
        while (spawning) {
            yield return new WaitForSeconds(Random.Range(7.0f, 15.0f));
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 8.5f, 0);
            int randomPowerUp = Random.Range(0, 3);
            Instantiate(powerUps[randomPowerUp], posToSpawn, Quaternion.identity);
        }
    }

    public void OnPlayerDeath() {
        spawning = false;
    }
}
