using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public float health = 100f;
    public float maxHealth = 100f;
    public Image healthRing;
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (healthRing != null)
        {
            healthRing.fillAmount = health / maxHealth;
            Debug.Log("HEALTHBAR");
        }
            

        else
            Debug.Log("NO HEALTHBAR");
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
