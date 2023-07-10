using UnityEngine;

public class AmmoProvision : Collectable {

    protected override void OnPickUp(GameObject player) {
        if (player.TryGetComponent(out ShootController controller))
            controller.AddAmmoCount(5);
    }
}
