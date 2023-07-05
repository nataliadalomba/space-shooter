public class AmmoProvision : Collectable {

    protected override void OnPickUp(Player player) {
        player.AddAmmoCount(5);
    }
}
