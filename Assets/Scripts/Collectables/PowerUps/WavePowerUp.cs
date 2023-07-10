using UnityEngine;

public class WavePowerUp : PowerUp {
    protected override void ApplyPowerUp(GameObject player) {
        if (player.TryGetComponent(out WaveAttack attack))
            attack.WavePowerUpActive();
    }
}
