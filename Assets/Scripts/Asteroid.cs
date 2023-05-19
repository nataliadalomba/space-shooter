using UnityEngine;

public class Asteroid : MonoBehaviour {

    [SerializeField]
    private float speed = 25f;
    [SerializeField]
    private GameObject explosionPrefab;
    private SpawnManager spawnManager;

    private void Start() {
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (spawnManager == null)
            Debug.LogError("The Spawn Manager is null.");
    }

    private void Update() {
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Laser") {
            Destroy(other.gameObject);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            spawnManager.StartSpawning();
            Destroy(this.gameObject, .25f);
        }
    }
}
