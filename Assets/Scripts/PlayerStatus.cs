using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth = 100;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log("현재 체력 : "+ currentHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if ((currentHealth <=0))
        {
            currentHealth = 0;
            Debug.Log("사망");
        }
        Debug.Log("현재 체력: " + currentHealth);
    }
}
