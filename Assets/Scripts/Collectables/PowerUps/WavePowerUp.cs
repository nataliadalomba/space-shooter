public class WavePowerUp : PowerUp {
    protected override void ApplyPowerUp(Player player) {
        player.WavePowerUpActive();
    }
}