using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTarget : MonoBehaviour
{
    public float health = 100f;
    public float maxHealth = 100f;
   // public Image healthRing;
    
    public void Start()
    {
        //healthRing = GetComponent<Image>();
    }
    public void TakeDamage(float amount)
    {
        //healthRing.fillAmount = health / maxHealth;
        //Debug.Log(healthRing.fillAmount);
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
