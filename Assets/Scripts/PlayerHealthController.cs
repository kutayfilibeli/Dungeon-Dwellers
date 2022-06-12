using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    public int currentHealth;
    public int maxHealth;

    public float damageInvincibilityLength = 1f;
    private float invincibilityCount;

   
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = Tracker.instance.maxHealth;
        currentHealth = Tracker.instance.currentHealth;
        

        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(invincibilityCount > 0)
        {
            invincibilityCount -= Time.deltaTime;

            if(invincibilityCount <= 0)
            {
                PlayerController.instance.bodySR.color = new Color(PlayerController.instance.bodySR.color.r, PlayerController.instance.bodySR.color.g, PlayerController.instance.bodySR.color.b, 1f);
            }
        }
     
        
    }

    public void DamagePlayer()
    {
        if(invincibilityCount <= 0)
        {
            AudioManager.instance.playSFX(10);

            currentHealth--;

            invincibilityCount = damageInvincibilityLength;

            PlayerController.instance.bodySR.color = new Color(PlayerController.instance.bodySR.color.r, PlayerController.instance.bodySR.color.g, PlayerController.instance.bodySR.color.b, .5f);

        if (currentHealth <= 0)
        {
            PlayerController.instance.gameObject.SetActive(false);
            
            UIController.instance.DeathScreen.SetActive(true);

                AudioManager.instance.playGameOver();

                AudioManager.instance.playSFX(9);
            }

        

        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        }
    }

    public void invincibleWhileDashing(float length)
    {
        invincibilityCount = length;
        PlayerController.instance.bodySR.color = new Color(PlayerController.instance.bodySR.color.r, PlayerController.instance.bodySR.color.g, PlayerController.instance.bodySR.color.b, .5f);
    }

    public void HealPlayer(int healingAmount)
    {
        currentHealth += healingAmount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;


        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }
}
