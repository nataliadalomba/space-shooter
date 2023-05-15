using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    [SerializeField]
    private float speed = 4f;

    private Player player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update() {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        if (transform.position.y <= -7f) {
            float randX = Random.Range(-10f, 10f);
            transform.position = new Vector3(randX, 9, 0);
        }
    }

    private void OnTriggerEnter2D (Collider2D other) {
        if (other.tag == "Player") {
            player.Damage();
            Destroy(this.gameObject);
        }

        if (other.tag == "Laser") {
            Destroy(other.gameObject);
            if (player != null)
                player.AddToScore(10);
            Destroy(this.gameObject);
        }
    }
}
