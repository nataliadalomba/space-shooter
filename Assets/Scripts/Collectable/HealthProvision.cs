public class HealthProvision : Collectable {

    protected override void OnPickUp(Player player) {
        player.AddHealth();
    }
}
