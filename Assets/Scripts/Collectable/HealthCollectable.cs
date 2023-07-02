public class HealthCollectable : Collectable {

    protected override void OnPickUp(Player player) {
        player.AddHealth();
    }
}
