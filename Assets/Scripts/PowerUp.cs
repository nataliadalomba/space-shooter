using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField] //0 = Triple Shot, 1 = Speed, 2 = Shield
    private int _powerUpID;

    void Update() {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y <= -6f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            Player player = other.GetComponent<Player>();
            if (player != null) {
                switch(_powerUpID) {
                    case 0:
                        player.TripleShotPowerUpActive();
                        break;
                    case 1:
                        player.SpeedPowerUpActive();
                        break;
                    case 2:
                        player.ShieldPowerUpActive();
                        break;
                    default:
                        Debug.Log("Invalid PowerUp ID");
                        break;
                }
            }
            Destroy(gameObject);
        }
    }
}
