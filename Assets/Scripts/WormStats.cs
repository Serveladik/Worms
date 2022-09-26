using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WormStats : MonoBehaviour
{
    public int maxHealth = 100;
    int health;

    [SerializeField] TextMeshProUGUI healthText;

    public void ChangeHealth()
    {
        health = maxHealth;
        healthText.text = health.ToString();
    }

    public void ChangeHealth(int amount)
    {
        health += amount;

        if(health > maxHealth)
        {
            health = maxHealth;
        }
        else if(health <= 0)
        {
            health = 0; 
        }
        healthText.text = health.ToString();
    }
}
