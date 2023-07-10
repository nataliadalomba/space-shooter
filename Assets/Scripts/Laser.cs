using UnityEngine;

public class Laser : MonoBehaviour {

    [SerializeField] private float speed = 8;
    private bool isEnemyLaser;

    private void Update() {
        if (isEnemyLaser == false)
            PlayerLaser();
        else EnemyLaser();
    }

    private void PlayerLaser() {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        
        if (transform.position.y >= 8) {
            if(transform.parent != null) {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    private void EnemyLaser() {
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
            HealthEntity player = other.GetComponentInParent<HealthEntity>();

            if (player != null) {
                player.TryDamage();
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
