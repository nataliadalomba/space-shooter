using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] private float speed = 3.5f;
    void Start() {
        //take the current position = new position (0, 0, 0)
        transform.position = Vector3.zero;
    }

    void Update() {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}
