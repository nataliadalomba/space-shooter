using UnityEngine;
using UnityEngine.UI;

public class ThrusterBar : MonoBehaviour {
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;

    private void Start() {
        SetThrusterSlider(0);
    }

    public void IncreaseThrusterSlider() {
        float addPerSecond = 0.1f;
        slider.value += addPerSecond * Time.deltaTime;
        Debug.Log("increasing slider " + slider.value);
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void DecreaseThrusterSlider() {
        float subtractPerSecond = 0.1f;
        slider.value -= subtractPerSecond * Time.deltaTime;
        Debug.Log("decreasing slider " + slider.value);
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetThrusterSlider(float amount) {
        float clampedAmount = Mathf.Clamp(amount, slider.minValue, slider.maxValue);
        slider.value = clampedAmount;
        fill.color = gradient.Evaluate(amount);
    }
}