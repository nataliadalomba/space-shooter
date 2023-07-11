using System.Collections;
using UnityEngine;

/// <summary>
/// Represents a player-controllable ship in the game, using <see cref="Input.GetAxis(string)"/> with <c>"Horizontal"</c> and <c>"Vertical"</c>.
/// </summary>
public class ShipMovementController2D : MonoBehaviour {
    [Tooltip("The player's currrent speed, measured in world-space units per second.")]
    [SerializeField, Min(0)] private float speed = 5;

    [Tooltip("The boosted speed at which you move after activating the speed boost (by using the Shift key).")]
    [SerializeField, Min(0)] private float speedBoost = 7;
    [SerializeField, Min(0.01f)] private float speedBoostDuration = 3;
    [SerializeField, Min(0.01f)] private float speedBoostCooldown = 20;

    [SerializeField] private float screenSizeX = 11.3f;

    [Header("Speed Power-Up")]
    [SerializeField] private SpriteRenderer thrusterSprite;
    [SerializeField] private float speedPowerUpMultiplier = 2;

    private float baseSpeed;
    private float timeUsedSpeedBoost = float.NegativeInfinity;
    private bool isSpeedPowerUpActive;

    public float Speed {
        get { return speed; }
        set { speed = Mathf.Max(0, value); }
    }

    public float SpeedBoostDuration => speedBoostDuration;
    public float SpeedBoostCooldown => speedBoostCooldown;

    public bool IsSpeedBoostActive => Time.time < (timeUsedSpeedBoost + speedBoostDuration);
    public bool IsSpeedBoostCoolingDown => Time.time < (timeUsedSpeedBoost + speedBoostCooldown);

    public float TimeUsedSpeedBoost => timeUsedSpeedBoost;
    public float RemainingSpeedBoostCooldown => Mathf.Max(0, (timeUsedSpeedBoost + speedBoostCooldown) - Time.time);

    private void Awake() {
        baseSpeed = speed;
    }

    private void Start() {
        transform.position = Vector3.zero;
    }

    private void Update() {
        CalculateMovement();
    }

    private void CalculateMovement() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0).normalized;
        StartSpeedBoost();

        Vector3 pos = transform.position + direction * speed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, -3.6f, 0);
        pos.z = 0;

        WrapX(ref pos);

        transform.position = pos;
    }

    //TODO: Write a new article! EPISODE 2! You CAN change structs by passing parameters by ref
    private void WrapX(ref Vector3 value) {
        if (value.x >= screenSizeX)
            value.x = -screenSizeX;
        else if (value.x <= -screenSizeX)
            value.x = screenSizeX;
    }

    private void StartSpeedBoost() {
        bool isSpeedBoostAvailable = !isSpeedPowerUpActive && !IsSpeedBoostActive && !IsSpeedBoostCoolingDown;
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && isSpeedBoostAvailable)
            StartCoroutine(SpeedBoostCoroutine());
    }

    private IEnumerator SpeedBoostCoroutine() {
        timeUsedSpeedBoost = Time.time;
        while (IsSpeedBoostActive) {
            speed = speedBoost;
            yield return null;
        }
        speed = baseSpeed;

        //NOTE: If we want to do something during/after cooldown, use this:
        //while (IsSpeedBoostCoolingDown)
        //    yield return null;
    }

    public void SpeedPowerUpActive() {
        isSpeedPowerUpActive = true;
        speed *= speedPowerUpMultiplier;
        thrusterSprite.color = Color.cyan;
        StartCoroutine(SpeedPowerDownCoroutine());
    }

    private IEnumerator SpeedPowerDownCoroutine() {
        WaitForSeconds wait = new WaitForSeconds(5);
        while (isSpeedPowerUpActive) {
            yield return wait;
            speed /= speedPowerUpMultiplier;
            thrusterSprite.color = Color.white;
            isSpeedPowerUpActive = false;
        }
    }
}
