public class PowerUp : Collectable {

    protected override void OnPickUp(Player player) {
        ApplyPowerUp(player);
    }

    //TODO: Make abstract later
    protected virtual void ApplyPowerUp(Player player) { }
}
