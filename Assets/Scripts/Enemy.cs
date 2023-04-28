using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private GameObject _player;

    //we have an Enemy prefab in hierachy and we're reusing it
    //so we don't need to instantiate or destroy it

    void Update() {
        //move down at 4 meters per second
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        //if bottom of screen
        //respawn at top with a new random x position
        if (transform.position.y <= -5f) {
            float randX = Random.Range(-12f, 12f);
            transform.position = new Vector3(randX, 7, 0);
        }
    }

    private void OnTriggerEnter(Collider other) {
        //if other is player
        //damage the player (eventually)
        //destroy us
        if (other.tag == "Player") {

            Destroy(this.gameObject);
        }

        //if other is laser
        //destroy laser
        //destroy us
        if (other.tag == "Laser") {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
