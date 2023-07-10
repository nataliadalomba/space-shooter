using System;
using UnityEngine;

public class HealthEntity : MonoBehaviour {
    [SerializeField, Min(1)] private int health = 3;

    [Header("Shield Protection")]
    [SerializeField] private SpriteRenderer shield;
    [SerializeField] private int totalShieldProtection = 3;

    private int maxHealth;
    private float timeInvincibleUntil = 0;
    private int currentShieldProtection;

    public event Action onDamaged;
    public event Action onHealed;
    public event Action onHealthChanged; //NOTE: Useful for UI in the future

    public int Health => health;
    public int MaxHealth => maxHealth;
    public bool IsInvincible => Time.time <= timeInvincibleUntil;
    public bool IsShieldPowerUpActive => currentShieldProtection > 0;

    private void Awake() {
        maxHealth = health;
    }

    public bool TryDamage() {
        if (IsInvincible)
            return false;

        if (IsShieldPowerUpActive)
            TakeShieldDamage();
        else Damage();
        return true;
    }

    private void Damage() {
        health--;
        OnDamaged();
        if (onDamaged != null)
            onDamaged();
        if (onHealthChanged != null)
            onHealthChanged();
    }

    //NOTE: This updates both us, AND other scripts, after we take damage.
    //This is IN RESPONSE to damage.
    private void OnDamaged() {
        if (health <= 0) {
            //NOTE: This would be problematic for our UI Manager script,
            //      who still needs to access the player's health briefly first.
            //      So let's destroy after a short delay.
            Destroy(gameObject, 0.1f);
            return;
        }

        StartInvincibility(2);
    }

    public void Heal() {
        if (health == 1 || health == 2) {
            health++;
            if (onHealed != null)
                onHealed();
            if (onHealthChanged != null)
                onHealthChanged();
        }
    }

    public void StartInvincibility(int seconds) {
        timeInvincibleUntil = Time.time + seconds;
    }

    private void TakeShieldDamage() {
        currentShieldProtection--;
        UpdateShieldColor();
        if (currentShieldProtection <= 0)
            ShieldPowerDown();
        StartInvincibility(2);
    }

    private void UpdateShieldColor() {
        Color c = shield.color;
        c.a = (float) currentShieldProtection / totalShieldProtection;
        shield.color = c;
    }

    public void ShieldPowerUpActive() {
        currentShieldProtection = totalShieldProtection;
        UpdateShieldColor();
        shield.gameObject.SetActive(true);
    }

    private void ShieldPowerDown() {
        shield.gameObject.SetActive(false);
    }
}
