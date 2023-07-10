using UnityEngine;

public class TripleShotPowerUp : PowerUp {
    protected override void ApplyPowerUp(GameObject player) {
        if (player.TryGetComponent(out ShootController controller))
            controller.TripleShotPowerUpActive();
    }
}
