public class ShieldPowerUp : PowerUp {
    protected override void ApplyPowerUp(Player player) {
        player.ShieldPowerUpActive();
    }
}