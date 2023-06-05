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
        }
    }

    public void AssignEnemyLaser() {
        isEnemyLaser = true;
    }

    public bool IsEnemyLaser() {
        return isEnemyLaser;
    }

    //if the tag is player and it's an enemy laser
    //set player to the player component
    //check player is there, then TryDamage and destroy the laser

    //if other is a child of the object with tag Player
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
