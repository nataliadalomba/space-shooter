using UnityEngine;

public class SpeedPowerUp : PowerUp {
    protected override void ApplyPowerUp(GameObject player) {
        if (player.TryGetComponent(out ShipMovementController2D moveController))
            moveController.SpeedPowerUpActive();
    }
}
