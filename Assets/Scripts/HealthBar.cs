using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider Slider;
    public Gradient Gradient;
    public Image Fill;
    public float SmoothSpeed = 0.1f;

    private float targetHealth;

    void Update()
    {
        if (!(Mathf.Abs(Slider.value - targetHealth) > 0.01f)) 
            return;

        Slider.value = Mathf.Lerp(Slider.value, targetHealth, SmoothSpeed);
        Fill.color = Gradient.Evaluate(Slider.normalizedValue);
    }

    public void SetMaxHealth(int health)
    {
        Slider.maxValue = health;
        Slider.value = health;
        targetHealth = health;

        Fill.color = Gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        targetHealth = health;
    }
}
