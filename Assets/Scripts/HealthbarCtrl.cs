using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarCtrl : MonoBehaviour
{
    Image healthBar;

    float maxHealth = 10f;

    public static float health;

    void Start()
    {
        healthBar = GetComponent<Image>();
        health = maxHealth;
    }

    void Update()
    {
        //updates the health bar
        healthBar.fillAmount = health / maxHealth;
    }
}
