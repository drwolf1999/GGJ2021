using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    private PlayerCombat playerCombat;
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }
     
    public void SetHealth(int health)
    {
        slider.value = health;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    void Start()
    {
        playerCombat = GameObject.Find("Player").GetComponent<PlayerCombat>();
        SetMaxHealth(playerCombat.Health);
    }

    void Update()
    {
        SetHealth(playerCombat.Health);
        Debug.Log("slider" + slider.maxValue + " " + slider.value);
    }
}
