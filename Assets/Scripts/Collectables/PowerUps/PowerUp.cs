using UnityEngine;

public class PowerUp : Collectable {

    protected override void OnPickUp(GameObject player) {
        ApplyPowerUp(player);
    }

    //TODO: Make abstract later
    protected virtual void ApplyPowerUp(GameObject player) { }
}
