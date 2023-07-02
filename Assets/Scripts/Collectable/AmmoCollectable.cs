public class AmmoCollectable : Collectable {

    protected override void OnPickUp(Player player) {
        player.AddAmmoCount(5);
    }
}
