using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private GameObject _player;

    /*we have an Enemy prefab in hierachy and we're reusing it
    so we don't need to instantiate or destroy it*/

    void Update() {
        //move down at 4 meters per second
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        /*if bottom of screen
        respawn at top with a new random x position*/
        if (transform.position.y <= -5f) {
            float randX = Random.Range(-12f, 12f);
            transform.position = new Vector3(randX, 7, 0);
        }
    }

    private void OnTriggerEnter(Collider other) {
        /*if other is player
        //damage the player
        destroy us*/
        if (other.tag == "Player") {
            /*typically best practice to create a variable out of a component you're
            going to be accessing. avoids more errors and easy to keep track*/
            Player player = other.transform.GetComponent<Player>();

            /*damage the player (take off 1 life)
            //the only component you can directly access is transform
            //if you want another component off an object, you need to access
            //the main root of thr object through transform, then .GetComponent<>
            //with the component inside. Then any methods in it (including if the
            //component is your script)
            //other.transform.GetComponent<Player>().Damage();

            //prepare to also null check to catch any null ref exceptions
            //when a component or object you're trying to access doesn't exist
            this ensures handling errors so your end user doesn'y crash*/
            if (player != null)
                player.Damage();
            Destroy(this.gameObject);
        }

        /*if other is laser
        //destroy laser
        destroy us*/
        if (other.tag == "Laser") {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
