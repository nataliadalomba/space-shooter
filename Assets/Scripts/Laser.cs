﻿using UnityEngine;

public class Laser : MonoBehaviour {

    [SerializeField] private float speed = 8f;
    [SerializeField] private int playerLaserCount = 15;
    private bool isEnemyLaser;

    
    void Update() {
        if (isEnemyLaser == false)
            PlayerLaser();
        else EnemyLaser();
    }

    void PlayerLaser() {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        
        if (transform.position.y >= 8) {
            if(transform.parent != null) {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    void EnemyLaser() {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y <= -8) {
            if (transform.parent != null) {
                Destroy(transform.parent.gameObject);
            }
        }
    }

    public void AssignEnemyLaser() {
        isEnemyLaser = true;
    }

    public bool IsEnemyLaser() {
        return isEnemyLaser;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (isEnemyLaser) {
            Player player = other.GetComponentInParent<Player>();

            if (player != null) {
                player.TryDamage();
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
