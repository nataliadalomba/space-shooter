using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    private float _speed = 4f;

    private Player _player;

    private void Start() {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update() {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= -7f) {
            float randX = Random.Range(-10f, 10f);
            transform.position = new Vector3(randX, 9, 0);
        }
    }

    private void OnTriggerEnter2D (Collider2D other) {
        if (other.tag == "Player") {
            _player = other.transform.GetComponent<Player>();

            if (_player != null)
                _player.Damage();
            Destroy(this.gameObject);
        }

        if (other.tag == "Laser") {
            Destroy(other.gameObject);
            if(_player != null)
                _player.AddToScore(10);
            Destroy(this.gameObject);
        }
    }
}
