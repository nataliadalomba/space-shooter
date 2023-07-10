using UnityEngine;

public class DamageableWings : MonoBehaviour {
    [SerializeField] private HealthEntity target;
    [SerializeField] private SpriteRenderer[] wingDamageSprites = new SpriteRenderer[2];

    private void OnEnable() {
        target.onDamaged += UpdateFromDamage;
        target.onHealed += UpdateFromHealing;
    }

    private void OnDisable() {
        target.onDamaged -= UpdateFromDamage;
        target.onHealed -= UpdateFromHealing;
    }

    private void UpdateFromDamage() {
        int health = target.Health;

        //TODO: Support arbitrary number of wingDamageSprites
        if (health <= 1) {
            wingDamageSprites[0].enabled = true;
            wingDamageSprites[1].enabled = true;
        } else if (health == 2) {
            int index = Random.Range(0, wingDamageSprites.Length);
            wingDamageSprites[index].enabled = true;
            wingDamageSprites[1 - index].enabled = false;
        }
    }

    private void UpdateFromHealing() {
        if (wingDamageSprites[0].enabled)
            wingDamageSprites[0].enabled = false;
        else wingDamageSprites[1].enabled = false;
    }
}
