using UnityEngine;

public class HealthProvision : Collectable {

    protected override void OnPickUp(GameObject player) {
        if (player.TryGetComponent(out HealthEntity entity))
            entity.Heal();
    }
}
