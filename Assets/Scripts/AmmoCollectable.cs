using UnityEngine;

public class AmmoCollectable : Collectable {

    private Player player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    protected void AddAmmo() {
        player.AddLaserCount(5);
    }

    protected override void OnPickUp(Player player) {
        AddAmmo();
    }
}
