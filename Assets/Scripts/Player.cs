﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private float _speed = 4.5f;

    void Start() {
        transform.position = Vector3.zero;
    }

    void Update() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        //if player position on the y is greater than 0
        //y position = 0
        //else if position on the y is less than -2.7f
        //y pos = -2.7f

        if (transform.position.y >= 0) {
            transform.position = new Vector3(transform.position.x, 0, 0);
        } else if (transform.position.y <= -3.9f) {
            transform.position = new Vector3(transform.position.x, -3.9f, 0);
        }

        //if player on the x greater than 12.8
        //x pos = -12.8
        //else if player on the x is less than -12.8
        //x pos = 12.8

        if (transform.position.x >= 15.2f) {
            transform.position = new Vector3(-15.2f, transform.position.y, 0);
        } else if (transform.position.x <= -15.2f) {
            transform.position = new Vector3(15.2f, transform.position.y, 0);
        }
    }
}
