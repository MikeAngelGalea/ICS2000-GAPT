using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    // setting the maximum health for the health bar
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    // setting the current health for the health bar
    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
