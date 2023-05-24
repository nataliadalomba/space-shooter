using UnityEngine;

public class Laser : MonoBehaviour {

    [SerializeField]
    private float speed = 8f;
    private bool isEnemyLaser;
    
    void Update() {
        if (isEnemyLaser == false)
            MoveUp();
        else MoveDown();
    }

    void MoveUp() {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        
        if (transform.position.y >= 8) {
            if(transform.parent != null) {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    void MoveDown() {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y <= -8) {
            if (transform.parent != null) {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser() {
        isEnemyLaser = true;
    }

    public bool IsEnemyLaser() {
        return isEnemyLaser;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" && isEnemyLaser) {
            Player player = other.GetComponent<Player>();

            if (player != null) {
                player.Damage();
                Destroy(this.gameObject);
            }
        }
    }
}
