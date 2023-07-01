using UnityEngine;

public class AmmoCollectable : Collectable {

    private Player player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void AddAmmo() {
        player.AddLaserCount(10);
        Debug.Log("added 10 ammo");

    }

    protected override void OnPickUp(Player player) {
        AddAmmo();
    }
}
