using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    [SerializeField]
    private float _speed = 8f;
    
    void Update() {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        
        if (transform.position.y >= 8) {
            //check if this object has a parent
            //if it does, destroy the parent object too
            if(transform.parent != null) {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
