using UnityEngine;

public class ShieldPowerUp : PowerUp {
    protected override void ApplyPowerUp(GameObject player) {
        if (player.TryGetComponent(out HealthEntity entity))
            entity.ShieldPowerUpActive();
    }
}
