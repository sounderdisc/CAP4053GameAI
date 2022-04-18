using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public Image healthRing;
    public Target target;
    // Update is called once per frame
    void Update()
    {
        healthRing.fillAmount = target.health / maxHealth;
    }
}
